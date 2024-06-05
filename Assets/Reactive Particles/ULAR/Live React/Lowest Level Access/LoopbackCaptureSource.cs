using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.Streams;
using CSCore.Utils;
using CSCore.SoundIn;

namespace ULAR {

	public class LoopbackCaptureSource : SpectrumSource
	{
		private struct SpectrumIndices
		{
			public int Lower;
			public int Upper;
		}

		private double[] _spectrumData = new double[0];
		private SpectrumIndices _spectrumIndices;

		public WasapiLoopbackCapture _receiveAudio;
		private ISampleSource _sampleSource;
		private MMDeviceEnumerator _deviceEnumerator;
		private FftProvider _fftProvider;
		private SingleBlockNotificationStream _blockStream;
		private MMNotificationClient _notificationClient;

		/// Automatically start reading on startup
		public bool AutoStart = true;

		#region Properties


		/// The ISoundIn of the object
		public ISoundIn SoundIn
		{
			get
			{
				return _receiveAudio;
			}
		}


		/// How many FFT samples to generate
		public FftSize FftSize = FftSize.Fft2048;
		private FftSize _oldFftSize;

		[SerializeField]

		/// Minimum frequency to capture
		public int MinFrequency = 20;
		private int _oldMinFrequency;

		[SerializeField]

		/// Maximum frequency to capture
		public int MaxFrequency = 20000;
		private int _oldMaxFrequency;


		/// The absolute minimum value a frequency can be at
		public float MinimumValue = 0.000001f;


		/// Amount of frequencies in the selected range
		public int FreqRange
		{
			get
			{
				return MaxFrequency - MinFrequency;
			}
		}


		/// Gets whether or not the audio capture source is playing
		public bool IsPlaying
		{
			get
			{
				return _receiveAudio != null && _receiveAudio.RecordingState == RecordingState.Recording;
			}
		}

		#endregion

		private void OnDisable()
		{
			Debug.Log("SpectrumSource disabling");
			Stop();
		}

		private void OnEnable()
		{
			if (AutoStart)
				Begin();
		}

		private void Awake()
		{
			_deviceEnumerator = new MMDeviceEnumerator();
			_notificationClient = new MMNotificationClient(_deviceEnumerator);
			_notificationClient.DeviceStateChanged += DeviceStateChanged;
		}

		// Use this for initialization
		void Start()
		{
			CaptureSource = this;
		}

		// Update is called once per frame
		void Update()
		{

			if (_oldMinFrequency != MinFrequency ||
				_oldMaxFrequency != MaxFrequency ||
				_oldFftSize != FftSize)
			{
				Begin();
				UpdateOld();
			}

			if (IsPlaying)
				UpdateSpectrum();
		}

		/// Find the correct device, set it up, and begin capture
		public void Begin()
		{
			if (IsPlaying)
				Stop();

			var device = GetDevice();

			Debug.Log("Starting capture with device: " + device.FriendlyName);

			_receiveAudio = new WasapiLoopbackCapture()
			{
				Device = device
			};
			_receiveAudio.DataAvailable += CaptureDataAvailable;
			_receiveAudio.Initialize();

			_sampleSource = new SoundInSource(_receiveAudio).ToSampleSource();

			UpdateOld();

			_fftProvider = new FftProvider(_sampleSource.WaveFormat.Channels, FftSize);
			_blockStream = new SingleBlockNotificationStream(_sampleSource);
			_blockStream.SingleBlockRead += SingleBlockRead;

			_receiveAudio.Start();
		}


		/// Stop the capture safely
		public void Stop()
		{
			if (_receiveAudio != null)
				_receiveAudio.Dispose();

			if (_blockStream != null)
				_blockStream.Dispose();

			Debug.Log("Stopped capturing.");
		}


		/// Gets the spectrum data from the audio capture
		public override double[] GetSpectrumData()
		{
			return _spectrumData;
		}


		/// Gets the audio device to use for streaming
		private MMDevice GetDevice(string guid = null)
		{
			// TODO: get device from settings
			if (string.IsNullOrEmpty(guid))
			{
				var devices = new List<MMDevice>(_deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active));
				if (devices.Exists(d => { return d.DeviceID == guid; }))
					return devices.Find(d => { return d.DeviceID == guid; });   // i really wish i could avoid typing this predicate twice

			}
			return _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
		}


		/// Updates the spectrum with the latest spectrum data
		private void UpdateSpectrum()
		{
			var buffer = new Complex[(int)FftSize];

			if (!_fftProvider.GetFftData(buffer))
				return; // if no data was read, return

			var fft = new double[buffer.Length];
			for (int i = 0; i < buffer.Length; i++)
			{
				fft[i] = buffer[i].Value;
			}

			var indices = _spectrumIndices;
			var usable = new double[indices.Upper - indices.Lower];
			for (int i = indices.Lower; i < indices.Upper; i++)
			{
				usable[i - indices.Lower] = fft[i];
			}

			for (int i = 0; i < usable.Length; i++) // loop through usable values to clean them up
			{
				usable[i] = Math.Max(MinimumValue, usable[i]);
			}

			_spectrumData = usable;
		}


		/// Calculates the point in the raw fft data where our frequency lies, based on sample rate
		public int GetFrequencyIndex(int frequency)
		{
			// https://stackoverflow.com/questions/4364823/how-do-i-obtain-the-frequencies-of-each-value-in-an-fft

			double f = _sampleSource.WaveFormat.SampleRate / 2d;
			return (int)((frequency / f) * ((int)FftSize / 2));
		}


		/// Update old values since the editor doesn't support properties
		private void UpdateOld()
		{
			Debug.Log("LoopbackCapture properties changed, updating...");

			_oldMinFrequency = MinFrequency;
			_oldMaxFrequency = MaxFrequency;
			_oldFftSize = FftSize;
			_spectrumIndices = new SpectrumIndices()
			{
				Lower = GetFrequencyIndex(MinFrequency),
				Upper = GetFrequencyIndex(MaxFrequency)
			};
		}

		#region Events

		private void DeviceStateChanged(object sender, DeviceStateChangedEventArgs e)
		{
			Debug.Log("Device changed: " + e.DeviceId);

			if (IsPlaying)
				Begin();    // restart device
		}

		private void SingleBlockRead(object sender, SingleBlockReadEventArgs e)
		{
			if (e.Channels > 2)
				_fftProvider.Add(e.Samples, e.Samples.Length);
			else
				_fftProvider.Add(e.Left, e.Right);
		}

		private void CaptureDataAvailable(object sender, DataAvailableEventArgs e)
		{
			int sampleCount = e.ByteCount / e.Format.BytesPerSample;
			var buffer = new float[sampleCount];

			// read data so that the blockstream keeps populating
			while (_blockStream.Read(buffer, 0, buffer.Length) > 0) ;
		}

		private void CaptureStopped(object sender, StoppedEventArgs e)
		{
			Debug.Log("WASAPI capture stopped.");
		}

		#endregion
	}
}
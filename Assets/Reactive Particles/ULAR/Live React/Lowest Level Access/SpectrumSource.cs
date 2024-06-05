using UnityEngine;

/// A class that derives from MonoBehaviour that forces derived classes to implement it's methods
/// It's like an interface but Unity-friendly

namespace ULAR {

    public abstract class SpectrumSource : MonoBehaviour
    {
        /// Gets an array of FFT data from the source
        abstract public double[] GetSpectrumData();

        /// The original capture source
        [HideInInspector]
        public LoopbackCaptureSource CaptureSource;
    }
}
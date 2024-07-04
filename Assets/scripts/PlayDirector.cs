using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR;

public class PlayDirector : MonoBehaviour
{
    public PlayableDirector director;
    public AudioSource typhoonSongM;
    public GameObject controlPanel;
    public GameObject plants;

    [SerializeField]
    private ParticleSystem burstParticles;

    [SerializeField]
    private ParticleSystem burstParticles2;

    [SerializeField]
    private ParticleSystem burstParticles3;

    [SerializeField]
    private ParticleSystem burstParticles4;

    [SerializeField]
    private ParticleSystem burstParticles5;

    [SerializeField]
    private ParticleSystem burstParticles6;

    [SerializeField]
    private ParticleSystem burstParticles7;

    [SerializeField]
    private ParticleSystem burstParticles8;

    [SerializeField]
    private ParticleSystem burstParticles9;

    [SerializeField]
    private ParticleSystem burstParticles10;

    [SerializeField]
    private ParticleSystem burstParticles11;

    [SerializeField]
    private ParticleSystem burstParticles12;

    [SerializeField]
    private ParticleSystem burstParticles13;

    [SerializeField]
    private ParticleSystem burstParticles14;

    [SerializeField]
    private ParticleSystem burstParticles15;

    [SerializeField]
    private ParticleSystem burstParticles16;

    [SerializeField]
    private ParticleSystem burstParticles17;

    [SerializeField]
    private ParticleSystem burstParticles18;

    [SerializeField]
    private ParticleSystem burstParticles19;

    [SerializeField]
    private ParticleSystem burstParticles20;


    public GameObject stephTestMoe;

    public GameObject fireWorks;


    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        typhoonSongM = GetComponentInParent<AudioSource>();
        //director.played += Director_Played;
        //director.stopped += Director_Stopped;
    }

    private void Update()
    {
        // Check for the B button press on the Quest 3 controller
        if (IsBButtonPressed())
        {
            StartTimeline();
            plants.SetActive(true);
            
        }

        ////This was fo Debugging Lidia's Effects on the headset
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log("Im a bitch!");
            // PlaySongWithLidia();
            StartTimeline();
        }
    }

    //private void Director_Stopped(PlayableDirector obj)
    //{
    //    controlPanel.SetActive(true);
    //}

    //private void Director_Played(PlayableDirector obj)
    //{
    //    controlPanel.SetActive(false);
    //}

    public void StartTimeline()
    {
        typhoonSongM.Play();
        director.Play();
        StartCoroutine(StephParticleCourotine());
        StartCoroutine(FireWorksCourtine());
        
        //PlaySongWithLidia();
    }

    IEnumerator FireWorksCourtine()
    {
        yield return new WaitForSeconds(4f);
        stephTestMoe.SetActive(true);
    }

    IEnumerator StephParticleCourotine()
    {
        yield return new WaitForSeconds(81.20f);
        fireWorks.SetActive(true);
    }


    private bool IsBButtonPressed()
    {
        // Check if the B button is pressed on the right hand controller
        bool bButtonPressed = false;
        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightHandDevice.isValid)
        {
            rightHandDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bButtonPressed);
        }

        return bButtonPressed;
    }


    public void PlaySongWithLidia()
    {
        typhoonSongM.Play();
        StartCoroutine(BurstParticles1());
        StartCoroutine(BurstParticles2());
        //StartCoroutine(BurstParticles3());
        //StartCoroutine(BurstParticles4());
        StartCoroutine(BurstParticles5());
        //StartCoroutine(BurstParticles6());
        //StartCoroutine(BurstParticles7());
        StartCoroutine(BurstParticles8());
        //StartCoroutine(BurstParticles9());
        //StartCoroutine(BurstParticles10());
        StartCoroutine(BurstParticles11());
        //StartCoroutine(BurstParticles12());
        StartCoroutine(BurstParticles13());
        //StartCoroutine(BurstParticles14());
        StartCoroutine(BurstParticles15());
        StartCoroutine(BurstParticles16());
        StartCoroutine(BurstParticles17());
        StartCoroutine(BurstParticles18());
        StartCoroutine(BurstParticles19());
        StartCoroutine(BurstParticles20());

    }


    IEnumerator BurstParticles1()
    {
        yield return new WaitForSeconds(61.40f);
        burstParticles.gameObject.SetActive(true);
        
    }

    IEnumerator BurstParticles2()
    {
        yield return new WaitForSeconds(63.32f);
        burstParticles2.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles3()
    {
        yield return new WaitForSeconds(63.41f);
        burstParticles3.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles4()
    {
        yield return new WaitForSeconds(64.29f);
        burstParticles4.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles5()
    {
        yield return new WaitForSeconds(65.34f);
        burstParticles5.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles6()
    {
        yield return new WaitForSeconds(66.40f);
        burstParticles6.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles7()
    {
        yield return new WaitForSeconds(68.42f);
        burstParticles7.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles8()
    {
        yield return new WaitForSeconds(69.25f);
        burstParticles8.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles9()
    {
        yield return new WaitForSeconds(70.31f);
        burstParticles9.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles10()
    {
        yield return new WaitForSeconds(71.40f);
        burstParticles10.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles11()
    {
        yield return new WaitForSeconds(72.80f);
        burstParticles11.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles12()
    {
        yield return new WaitForSeconds(74.26f);
        burstParticles12.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles13()
    {
        yield return new WaitForSeconds(75.34f);
        burstParticles13.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles14()
    {
        yield return new WaitForSeconds(76.39f);
        burstParticles14.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles15()
    {
        yield return new WaitForSeconds(79.27f);
        burstParticles15.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles16()
    {
        yield return new WaitForSeconds(79.37f);
        burstParticles16.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles17()
    {
        yield return new WaitForSeconds(80.24f);
        burstParticles17.gameObject.SetActive(true);
    }


    IEnumerator BurstParticles18()
    {
        yield return new WaitForSeconds(80.34f);
        burstParticles18.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles19()
    {
        yield return new WaitForSeconds(81.20f);
        burstParticles19.gameObject.SetActive(true);
    }

    IEnumerator BurstParticles20()
    {
        yield return new WaitForSeconds(81.30f);
        burstParticles20.gameObject.SetActive(true);
    }


   


}

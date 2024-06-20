using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class TimeLineStarter : MonoBehaviour
{

    // Static instance to allow access from other scripts
    public static TimeLineStarter Instance { get; private set; }

    [SerializeField] PlayableDirector playableDirector;

    void Awake()
    {
        // Ensure that there is only one instance of TimeLineStarter
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playableDirector = FindAnyObjectByType<PlayDirector>().GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the spacebar is pressed
       
    }

    public void StartTimeline()
    {
        // Check if the PlayableDirector is assigned
        if (playableDirector != null)
        {
            // Play the Timeline
            playableDirector.Play();
        }
        else
        {
            Debug.LogWarning("PlayableDirector is not assigned.");
        }
    }

}

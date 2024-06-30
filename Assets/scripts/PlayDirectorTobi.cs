using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayDirectorTobi : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director;
    // Start is called before the first frame update
    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void StartTimelineTobi()
    {
        director.Play();
    }
}

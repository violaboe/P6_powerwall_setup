using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class PoseEventManager : MonoBehaviour
{
    [System.Serializable]
    private class PoseEvent
    {
        public string eventName;
        public UnityEvent[] poseEvents;
    }

    [SerializeField]
    private List<PoseEvent> poseEventsList;
    public static event Action<Vector3> OnAttachHand;

    // Start is called before the first frame update
    void Start()
    {
        // Example of how to invoke all UnityEvents for a specific named event
        InvokePoseEvents("ExampleEventName");
    }

    public void InvokePoseEvents(string eventName)
    {
        PoseEvent namedEvent = poseEventsList.Find(e => e.eventName == eventName);
        if (namedEvent != null)
        {
            foreach (UnityEvent poseEvent in namedEvent.poseEvents)
            {
                poseEvent.Invoke();
            }
        }
        else
        {
            Debug.LogWarning($"Event with name {eventName} not found.");
        }
    }

    public void RaiseAttachToHandEvent(Vector3 position)
    {
        OnAttachHand?.Invoke(position);
    }
}

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MyEventReceiver : MonoBehaviour, INotificationReceiver
{
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is SignalEmitter signalEmitter)
        {
            Debug.Log("Signal received: " + signalEmitter.asset.name);
            // Call your event or method here
            MyCustomEvent();
        }
    }

    private void MyCustomEvent()
    {
        Debug.Log("My custom event has been triggered!");
        // Implement your event logic here
    }
}

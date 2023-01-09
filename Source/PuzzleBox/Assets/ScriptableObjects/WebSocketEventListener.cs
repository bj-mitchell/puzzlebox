using Assets.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

public class WebSocketEventListener : MonoBehaviour
{
    public WebSocketEvent Event;
    public WebSocketEventMessage Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(WebSocketMessage message)
    {
        Response.Invoke(message);
    }
}

using Assets.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class WebSocketEvent : ScriptableObject
{
    private List<WebSocketEventListener> listeners = new List<WebSocketEventListener>();

    public void Raise(WebSocketMessage message)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(message);
        }
    }

    public void RegisterListener(WebSocketEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(WebSocketEventListener listener)
    {
        listeners.Remove(listener);
    }
}

[System.Serializable]
public class WebSocketEventMessage : UnityEvent<WebSocketMessage> { }

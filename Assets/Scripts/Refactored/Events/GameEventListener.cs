using System;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Serializable]
    public struct EventWithResponse
    {
        public GameEvent gameEvent;
        public UnityEvent<object> response;
    }
    [SerializeField]
    public EventWithResponse[] events;

    public void OnEnable()
    {
        for (int i = 0; i < events.Length; i++)
        {
            events[i].gameEvent.RegisterListener(this);
        }
    }
    public void OnDisable()
    {
        for (int i = 0; i < events.Length; i++)
        {
            events[i].gameEvent.UnRegisterListener(this);
        }
    }
    public void OnEventRaised(GameEvent gameEvent, object obj)
    {
        for (int i = 0; i < events.Length; i++)
        {
            if (events[i].gameEvent == gameEvent)
            {
                events[i].response.Invoke(obj);
            }
        }
    }
}

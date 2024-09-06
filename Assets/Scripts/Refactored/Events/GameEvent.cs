using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private readonly HashSet<GameEventListener> listeners = new();
    [SerializeField] private GameEvent[] eventsToRaiseBefore;
    [SerializeField] private GameEvent[] eventsToRaiseAfter;

    [SerializeField] private string typeOfObjectParameter;
    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void RaiseEvent(object obj = null)
    {
        foreach (var item in eventsToRaiseBefore)
        {
            item.RaiseEvent(obj);
        }
        foreach (var item in listeners)
        {
            item.OnEventRaised(this, obj);
        }
        foreach (var item in eventsToRaiseAfter)
        {
            item.RaiseEvent(obj);
        }
    }

    public List<GameEventListener> GetListenersList()
    {
        return listeners.ToList();
    }
}

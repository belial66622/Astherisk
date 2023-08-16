using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAggregate<TEvent> : Singleton<EventAggregate<TEvent>>
{
    public Dictionary<Type, Action<TEvent>> _events = new Dictionary<Type, Action<TEvent>>();

    public void StartListening(Action<TEvent> action)
    {
        var messageType = typeof(TEvent);
        bool isEventExist = _events.ContainsKey(messageType);
        if (!isEventExist)
        {
            _events.Add(messageType, action);
            Debug.Log($"Subscribe, Event: {_events.Count}");
        }
        else
        {
            _events[messageType] += action;
        }
    }

    public void StopListening(Action<TEvent> action)
    {
        var messageType = typeof(TEvent);
        bool isEventExist = _events.ContainsKey(messageType);
        if (isEventExist)
        {
            _events[messageType] -= action;
            Debug.Log($"Subscribe, Event: {_events.Count}");
        }
    }

    public void TriggerEvent(TEvent eventType)
    {
        var messageType = typeof(TEvent);
        bool isEventExist = _events.ContainsKey(messageType);
        if (isEventExist)
        {
            _events[messageType]?.Invoke(eventType);
        }
    }
}

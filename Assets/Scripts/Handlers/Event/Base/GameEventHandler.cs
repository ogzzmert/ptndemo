using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventHandler : Handler
{
    [field: SerializeField] private GameEvent[] eventList;
    Dictionary<GameEventType, UnityEvent> events = new Dictionary<GameEventType, UnityEvent>();

    protected override void initialize()
    {
        foreach(GameEvent e in eventList) events.Add(e.type, e.action);
    }
    protected override HandlerType GetHandlerType()
    {
        return HandlerType.Event;
    }
    public void call(GameEventType eventType)
    {
        if (events.ContainsKey(eventType)) events[eventType].Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventHandler : Handler
{
    Dictionary<GameEventType, UnityEvent> events = new Dictionary<GameEventType, UnityEvent>();

    protected override void initialize()
    {
        foreach(GameEvent e in GetComponentsInChildren<GameEvent>()) events.Add(e.type, e.action);
    }
    public void call(GameEventType eventType)
    {
        if (events.ContainsKey(eventType)) events[eventType].Invoke();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : MonoBehaviour
{
    [field : SerializeField] public GameEventType type { get; private set; }
    [field : SerializeField] public UnityEvent action { get; private set; }
    
}

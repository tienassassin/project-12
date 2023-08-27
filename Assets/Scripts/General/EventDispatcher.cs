/*
 * Copyright (c) 2023 vergil2k2.
 * All rights reserved.
 * Email: liw140402@gmail.com
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : Singleton<EventDispatcher>
{
    private readonly Dictionary<EventID, Action<object>> _eventDict = new();

    /// <summary>
    ///     Add a listener to EventDispatcher
    /// </summary>
    /// <param name="eventID">EventID that object want to register</param>
    /// <param name="listener">Callback will be invoked when post this EventID</param>
    public void AddListener(EventID eventID, Action<object> listener)
    {
        _eventDict.TryAdd(eventID, null);
        _eventDict[eventID] += listener;
    }

    /// <summary>
    ///     Add a listener to EventDispatcher
    /// </summary>
    /// <param name="eventID">EventID that object want to unregister</param>
    /// <param name="listener">Callback will not be invoked anymore when post this EventID</param>
    public void RemoveListener(EventID eventID, Action<object> listener)
    {
        if (!_eventDict.ContainsKey(eventID))
        {
            EditorLog.Message($"Event {eventID} has 0 listeners");
            return;
        }

        _eventDict[eventID] -= listener;
        if (_eventDict[eventID] != null) return;

        EditorLog.Message($"Event {eventID} will be removed because all its listeners have been removed");
        _eventDict.Remove(eventID);
    }

    /// <summary>
    ///     Post the event, notify all its listeners
    /// </summary>
    /// <param name="eventID">EventID that will be posted</param>
    /// <param name="payload">Attached data, can be anything (class, struct...)</param>
    public void PostEvent(EventID eventID, object payload = null)
    {
        if (!_eventDict.ContainsKey(eventID))
        {
            EditorLog.Message($"Event {eventID} has 0 listeners");
            return;
        }

        _eventDict[eventID]?.Invoke(payload);
    }

    /// <summary>
    ///     Remove all events and their listeners
    /// </summary>
    public void RemoveAllListener()
    {
        _eventDict.Clear();
    }
}

public enum EventID
{
    ON_HIGHLIGHT_AURA,
    ON_LINEUP_CHANGED,
    ON_BATTLE_SCENE_LOADED,

    ON_TAKE_TURN,
    ON_TARGET_FOCUSED,
    ON_ACTION_QUEUE_CHANGED
}

public static class EventDispatcherExtensions
{
    public static void AddListener(this MonoBehaviour obj, EventID eventID, Action<object> listener)
    {
        EventDispatcher.Instance.AddListener(eventID, listener);
    }

    public static void RemoveListener(this MonoBehaviour obj, EventID eventID, Action<object> listener)
    {
        EventDispatcher.Instance.RemoveListener(eventID, listener);
    }

    public static void PostEvent(this MonoBehaviour obj, EventID eventID, object payload = null)
    {
        EventDispatcher.Instance.PostEvent(eventID, payload);
    }
}
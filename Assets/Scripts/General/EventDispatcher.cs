/*
 * Copyright (c) 2023 Duztine Lee.
 * All rights reserved.
 * Email: liw140402@gmail.com
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : Singleton<EventDispatcher>
{
    private Dictionary<EventID, Action<object>> eventDict = new();

    /// <summary>
    /// Add a listener to EventDispatcher
    /// </summary>
    /// <param name="eventID">EventID that object want to register</param>
    /// <param name="listener">Callback will be invoked when post this EventID</param>
    public void AddListener(EventID eventID, Action<object> listener)
    {
        eventDict.TryAdd(eventID, null);
        eventDict[eventID] += listener;
    }

    /// <summary>
    /// Add a listener to EventDispatcher
    /// </summary>
    /// <param name="eventID">EventID that object want to unregister</param>
    /// <param name="listener">Callback will not be invoked anymore when post this EventID</param>
    public void RemoveListener(EventID eventID, Action<object> listener)
    {
        if (!eventDict.ContainsKey(eventID))
        {
            EditorLog.Error($"Event {eventID} has 0 listeners");
            return;
        }
        
        eventDict[eventID] -= listener;
        if (eventDict[eventID] != null) return;
        
        EditorLog.Message($"Event {eventID} will be removed because all its listeners have been removed");
        eventDict.Remove(eventID);
    }

    /// <summary>
    /// Post the event, notify all its listeners
    /// </summary>
    /// <param name="eventID">EventID that will be posted</param>
    /// <param name="payload">Attached data, can be anything (class, struct...)</param>
    public void PostEvent(EventID eventID, object payload = null)
    {
        if (!eventDict.ContainsKey(eventID))
        {
            EditorLog.Error($"Event {eventID} has 0 listeners");
            return;
        }

        eventDict[eventID]?.Invoke(payload);
    }

    /// <summary>
    /// Remove all events and their listeners
    /// </summary>
    public void RemoveAllListener()
    {
        eventDict.Clear();
    }
}

public enum EventID
{
    ON_HIGHLIGHT_AURA,
    ON_LINEUP_CHANGED,
}

public static class EventDispatcherExtension
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
using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventDispatcher
{
    private static Dictionary<EventID, Action<object>> _eventDict = new();

    /// <summary>
    ///     Add a listener to EventDispatcher
    /// </summary>
    /// <param name="eventID">EventID that object want to register</param>
    /// <param name="listener">Callback will be invoked when post this EventID</param>
    public static void AddListener(EventID eventID, Action<object> listener)
    {
        _eventDict.TryAdd(eventID, null);
        _eventDict[eventID] += listener;
    }

    /// <summary>
    ///     Add a listener to EventDispatcher
    /// </summary>
    /// <param name="eventID">EventID that object want to unregister</param>
    /// <param name="listener">Callback will not be invoked anymore when post this EventID</param>
    public static void RemoveListener(EventID eventID, Action<object> listener)
    {
        if (!_eventDict.ContainsKey(eventID))
        {
            DebugLog.Message($"Event {eventID} has 0 listeners");
            return;
        }

        _eventDict[eventID] -= listener;
        if (_eventDict[eventID] != null) return;

        _eventDict.Remove(eventID);
    }

    /// <summary>
    ///     Post the event, notify all its listeners
    /// </summary>
    /// <param name="eventID">EventID that will be posted</param>
    /// <param name="payload">Attached data, can be anything (class, struct...)</param>
    public static void PostEvent(EventID eventID, object payload = null)
    {
        if (!_eventDict.ContainsKey(eventID))
        {
            DebugLog.Message($"Event {eventID} has 0 listeners");
            return;
        }

        _eventDict[eventID]?.Invoke(payload);
    }

    /// <summary>
    ///     Remove all events and their listeners
    /// </summary>
    public static void RemoveAllListener()
    {
        _eventDict.Clear();
    }
}

public enum EventID
{
    ON_UPDATE_CURRENCIES,
    
    ON_AURA_HIGHLIGHTED,
    ON_LINEUP_CHANGED,
    ON_BATTLE_SCENE_LOADED,

    ON_HEROES_SPAWNED,
    ON_TURN_TAKEN,
    ON_ENERGY_UPDATED,

    ON_TARGET_FOCUSED,
    ON_ACTION_QUEUE_CHANGED,
    ON_CURRENT_ENTITY_UPDATED,

    ON_FIRE_SPIRIT_UPDATED,
    ON_FIRE_SPIRIT_PREVIEWED
}

public static class EventDispatcherExtensions
{
    public static void AddListener(this MonoBehaviour obj, EventID eventID, Action<object> listener)
    {
        EventDispatcher.AddListener(eventID, listener);
    }

    public static void RemoveListener(this MonoBehaviour obj, EventID eventID, Action<object> listener)
    {
        EventDispatcher.RemoveListener(eventID, listener);
    }

    public static void PostEvent(this MonoBehaviour obj, EventID eventID, object payload = null)
    {
        EventDispatcher.PostEvent(eventID, payload);
    }
}
using System;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;

public static class EventManager
{

    private static Dictionary<Type, Action<IEvent>> events;

    public static void Publish<T>(T data) where T : IEvent
    {
        if (events == null)
        {
            return;
        }
        if (!events.ContainsKey(typeof(T)))
        {
            return;
        }
        events[typeof(T)]?.Invoke(data);
    }

    public static void Subscribe<T>(Action<T> action) where T : IEvent
    {
        if (events == null)
        {
            events = new Dictionary<Type, Action<IEvent>>();
        }
        if (!events.ContainsKey(typeof(T)))
        {
            events.Add(typeof(T), null);
        }
        events[typeof(T)] = events[typeof(T)] + (Action<IEvent>)(e => action((T)e));
    }

    public static void Unsubscribe<T>(Action<T> action) where T : IEvent
    {
        if (events == null)
        {
            return;
        }
        if (!events.ContainsKey(typeof(T)))
        {
            return;
        }
        events[typeof(T)] = events[typeof(T)] - (Action<IEvent>)(e => action((T)e));
    }

}
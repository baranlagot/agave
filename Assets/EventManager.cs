using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public static class EventManager
{
    private static readonly Dictionary<Type, Delegate> events = new Dictionary<Type, Delegate>();

    public static void Subscribe<T>(Action<T> handler) where T : IEvent
    {
        if (events.TryGetValue(typeof(T), out Delegate existingHandlers))
        {
            events[typeof(T)] = Delegate.Combine(existingHandlers, handler);
        }
        else
        {
            events[typeof(T)] = handler;
        }
    }

    public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        if (events.TryGetValue(typeof(T), out Delegate existingHandlers))
        {
            events[typeof(T)] = Delegate.Remove(existingHandlers, handler);
        }
    }

    public static void Publish<T>(T eventData) where T : IEvent
    {
        if (events.TryGetValue(typeof(T), out Delegate handler))
        {
            if (handler is Action<T> action)
            {
                action(eventData);
            }
        }
    }
}
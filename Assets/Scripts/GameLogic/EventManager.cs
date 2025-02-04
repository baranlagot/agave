using System;
using System.Collections.Generic;

/// <summary>
/// Manages events in the game.
/// </summary>
public static class EventManager
{
    private static readonly Dictionary<Type, Delegate> events = new Dictionary<Type, Delegate>();

    /// <summary>
    /// Subscribes to an event.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    /// <param name="handler">The handler to subscribe to the event.</param>
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

    /// <summary>
    /// Unsubscribes from an event.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    /// <param name="handler">The handler to unsubscribe from the event.</param>

    public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        if (events.TryGetValue(typeof(T), out Delegate existingHandlers))
        {
            events[typeof(T)] = Delegate.Remove(existingHandlers, handler);
        }
    }

    /// <summary>
    /// Publishes an event.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    /// <param name="eventData">The data to publish.</param>
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
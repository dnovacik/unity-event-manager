using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cognision.Tools
{
    [ExecuteAlways]
    public static class EventManager
    {
        private static Dictionary<Type, List<(IEventListenerBase listener, Delegate callback)>> _subscribers
            = new Dictionary<Type, List<(IEventListenerBase, Delegate)>>();

        public delegate void EventCallback<T>(T eventType);

        public static void AddListener<EventType>(IEventListener<EventType> caller, EventCallback<EventType> listener)
        {
            if (_subscribers.TryGetValue(typeof(EventType), out var listeners))
            {
                if (!listeners.Contains((caller, listener)))
                {
                    listeners.Add((caller, listener));
                }
            }
            else
            {
                _subscribers.Add(typeof(EventType), new List<(IEventListenerBase, Delegate)> { (caller, listener) });
            }
        }

        public static void RemoveListener<EventType>(IEventListener<EventType> caller, EventCallback<EventType> listener)
        {
            if (_subscribers.TryGetValue(typeof(EventType), out var listeners))
            {
                listeners.Remove((caller, listener));
            }
        }

        public static void TriggerEvent<EventType>(EventType eventType)
        {
            if (_subscribers.TryGetValue(typeof(EventType), out var listeners))
            {
                listeners.ForEach(l =>
                {
                    if (l.listener != null)
                    {
                        l.callback.DynamicInvoke(eventType);
                    }
                });
            }
        }
    }

    public static class EventRegister
    {
        public static void StartListening<EventType>(this IEventListener<EventType> caller, EventManager.EventCallback<EventType> listener) where EventType : struct
        {
            EventManager.AddListener(caller, listener);
        }

        public static void StopListening<EventType>(this IEventListener<EventType> caller, EventManager.EventCallback<EventType> listener) where EventType : struct
        {
            EventManager.RemoveListener(caller, listener);
        }
    }

    public interface IEventListenerBase { }
    public interface IEventListener<T> : IEventListenerBase
    {
    }
}

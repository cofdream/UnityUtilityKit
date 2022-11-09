using System;
using System.Collections.Generic;

namespace Cofdream.ToolKitRuntime.Core
{
    public class EventDispatcher<T> : IEvent<T>
    {
        private readonly Dictionary<T, List<Delegate>> allEventHandlers = null;
        private readonly List<List<Delegate>> removeEventHandles = null;


        public EventDispatcher()
        {
            allEventHandlers = new Dictionary<T, List<Delegate>>(10);
            removeEventHandles = new List<List<Delegate>>();
        }

        public void ClearAllHandle()
        {
            foreach (var delegates in allEventHandlers.Values)
            {
                delegates.Clear();
            }
            removeEventHandles.Clear();
        }
        public void ClearNullHandle()
        {
            int length = removeEventHandles.Count;
            for (int i = 0; i < length; i++)
            {
                List<Delegate> delegates = removeEventHandles[i];
                int oldLength = delegates.Count;
                int newLength = 0;
                for (int j = 0; j < oldLength; j++)
                {
                    if (delegates[j] != null)
                    {
                        if (newLength != j)
                        {
                            delegates[newLength] = delegates[j];
                        }
                        newLength++;
                    }
                }
                delegates.RemoveRange(newLength, oldLength - newLength);
            }
        }

        public void Add(T type, EventHandler<T> eventHandler)
        {
            AddHandle(allEventHandlers, type, eventHandler);
        }
        public void Remove(T type, EventHandler<T> eventHandler)
        {
            RemoveHandle(allEventHandlers, removeEventHandles, type, eventHandler);
        }
        public void Invoke(T type)
        {
            Send(allEventHandlers, type);
        }


        private static void AddHandle(Dictionary<T, List<Delegate>> dic, T type, Delegate eventHandler)
        {
            if (dic.TryGetValue(type, out List<Delegate> delegates))
            {
                delegates.Add(eventHandler);
            }
            else
            {
                delegates = new List<Delegate>() { eventHandler };
                dic.Add(type, delegates);
            }
        }
        private static void RemoveHandle(Dictionary<T, List<Delegate>> dic, List<List<Delegate>> removeHandles, T type, Delegate eventHandler)
        {
            if (dic.TryGetValue(type, out List<Delegate> delegates))
            {
                if (delegates != null)
                {
                    int index = delegates.IndexOf(eventHandler);
                    if (index != -1)
                    {
                        delegates[index] = null;
                    }

                    if (removeHandles.Contains(delegates) == false)
                    {
                        removeHandles.Add(delegates);
                    }
                }
            }
        }
        private static void Send(Dictionary<T, List<Delegate>> dic, T key)
        {
            if (dic.TryGetValue(key, out List<Delegate> delegates))
            {
                Send(delegates, key);
            }
        }
        private static void Send(List<Delegate> delegates, T arg)
        {
            int length = delegates.Count;
            for (int i = 0; i < length; i++)
            {
                if (delegates[i] is EventHandler<T> handler)
                {
                    handler.Invoke(arg);
                }
            }
        }
    }
}
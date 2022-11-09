using System;
using System.Collections.Generic;

namespace Cofdream.ToolKitRuntime.Core
{
    public class EventArgDispatcher<T> : IEventArg<T>
    {
        private readonly Dictionary<T, List<Delegate>> allEventHandlers = null;
        private readonly List<List<Delegate>> removeEventHandles = null;


        public EventArgDispatcher()
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
        public void Invoke(T key)
        {
            Send(allEventHandlers, key);
        }

        public void Add<Arg>(T key, EventHandler<T, Arg> eventHandler)
        {
            AddHandle(allEventHandlers, key, eventHandler);
        }
        public void Remove<Arg>(T key, EventHandler<T, Arg> eventHandler)
        {
            RemoveHandle(allEventHandlers, removeEventHandles, key, eventHandler);
        }
        public void Invoke<Arg>(T key, Arg arg)
        {
            Send(allEventHandlers, key, arg);
        }


        private static void AddHandle(Dictionary<T, List<Delegate>> dic, T type, Delegate eventHandler)
        {
            List<Delegate> delegates = null;
            if (dic.TryGetValue(type, out delegates))
            {
                delegates.Add(eventHandler);
            }
            else
            {
                dic.Add(type, new List<Delegate>() { eventHandler });
            }
        }
        private static void RemoveHandle(Dictionary<T, List<Delegate>> dic, List<List<Delegate>> removeHandles, T type, Delegate eventHandler)
        {
            List<Delegate> delegates = null;
            if (dic.TryGetValue(type, out delegates))
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

        private static void Send(Dictionary<T, List<Delegate>> dic, T type)
        {
            List<Delegate> delegates = null;
            if (dic.TryGetValue(type, out delegates))
            {
                Send(delegates, type);
            }
        }
        private static void Send(List<Delegate> delegates, T type)
        {
            int length = delegates.Count;
            for (int i = 0; i < length; i++)
            {
                var handler = delegates[i] as EventHandler<T>;
                if (handler != null)
                {
                    handler.Invoke(type);
                }
            }
        }

        private static void Send<Arg>(Dictionary<T, List<Delegate>> dic, T type, Arg arg)
        {
            List<Delegate> delegates = null;
            if (dic.TryGetValue(type, out delegates))
            {
                Send(delegates, type, arg);
            }
        }
        private static void Send<Arg>(List<Delegate> delegates, T type, Arg arg)
        {
            int length = delegates.Count;
            for (int i = 0; i < length; i++)
            {
                var handler = delegates[i] as EventHandler<T, Arg>;
                if (handler != null)
                {
                    handler.Invoke(type, arg);
                }
            }
        }
    }
}

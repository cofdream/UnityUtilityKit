
namespace Cofdream.ToolKitRuntime.Core
{
    public delegate void EventHandler<T>(T key);
    public delegate void EventHandler<T, Arg>(T key, Arg arg);

    public interface IEvent<T>
    {
        void Add(T id, EventHandler<T> action);
        void Remove(T id, EventHandler<T> action);
    }
}
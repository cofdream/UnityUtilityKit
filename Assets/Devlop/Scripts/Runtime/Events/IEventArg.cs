
namespace Cofdream.ToolKitRuntime.Core
{
    public interface IEventArg<T>:IEvent<T>
    {
        void Add<Arg>(T id, EventHandler<T, Arg> action);
        void Remove<Arg>(T id, EventHandler<T, Arg> action);
    }
}
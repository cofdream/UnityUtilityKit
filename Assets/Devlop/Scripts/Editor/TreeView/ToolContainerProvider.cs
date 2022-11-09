using System;

namespace Cofdream.ToolKitEditor
{
    public class ToolContainerProvider : ToolProvider
    {
        public Action ActionHandler;
        public Action DeactivateHandler;
        public Action DrawHandler;
        public Action DestoryHandler;

        public ToolContainerProvider(string path) : base(path) { }

        public override void OnActive()
        {
            ActionHandler?.Invoke();
        }

        public override void OnDeactivate()
        {
            DeactivateHandler?.Invoke();
        }

        public override void OnDraw()
        {
            DrawHandler?.Invoke();
        }
        public override void OnDestroy()
        {
            DestoryHandler?.Invoke();
        }
    }
}
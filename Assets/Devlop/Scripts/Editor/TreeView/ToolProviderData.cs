using System.Collections.Generic;

namespace Cofdream.ToolKitEditor
{
    [System.Serializable]
    public class ToolProviderData
    {
        //key: id  vale: Providers index
        public Dictionary<int, int> ID2IndexDic;

        public ToolProvider[] Providers;

        private ToolProvider currentProvider;
        public ToolProvider CurrentProvider => currentProvider;

        public void ChangeProvider(int id)
        {
            if (ID2IndexDic != null && ID2IndexDic.TryGetValue(id, out int index))
            {
                CurrentProvider?.OnDeactivate();
                currentProvider = Providers[index];
                currentProvider.OnActive();
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    [System.Serializable]
    public class ToolTreeView : TreeView
    {
        private ToolProviderData providerData;

        public ToolTreeView(TreeViewState state, ToolProviderData providerData) : base(state)
        {
            this.providerData = providerData;
            Reload();
        }
        protected override bool CanMultiSelect(TreeViewItem item) => false;

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem(-1, -1, "Root");

            if (providerData.Providers != null && providerData.Providers.Length > 0)
            {
                AddItemsToRoot(root);
            }
            else
            {
                root.AddChild(new TreeViewItem(-2, 0, "Empty"));
            }
            return root;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            if (selectedIds.Count > 0)
            {
                providerData.ChangeProvider(selectedIds.First());
            }
        }


        // 存储思路
        public void AddItemsToRoot(TreeViewItem root)
        {
            int toolsLength = providerData.Providers.Length;
            var id2IndexDic = providerData.ID2IndexDic = new Dictionary<int, int>(toolsLength);

            int id = root.id;
            for (int index = 0; index < toolsLength; index++)
            {
                var toolPath = providerData.Providers[index].ToolPath;

                // 获取直系父节点
                var toolPaths = toolPath.Split("/");//解析路径
                int parentCount = toolPaths.Length - 1;
                var lastParentNode = root;
                //寻找最后一个未被创建的
                int j = 0;
                for (; j < parentCount; j++)
                {
                    if (lastParentNode.children == null)
                    {
                        break;
                    }
                    else
                    {
                        string path = toolPaths[j];
                        foreach (var item in lastParentNode.children)
                        {
                            if (item.displayName == path)
                            {
                                lastParentNode = item;
                                break;
                            }
                        }
                    }
                }

                //补足空的父节点
                for (; j < parentCount; j++)
                {
                    var item = new TreeViewItem(++id, lastParentNode.depth + 1, toolPaths[j]);
                    lastParentNode.AddChild(item);
                    lastParentNode = item;
                }

                // 最少有一个可创建
                if (j > parentCount)
                {
                    //某个工具的节点可能是某个节点的父节点，被先创建了
                    int lastParentNodeId = lastParentNode.id;
                    if (id2IndexDic.ContainsKey(lastParentNodeId))
                    {
                        Debug.LogError($"{toolPath} 工具路径已存在！");
                    }
                    else
                    {
                        id2IndexDic.Add(lastParentNodeId, index);
                    }
                }
                else
                {
                    id2IndexDic.Add(++id, index);
                    lastParentNode.AddChild(new TreeViewItem(id, lastParentNode.depth + 1, toolPaths[j]));
                }
            }
        }
    }
}
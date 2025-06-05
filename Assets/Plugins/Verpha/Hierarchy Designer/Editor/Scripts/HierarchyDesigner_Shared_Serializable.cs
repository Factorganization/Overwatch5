#if UNITY_EDITOR
using System.Collections.Generic;

namespace Plugins.Verpha.Hierarchy_Designer.Editor.Scripts
{
    [System.Serializable]
    internal class HierarchyDesigner_Shared_Serializable<T>
    {
        public List<T> items;

        public HierarchyDesigner_Shared_Serializable(List<T> items)
        {
            this.items = items;
        }
    }
}
#endif
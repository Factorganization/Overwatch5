using GameContent.Actors.EnemySystems.EnemyNavigation;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.NavSpace
{
    [CustomEditor(typeof(NavSpaceSubData))]
    public class NavSpaceSubDataEditor : UnityEditor.Editor
    {
        #region methodes

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            visualTreeAsset.CloneTree(root);
            
            return root;
        }

        #endregion
        
        #region fields

        public VisualTreeAsset visualTreeAsset;

        #endregion
    }
}
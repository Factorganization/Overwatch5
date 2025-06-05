using GameContent.Actors.EnemySystems.EnemyNavigation;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.NavSpace
{
    [CustomEditor(typeof(NavSpaceObstacle))]
    public class ObstacleEditor : UnityEditor.Editor
    {
        #region methodes

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            visualTree.CloneTree(root);
            
            return root;
        }

        #endregion
        
        #region fields

        public VisualTreeAsset visualTree;

        #endregion
    }
}
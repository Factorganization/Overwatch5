using GameContent.Actors.EnemySystems.EnemyNavigation;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.NavSpace
{
    [CustomEditor(typeof(NavSpaceRunTimeManager))]
    public class NavSpaceRtmEditor : UnityEditor.Editor
    {
        #region methodes

        private void OnEnable()
        {
            _target = (NavSpaceRunTimeManager)target;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            visualTree.CloneTree(root);

            return root;
        }

        #endregion
        
        #region fields

        public VisualTreeAsset visualTree;

        private NavSpaceRunTimeManager _target;

        #endregion
    }
}
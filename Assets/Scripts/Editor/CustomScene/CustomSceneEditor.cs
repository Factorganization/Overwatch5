using RunTimeContent.CustomSceneManagement;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.CustomScene
{
    [CustomEditor(typeof(SceneListSo))]
    public class CustomSceneEditor : UnityEditor.Editor
    {
        #region methodes

        private void OnEnable()
        {
            _target = (SceneListSo)target;
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            visualTree.CloneTree(root);
            
            _loadGroupButton = root.Q<Button>("LoadGroupButton");
            _loadGroupButton.RegisterCallback<ClickEvent>(OnLoadGroupButton);
            
            return root;
        }

        private void OnLoadGroupButton(ClickEvent evt)
        {
            _target.LoadSceneGroup();
        }
        
        #endregion

        #region fields

        public VisualTreeAsset visualTree;

        private Button _loadGroupButton;

        private SceneListSo _target;
        
        #endregion
    }
}

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.IMGUI.Controls;
using UnityEditor;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [CustomEditor(typeof(NavSpaceBoundsDataHandling), true), CanEditMultipleObjects]
    public sealed class CustomBoundsDrawer : Editor
    {
        private readonly BoxBoundsHandle _mBoundsHandle = new();
        private SerializedProperty _mPosition;
        private SerializedProperty _mBounds;

        private void OnEnable()
        {
            _mPosition = serializedObject.FindProperty("position");
            _mBounds = serializedObject.FindProperty("bounds");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_mPosition);
            EditorGUILayout.PropertyField(_mBounds);

            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void OnSceneGUI()
        {
            var navBounds = (NavSpaceBoundsDataHandling)target;

            _mBoundsHandle.center = navBounds.Bounds.center;
            _mBoundsHandle.size = navBounds.Bounds.size;
            

            EditorGUI.BeginChangeCheck();

            _mBoundsHandle.DrawHandle();
            _mBoundsHandle.SetColor(Color.cyan);
            Handles.Label(navBounds.Position, "Bound Center");
            var pos = Handles.PositionHandle(navBounds.Position, Quaternion.identity);
            
            if (!EditorGUI.EndChangeCheck())
                return;
            
            Undo.RecordObject(navBounds, "Change Bounds");

            var newBounds = new Bounds
            {
                center = _mBoundsHandle.center,
                size = _mBoundsHandle.size
            };
            navBounds.Bounds = newBounds;
            navBounds.Position = pos;
        }
    }
}
#endif
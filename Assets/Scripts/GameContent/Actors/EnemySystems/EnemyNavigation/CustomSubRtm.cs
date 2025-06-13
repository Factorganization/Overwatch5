#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [CustomEditor(typeof(NavSpaceSubRunTimeArea), true), CanEditMultipleObjects]
    public sealed class CustomSubRtm : Editor
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
            var navBounds = (NavSpaceSubRunTimeArea)target;

            _mBoundsHandle.center = navBounds.Bounds.center;
            _mBoundsHandle.size = navBounds.Bounds.size;
            

            EditorGUI.BeginChangeCheck();

            _mBoundsHandle.DrawHandle();
            _mBoundsHandle.SetColor(Color.red);
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

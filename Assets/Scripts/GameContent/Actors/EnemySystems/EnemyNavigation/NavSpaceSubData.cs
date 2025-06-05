using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [CreateAssetMenu(fileName = "NavSpaceSubData", menuName = "NavSpace/NavSpaceSubData")]
    public class NavSpaceSubData : ScriptableObject
    {
        #region methodes

        public void AddNode(SerializedOctreeNode node)
        {
            nodes.Add(node);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public void AddEdge(SerializedOctreeEdge edge)
        {
            edges.Add(edge);
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        #endregion
        
        #region fields
        
        [HideInInspector] public List<SerializedOctreeNode> nodes = new();
        
        [HideInInspector] public List<SerializedOctreeEdge> edges = new();
        
        [HideInInspector] public int depth;
        
        #endregion
    }
}
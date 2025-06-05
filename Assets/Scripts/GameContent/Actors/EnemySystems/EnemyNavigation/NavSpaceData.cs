using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [CreateAssetMenu(fileName = "NavSpaceData", menuName = "NavSpace/NavSpaceData")]
    public class NavSpaceData : ScriptableObject
    {
        #region methodes

        public void AddSubData(NavSpaceSubData sub)
        {
            subData.Add(sub);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        
        public void AddNode(SerializedOctreeNode node, NavSpaceSubData sub)
        {
            sub.AddNode(node);
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif 
        }
        
        public void AddEdge(SerializedOctreeEdge edge, NavSpaceSubData sub)
        {
            sub.AddEdge(edge);
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        #endregion
        
        #region fields

        [HideInInspector] public List<NavSpaceSubData> subData;

        [HideInInspector] public float minBoundSize;

        [HideInInspector] public string subDataPath;

        #endregion
    }
}
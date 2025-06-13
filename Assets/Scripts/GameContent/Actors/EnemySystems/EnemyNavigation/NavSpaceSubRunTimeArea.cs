using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceSubRunTimeArea : MonoBehaviour
    {
        #region properties

        public List<RunTimePathNode> Nodes => _nodes;
        
        public Vector3 Position
        {
            set => position = value;
        }

        public Bounds Bounds
        {
            set => bounds = value;
        }

        #endregion

        #region methodes

        public void Init(List<RunTimePathNode> runTimePathNodes)
        {
            foreach (var n in runTimePathNodes)
            {
                if (!bounds.Contains(n.position))
                    continue;
                
                _nodes.Add(n);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0.5f, 0, 1);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0.5f, 0, 0.1f);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }

        #endregion
        
        #region fields
        
        [HideInInspector] [SerializeField] private Bounds bounds;
        
        [HideInInspector] [SerializeField] private Vector3 position;

        private readonly List<RunTimePathNode> _nodes = new();

        #endregion
    }
}
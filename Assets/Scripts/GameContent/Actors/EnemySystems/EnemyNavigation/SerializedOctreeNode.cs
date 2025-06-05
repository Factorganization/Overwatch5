using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [System.Serializable]
    public class SerializedOctreeNode
    {
        #region constructors
        
        public SerializedOctreeNode(Node node)
        {
            id = node.id;
            position = node.octreeNode.bounds.center;
            bounds = node.octreeNode.bounds;
            
            /*edges = new List<SerializedOctreeEdge>();
            foreach (var e in node.edges)
                edges.Add(new SerializedOctreeEdge(e));*/
            
            depth = node.depth;
        }
        
        #endregion

        #region methodes
        
        public static RunTimePathNode CreateRunTimePathNode(SerializedOctreeNode current)
        {
            return new RunTimePathNode(current.id, current.position, current.depth);
        }
        
        #endregion
        
        #region fields
        
        public int id;

        public Vector3 position;

        //public List<SerializedOctreeEdge> edges;

        public Bounds bounds;
        
        public int depth;

        #endregion
    }

    public class RunTimePathNode
    {
        #region constructors
        
        public RunTimePathNode(int id, Vector3 position, int depth)
        {
            this.id = id;
            this.position = position;
            edges = new List<RunTimePathEdge>();
            isAvailable = true;
            this.depth = depth;
        }
        
        #endregion
        
        #region fields

        public readonly int id;

        public bool isAvailable;
        
        public readonly List<RunTimePathEdge> edges;

        public readonly int depth;

        public Vector3 position;

        public RunTimePathNode from;

        public float g;

        public float h;

        public float f;
        
        #endregion
    }
}
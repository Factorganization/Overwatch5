using System.Collections.Generic;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [System.Serializable]
    public class SerializedOctreeEdge
    {
        #region constructors
        
        public SerializedOctreeEdge(Edge edge)
        {
            a = edge.a.id;
            b = edge.b.id;
            depth = edge.depth;
        }
        
        #endregion

        #region methodes

        public static RunTimePathEdge CreateRunTimePathEdge(SerializedOctreeEdge current, List<RunTimePathNode> nodes)
        {
            return new RunTimePathEdge(current.a, current.b, nodes, current.depth);
        }

        public static RunTimePathEdge CreateRunTimePathEdge(RunTimePathNode a, RunTimePathNode b)
        {
            return new RunTimePathEdge(a, b);
        }

        #endregion
        
        #region fields
        
        public int a;

        public int b;
        
        public int depth;
        
        #endregion
    }

    public class RunTimePathEdge
    {
        #region constructors

        public RunTimePathEdge(int a, int b, List<RunTimePathNode> nodes, int depth)
        {
            this.a = nodes[a];
            this.b = nodes[b];
            this.depth = depth;
        }

        public RunTimePathEdge(RunTimePathNode a, RunTimePathNode b)
        {
            this.a = a;
            this.b = b;
        }

        #endregion
        
        #region fields

        public readonly RunTimePathNode a;

        public readonly RunTimePathNode b;

        public readonly int depth;

        #endregion
    }
}
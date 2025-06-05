using System.Collections.Generic;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavGraph
    {
        #region constructors

        public NavGraph()
        {
            Node.ResetId();
        }

        #endregion
        
        #region methodes
        
        public void AddNode(OctreeNode node)
        {
            if (!nodes.ContainsKey(node))
            {
                nodes.Add(node, new Node(node, _currentDepth));
                _currentDepthContentCount++;
                if (_currentDepthContentCount >= DepthCountThreshold)
                {
                    _currentDepth++;
                    _currentDepthContentCount = 0;
                }
            }
        }

        public void AddEdge(OctreeNode a, OctreeNode b)
        {
            var nodeA = FindNode(a);
            var nodeB = FindNode(b);
            
            if (nodeA is null || nodeB is null)
                return;
            
            var edge = new Edge(nodeA, nodeB, _currentDepth);

            if (!edges.Add(edge))
                return;
            
            _currentDepthContentCount++;
            if (_currentDepthContentCount >= DepthCountThreshold)
            {
                _currentDepth++;
                _currentDepthContentCount = 0;
            }
            
            nodeA.edges.Add(edge);
            nodeB.edges.Add(edge);
        }
        
        private Node FindNode(OctreeNode octreeNode)
        {
            nodes.TryGetValue(octreeNode, out var node);
            return node;
        }
        
        #endregion
        
        #region fields
        
        public readonly Dictionary<OctreeNode, Node> nodes = new();
        
        public readonly HashSet<Edge> edges = new();

        private int _currentDepth;

        private int _currentDepthContentCount;
        
        private const int DepthCountThreshold = 500000;

        #endregion
    }

    public class Node
    {
        #region constructors

        public Node(OctreeNode ot, int depth)
        {
            id = nextId++;
            octreeNode = ot;
            this.depth = depth;
        }

        #endregion

        #region methodes

        public override bool Equals(object obj) => obj is Node other && id == other.id;
        
        public override int GetHashCode() => id.GetHashCode();
        
        public static void ResetId() => nextId = 0;

        #endregion
        
        #region fields

        private static int nextId;
        
        public readonly int id;
        
        public readonly List<Edge> edges = new();
        
        public readonly OctreeNode octreeNode;

        public Node from;

        public float f, g, h;

        public readonly int depth;

        #endregion
    }

    public class Edge
    {
        #region constructors

        public Edge(Node a, Node b, int depth)
        {
            this.a = a;
            this.b = b;
            this.depth = depth;
        }

        #endregion

        #region methodes

        public override bool Equals(object obj)
        {
            return obj is Edge other && ((a == other.a && b == other.b) || (a == other.b && b == other.a));
        }
        
        public override int GetHashCode() => a.GetHashCode() ^ b.GetHashCode();

        #endregion
        
        #region fields
        
        public readonly Node a;

        public readonly Node b;
        
        public readonly int depth;
        
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceRunTimeManager : MonoBehaviour
    {
        #region properties

        public static NavSpaceRunTimeManager Manager { get; private set; }
        
        public List<RunTimePathNode> RunTimePathNodes => _runTimePathNodes;
        
        public List<RunTimePathEdge> RunTimePathEdges => _runTimePathEdges;
        
        public List<NavSpaceObstacle> Obstacles => _obstacles;
        
        public bool NavSpaceLoaded => _navSpaceLoaded;

        #endregion
        
        #region methodes

        private void Awake()
        {
            Manager = this;
        }
        
        private async void Start()
        {
            _navSpaceLoaded = false;
            _runTimePathNodes = new List<RunTimePathNode>();
            _runTimePathEdges = new List<RunTimePathEdge>();
            _obstacles = new List<NavSpaceObstacle>();

            await Task.Run(GenRunTimePath);
            //await UniTask.WaitUntil(() => _navSpaceLoaded);
        }

        private void GenRunTimePath()
        {
            foreach (var sd in navSpaceData.subData)
            {
                foreach (var n in sd.nodes)
                {
                    _runTimePathNodes.Add(SerializedOctreeNode.CreateRunTimePathNode(n));
                }
            }

            _runTimePathNodes.Sort(CompareRunTimeNodes);
            
            foreach (var sd in navSpaceData.subData)
            {
                foreach (var e in sd.edges)
                {
                    var rte = SerializedOctreeEdge.CreateRunTimePathEdge(_runTimePathNodes[e.a], _runTimePathNodes[e.b]);
                    _runTimePathEdges.Add(rte);
                    _runTimePathNodes[e.a].edges.Add(rte);
                    _runTimePathNodes[e.b].edges.Add(rte);
                }
            }
            
            _navSpaceLoaded = true;
        }
        
        private void OnDrawGizmos()
        {
            if (viewOptions.view3dNavSpace)
            {
                foreach (var sd in navSpaceData.subData)
                {
                    foreach (var n in sd.nodes)
                    {
                        Gizmos.color = Color.Lerp(Color.blue, Color.green, navSpaceData.minBoundSize / n.bounds.size.x);
                        Gizmos.DrawWireCube(n.bounds.center, n.bounds.size);
                    }
                    
                }
            }
            
            switch (viewOptions.view3dNavPoints)
            {
                case true when RunTimePathNodes is null:
                {
                    Gizmos.color = new Color(1, 0.35f, 0, 0.25f);
                    foreach (var sd in navSpaceData.subData)
                    {
                        foreach (var n in sd.nodes)
                        {
                            Gizmos.DrawWireSphere(n.position, 0.3f);
                        }
                    }

                    break;
                }
                case true when RunTimePathNodes is not null:
                {
                    foreach (var n in RunTimePathNodes)
                    {
                        Gizmos.color = n.isAvailable ? new Color(0, 0.35f, 0, 0.25f) : new Color(1, 0.35f, 0, 0.25f);
                        Gizmos.DrawWireSphere(n.position, 0.3f);
                    }

                    break;
                }
            }
            
            switch (viewOptions.view3dNavPath)
            {
                case true when viewOptions.validateView3dNavPath && RunTimePathEdges is not null:
                {
                    Gizmos.color = new Color(1, 0, 0, 0.25f);
                    foreach (var e in RunTimePathEdges)
                    {
                        if (!e.a.isAvailable || !e.b.isAvailable)
                            continue;
                        
                        Gizmos.DrawLine(e.a.position, e.b.position);
                    }

                    break;
                }
            }
        }
        
        #endregion

        #region fields

        [SerializeField] private NavSpaceData navSpaceData;
        
        [SerializeField] private ViewOptions viewOptions;
        
        private List<RunTimePathNode> _runTimePathNodes;
        
        private List<RunTimePathEdge> _runTimePathEdges;
        
        private List<NavSpaceObstacle> _obstacles;
        
        private bool _navSpaceLoaded;
        
        private static readonly Comparison<RunTimePathNode> CompareRunTimeNodes =
            (a, b) => (int)Mathf.Sign(a.id - b.id);

        #endregion
    }

    [Serializable]
    public class ViewOptions
    {
        #region fields
        
        public bool view3dNavSpace;
        
        [Header("A vos risques et perils celui ci")]
        public bool view3dNavPath;
        
        [Header("Vraiment va falloir relancer Unity")]
        public bool validateView3dNavPath;
        
        [Space(10)]
        
        public bool view3dNavPoints;
        
        #endregion
    }
}
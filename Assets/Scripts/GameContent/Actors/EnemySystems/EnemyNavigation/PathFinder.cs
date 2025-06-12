using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public sealed class PathFinder
    {
        #region methodes
        
        public List<RunTimePathNode> FindPath(RunTimePathNode start, RunTimePathNode end)
        {
            var swRef = new Stopwatch();
            swRef.Start();
            
            _pathList.Clear();
            
            if (start is null || end is null)
            {
                Debug.LogWarning("No start or end node found");
                return new List<RunTimePathNode>();
            }

            _openList.Clear();
            _closedList.Clear();
            
            var iterationCount = 0;

            start.g = 0;
            start.h = Heuristic(start, end);
            start.f = start.g + start.h;
            start.from = null;
            _openList.Add(start);

            while (_openList.Count > 0)
            {
                
                if (++iterationCount > GameConstants.MaxPathFindIteration)
                {
                    Debug.LogWarning("Pathfind iteration exceeded");
                    return new List<RunTimePathNode>();
                }
                
                if (swRef.Elapsed.Seconds > 10)
                {
                    Debug.LogWarning("Cancelling Path Calculation");
                    return new List<RunTimePathNode>();
                }

                _openList.Sort(_nodeComparer);
                var current = _openList[0];
                _openList.Remove(current);

                if (current.id == end.id)
                {
                    return GetFullPath(current);
                }
                
                _closedList.Add(current);
                
                foreach (var e in current.edges)
                {
                    var n = e.a.id == current.id ? e.b : e.a;
                    
                    if (_closedList.Contains(n))
                        continue;
                    
                    var tempG = current.g + Heuristic(current, n);

                    if ((tempG >= n.g && _openList.Contains(n)) || !n.isAvailable)
                        continue;
                    
                    n.g = tempG;
                    n.h = Heuristic(n, end);
                    n.f = n.g + n.h;
                    n.from = current;
                    _openList.Add(n);
                }
            }
            
            //Debug.Log("No path found");
            return new List<RunTimePathNode>();
        }

        private static float Heuristic(RunTimePathNode a, RunTimePathNode b) => (a.position - b.position).sqrMagnitude;

        private List<RunTimePathNode> GetFullPath(RunTimePathNode current)
        {
            while (current is not null)
            {
                _pathList.Add(current);
                current = current.from;
            }
            
            _pathList.Reverse();
            return _pathList;
        }
        
        #endregion
        
        #region fields

        private readonly List<RunTimePathNode> _pathList = new();
        
        private readonly List<RunTimePathNode> _openList = new();
        
        private readonly List<RunTimePathNode> _closedList = new();
        
        private readonly Comparison<RunTimePathNode> _nodeComparer = (a, b) => (int)Mathf.Sign(a.f - b.f);
        
        #endregion
    }
}
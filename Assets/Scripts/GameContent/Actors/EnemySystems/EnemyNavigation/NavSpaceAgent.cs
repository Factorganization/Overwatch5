﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceAgent : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            _runTimeManager = FindFirstObjectByType<NavSpaceRunTimeManager>();
            if (_runTimeManager is null)
                Debug.LogError("No runtime manager found");
            
            _pathFinder = new PathFinder();
            Init(transform.position);
        }

        private async void Init(Vector3 pos)
        {
            try
            {
                _ct = _cancellationTokenSource.Token;
                _rTMProcessed = false;
                
                await UniTask.WaitUntil(() => _runTimeManager.NavSpaceLoaded, cancellationToken: _ct);
                _calculatingPath = false;
                _calculatingPath = false;
                _calculationTime = 0;
                _currentWayPointId = 0;
                _isStopped = false;
                _currentNode = GetClosestNode(pos);
                _rTMProcessed = true;

                //await UniTask.RunOnThreadPool(() => GetRandomDestination(pos, _ct), cancellationToken: _ct);
            }
            
            catch (Exception e)
            {
                _currentWayPointId = 0;
                _calculatingPath = false;
                _calculationTime = 0;
                throw new Exception(e.Message);
            }
        }
        
        private void Update()
        {
            if (_runTimeManager is null)
                return;
            
            if (_currentPath is null)
                return;
            
            if (!_rTMProcessed)
                return;
            
            if (_calculatingPath)
                _calculationTime += Time.deltaTime;
            
            if (_calculationTime > 10)
            {
                _cancellationTokenSource.Cancel();
                _calculationTime = 0;
                _calculatingPath = false;
                Debug.LogError("Agent discarded, Nav Space too heavy for calculation");
            }
            
            if (_isStopped)
                return;
            
            if (_currentPath.Count <= 0 || _currentWayPointId >= _currentPath.Count)
                return;
            
            if (Vector3.Distance(_currentPath[_currentWayPointId].position, transform.position) <= accuracy)
                _currentWayPointId++;

            if (_currentWayPointId >= _currentPath.Count)
                return;
            
            _currentNode = _currentPath[_currentWayPointId];
            _targetPosition = _currentNode.position;
            
            var dir = (_targetPosition - transform.position).normalized;
            transform.position += dir * (Time.fixedDeltaTime * speed);
        }

        public void SetTargetPosition(Vector3 target)
        {
            if (_calculatingPath)
                return;
            
            _calculatingPath = true;
            _calculationTime = 0;
            var pos = transform.position;
            
            HandlePathStarted(pos, target);
        }
        
        public void SetSpeed(float newSpeed) => speed = newSpeed;
        
        public void SetAccuracy(float newAccuracy) => accuracy = newAccuracy;
        
        public void SetStopped(bool isStopped) => _isStopped = isStopped;
        
        #region custom path Find

        private async void HandlePathStarted(Vector3 pos, Vector3 target)
        {
            try
            {
                var closestNode = await UniTask.RunOnThreadPool(() => GetClosestNode(pos), cancellationToken: _ct);
                var dest = await UniTask.RunOnThreadPool(() => GetClosestNode(target), cancellationToken: _ct);

                if (closestNode is null || dest is null)
                {
                    _calculatingPath = false;
                    _calculationTime = 0;
                    return;
                }
                
                _currentPath = await UniTask.RunOnThreadPool(() => _pathFinder.FindPath(closestNode, dest), cancellationToken: _ct);
                
                _currentWayPointId = 0;
                _calculatingPath = false;
            }
            
            catch (Exception e)
            {
                _currentWayPointId = 0;
                _calculatingPath = false;
                _calculationTime = 0;
                throw new Exception(e.Message);
            }
        }
        
        private RunTimePathNode GetClosestNode(Vector3 pos)
        {
            var d = float.MaxValue;
            RunTimePathNode closest = null;
            
            foreach (var rpn in _runTimeManager.RunTimePathNodes)
            {
                var td = Vector3.Distance(rpn.position, pos);

                if (td >= d)
                    continue;
                
                closest = rpn;
                d = td;
            }
            
            return closest;
        }

        private void OnDrawGizmos()
        {
            if (_runTimeManager is null || _currentPath is null)
                return;
            
            if (_currentPath.Count == 0)
                return;
            
            Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
            
            Gizmos.DrawWireSphere(_currentPath[0].position, 0.7f);
            Gizmos.DrawWireSphere(_currentPath[^1].position, 0.7f);

            for (var i = 0; i < _currentPath.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_currentPath[i].position, 0.4f);

                if (i >= _currentPath.Count - 1)
                    continue;
                
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(_currentPath[i].position, _currentPath[i + 1].position);
            }
        }
        
        #endregion

        #endregion
        
        #region fields

        [SerializeField] private float speed = 5f;

        [SerializeField] private float accuracy = 0.5f;

        private NavSpaceRunTimeManager _runTimeManager;
        
        private PathFinder _pathFinder;

        private bool _rTMProcessed;
        
        private List<RunTimePathNode> _currentPath;

        private Vector3 _targetPosition;

        private RunTimePathNode _currentNode;

        private int _currentWayPointId;

        private bool _isStopped;

        //private float _turnSpeed = 5f; //used if graph rotation

        private bool _calculatingPath;

        private float _calculationTime;

        private CancellationToken _ct;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        #endregion
    }
}
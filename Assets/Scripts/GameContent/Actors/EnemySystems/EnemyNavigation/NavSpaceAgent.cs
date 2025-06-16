using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceAgent : MonoBehaviour
    {
        #region properties

        public bool IsRoaming
        {
            get => _isRoaming;
            set => _isRoaming = value;
        }

        public NavSpaceSubRunTimeArea SubRunTimeArea => subRunTimeArea; 

        #endregion
        
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
            
            if (_calculationTime > 10.1f)
            {
                //_cancellationTokenSource.Cancel();
                _currentPath = new List<RunTimePathNode>();
                _calculationTime = 0;
                _calculatingPath = false;
                Debug.LogWarning("Agent discarded, Nav Space too heavy for calculation");
            }
            
            if (_isStopped)
                return;

            if (_currentPath.Count <= 0 || _currentWayPointId >= _currentPath.Count)
            {
                if (_isRoaming)
                    _isRoaming = false;
                return;
            }

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
            {
                return;
            }
            
            _calculatingPath = true;
            _calculationTime = 0;
            var pos = transform.position;
            
            HandlePathStarted(pos, target);
        }

        public void SetRandomTargetPosition()
        {
            if (_calculatingPath)
                return;
            
            _calculatingPath = true;
            _calculationTime = 0;

            var ri = Random.Range(0, subRunTimeArea is null ? _runTimeManager.Count : subRunTimeArea.Nodes.Count);
            var pos = transform.position;
            
            HandlePathStarted(pos, subRunTimeArea is null ? _runTimeManager.RunTimePathNodes[ri].position : subRunTimeArea.Nodes[ri].position);
        }
        
        public void SetSpeed(float newSpeed) => speed = newSpeed;
        
        public void SetAccuracy(float newAccuracy) => accuracy = newAccuracy;
        
        public void SetStopped(bool isStopped) => _isStopped = isStopped;
        
        #region custom path Find

        private async void HandlePathStarted(Vector3 pos, Vector3 target)
        {
            //try
            //{
                _isRoaming = true;
                var closestNode = await Task.Run(() => GetClosestNode(pos), cancellationToken: _ct);
                _startingNode = closestNode;
                var dest = await Task.Run(() => GetClosestNode(target), cancellationToken: _ct);
                _targetNode = dest;
                //var closestNode = GetClosestNode(pos);
                //var dest = GetClosestNode(target);
                
                if (closestNode is null || dest is null)
                {
                    _calculatingPath = false;
                    _calculationTime = 0;
                    return;
                }
                
                //await UniTask.WaitUntil(() => !PathFinder.Calculating, cancellationToken: _ct);
                _currentPath = await Task.Run(() => _pathFinder.FindPath(closestNode, dest), cancellationToken: _ct);
                //_currentPath = _pathFinder.FindPath(closestNode, dest);
                
                if (_currentPath is not null)
                    _isRoaming = true;
                
                _currentWayPointId = 0;
                _calculatingPath = false;
               
            //}
            
            /*catch (Exception e)
            {
                _currentWayPointId = 0;
                _calculatingPath = false;
                _calculationTime = 0;
                throw new Exception(e.Message);
            }*/
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
            
            Gizmos.color = Color.Lerp(Color.blue, Color.magenta, 0.5f);
            if (_startingNode is not null)
                Gizmos.DrawWireCube(_startingNode.position, Vector3.one * 0.35f);
            Gizmos.color = Color.Lerp(Color.green, Color.cyan, 0.5f);
            if (_targetNode is not null)
                Gizmos.DrawWireCube(_targetNode.position, Vector3.one * 0.35f);
            
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

        [SerializeField] private NavSpaceSubRunTimeArea subRunTimeArea;
        
        private NavSpaceRunTimeManager _runTimeManager;
        
        private PathFinder _pathFinder;

        private bool _rTMProcessed;
        
        private List<RunTimePathNode> _currentPath;

        private Vector3 _targetPosition;

        private RunTimePathNode _currentNode;

        private RunTimePathNode _targetNode;
        
        private RunTimePathNode _startingNode;
        
        private int _currentWayPointId;

        private bool _isStopped;

        //private float _turnSpeed = 5f; //used if graph rotation

        private bool _calculatingPath;

        private float _calculationTime;

        private bool _isRoaming;
        
        private CancellationToken _ct;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        #endregion
    }
}
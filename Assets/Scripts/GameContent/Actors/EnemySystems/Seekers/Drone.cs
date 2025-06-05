using DG.Tweening;
using GameContent.Actors.ActorData;
using UnityEngine;
using UnityEngine.AI;

namespace GameContent.Actors.EnemySystems.Seekers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Drone : Actor
    {
        #region methodes

        public override void Init(Transform player)
        {
            base.Init(player);
            
            IsActive = true;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _currentWaypoint = waypoints[0].position;
            _currentWaypointIndex = 0;
            transform.DOMove(_currentWaypoint, 5f);
        }
        
        public override void OnUpdate()
        {
            HandleWayDist();
        }

        public override void OnFixedUpdate()
        {
        }

        private void HandleWayDist()
        {
            if (Vector3.Distance(transform.position, _currentWaypoint) > 0.1f)
                return;

            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
            _currentWaypoint = waypoints[_currentWaypointIndex].position;
            
            transform.DOMove(_currentWaypoint, 5f);
        }
        
        #endregion

        #region fields

        [SerializeField] private DroneData droneData;
        
        [SerializeField] private Transform[] waypoints;

        private Vector3 _currentWaypoint;
        
        private int _currentWaypointIndex;
        
        private NavMeshAgent _navMeshAgent;
        
        private Vector3 _currentTargetPosition;
        
        #endregion
    }
}
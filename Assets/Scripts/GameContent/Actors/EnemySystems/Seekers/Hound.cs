using GameContent.Actors.ActorData;
using GameContent.Actors.EnemySystems.EnemyNavigation;
using GameContent.Management;
using UnityEngine;
using UnityEngine.AI;

namespace GameContent.Actors.EnemySystems.Seekers
{
    public class Hound : Actor
    {
        #region methodes

        public override void Init(Transform player)
        {
            IsActive = true;
            base.Init(player);
            GetComponent<NavMeshAgent>();
        }

        public override void OnUpdate()
        {
            _atkTimer += Time.deltaTime;
            
            if (!navSpaceAgent.IsRoaming)
            {
                if (_timerPos > 5f)
                {
                    _timerPos = 0;
                    navSpaceAgent.SetRandomTargetPosition();
                }
                else
                {
                    _timerPos += Time.deltaTime;
                }
            }
            
            if (Vector3.Distance(transform.position, playerTransform.position) < 10)
            {
                navSpaceAgent.IsRoaming = false;
                navSpaceAgent.SetTargetPosition(playerTransform.position);
                SuspicionManager.Manager.DetectionTime += 1;
            }
            else if (Vector3.Distance(transform.position, playerTransform.position) > 10)
            {
                SuspicionManager.Manager.ClosestHounds.Remove(this);
                SuspicionManager.Manager.DetectionTime -= 1;
            }
            
            
            /*if (Vector3.Distance(transform.position, playerTransform.position) < 2.5f)
            {
                if (_atkTimer > 2 && SuspicionManager.Manager.IsTracking)
                {
                    _atkTimer = 0;
                    SuspicionManager.Manager.PlayerHealth.TakeDamage(10);
                }
                _navMeshAgent.isStopped = true;
            }
            else
                _navMeshAgent.isStopped = false;

            if (Vector3.Distance(transform.position, playerTransform.position) > 10 && !SuspicionManager.Manager.IsTracking)
            {
                _navMeshAgent.destination = SuspicionManager.Manager.StartDebugPos;
            }*/
        }

        public override void OnFixedUpdate()
        {
        }

        public void SetTargetPosition(Vector3 pos)
        {
            //_navMeshAgent.destination = pos;
            navSpaceAgent.SetTargetPosition(pos);
        }

        #endregion

        #region fields

        [SerializeField] private HoundData houndData;

        [SerializeField] private NavSpaceAgent navSpaceAgent;

        private Vector3 _currentTargetPosition;

        private float _atkTimer;

        private float _timerPos;

        #endregion
    }
}

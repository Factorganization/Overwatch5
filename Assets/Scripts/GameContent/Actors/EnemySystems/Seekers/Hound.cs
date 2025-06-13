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
            //navSpaceAgent.SetRandomTargetPosition();
        }

        public override void OnUpdate()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            
            if (Vector3.Distance(transform.position, playerTransform.position) > 100f)
            {
                return;
            }
            
            _atkTimer += Time.deltaTime;
            
            if (!navSpaceAgent.IsRoaming && !SuspicionManager.Manager.IsTracking)
            {
                _timerPos += Time.deltaTime;
                if (_timerPos > 5f)
                {
                    navSpaceAgent.SetRandomTargetPosition();
                    _timerPos = 0;
                }
            }
            
            if (distanceToPlayer < 5f)
            {
                _closeEnough = true;
                SuspicionManager.Manager.DetectionTime += 1;
            }
            else if (distanceToPlayer > 10f && _closeEnough)
            {
                navSpaceAgent.SetTargetPosition(playerTransform.position);
                SuspicionManager.Manager.DetectionTime -= 1;
                _closeEnough = false;
            }
            
            if (distanceToPlayer < 12.5f && _closeEnough)
            {
                if (_atkTimer > 2 && SuspicionManager.Manager.IsTracking)
                {
                    _atkTimer = 0;
                    SuspicionManager.Manager.PlayerHealth.TakeDamage(10);
                }
            }
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

        private bool _closeEnough;

        #endregion
    }
}

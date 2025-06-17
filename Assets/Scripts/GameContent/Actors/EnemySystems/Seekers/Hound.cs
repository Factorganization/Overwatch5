using System;
using FMOD.Studio;
using GameContent.Actors.EnemySystems.EnemyNavigation;
using GameContent.Management;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.Seekers
{
    public class Hound : Actor
    {
        #region properties

        public bool HasPlayerInZone => navSpaceAgent.SubRunTimeArea is not null && 
                                       playerTransform is not null &&
                                       navSpaceAgent.SubRunTimeArea.Bounds.Contains(playerTransform.position);

        #endregion
        
        #region methodes

        private void Start()
        {
            _houndAmbientInstance = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.HoundAmbient);
            _houndAmbientInstance.start();
        }

        public override void Init(Transform player)
        {
            IsActive = true;
            base.Init(player);
            navSpaceAgent.SetSpeed(speed);
        }

        public override void OnUpdate()
        {
            if (playerTransform is null)
                return;

            if (navSpaceAgent.SubRunTimeArea is not null)
            {
                if (!navSpaceAgent.SubRunTimeArea.Bounds.Contains(playerTransform.position))
                    return;
            }
            
            var distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distanceToPlayer > SuspicionManager.Manager.Range)
                return;
            
            _atkTimer += Time.deltaTime;
            
            if (SuspicionManager.Manager.IsTracking)
            {
                if (navSpaceAgent.IsRoaming)
                    navSpaceAgent.IsRoaming = false;
                
                navSpaceAgent.SetSpeed(trackSpeed);
            }
            else
                navSpaceAgent.SetSpeed(speed);
            
            if (navSpaceAgent.IsRoaming == false && SuspicionManager.Manager.IsTracking == false)
            {
                _timerPos += Time.deltaTime;
                if (_timerPos > 5f)
                {
                    navSpaceAgent.SetRandomTargetPosition();
                    _timerPos = 0;
                }
            }
            
            var col = Physics.Raycast(transform.position, 
                (playerTransform.position - transform.position).normalized, 
                out var hit,
                detectionRange,
                collidableLayer);

            if (distanceToPlayer < detectionRange && col && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                SuspicionManager.Manager.DetectionTime += 1;
                _closeEnough = true;
            }
            
            if (distanceToPlayer > detectionRange && _closeEnough)
            {
                navSpaceAgent.SetTargetPosition(playerTransform.position);
                SuspicionManager.Manager.DetectionTime -= 1;
                _closeEnough = false;
            }
            
            if (distanceToPlayer < detectionRange && _closeEnough  && col && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (_atkTimer > 1 && SuspicionManager.Manager.IsTracking)
                {
                    _atkTimer = 0;
                    var dir = (playerTransform.position - transform.position).normalized;
                    laserEmiter.transform.rotation = Quaternion.LookRotation(dir);
                    laserEmiter.Emit(1);
                    AudioManager.Instance.PlayOneShot(FMODEvents.Instance.HoundAttack, transform.position);
                    SuspicionManager.Manager.PlayerHealth.TakeDamage(atkDamage);
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

        [SerializeField] private float speed;

        [SerializeField] private float trackSpeed;

        [SerializeField] private float atkRange;

        [SerializeField] private float atkDamage;

        [SerializeField] private float detectionRange;
        
        [SerializeField] private LayerMask collidableLayer;
        
        [SerializeField] private NavSpaceAgent navSpaceAgent;

        [SerializeField] private ParticleSystem laserEmiter;
        
        private Vector3 _currentTargetPosition;

        private float _atkTimer;

        private float _timerPos;

        private bool _closeEnough;
        
        private EventInstance _houndAmbientInstance;

        #endregion
    }
}

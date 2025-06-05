using GameContent.Management;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.SuspicionModifiers
{
    public class Door : Actor
    {
        #region methodes

        public override void OnUpdate()
        {
            var d = Vector3.Distance(playerTransform.position, doorCenter.position);
            
            if (d < detectionDistance && !_isInside)
            {
                _isInside = true;
                SuspicionManager.Manager?.AddSuspicion(suspicionAmount);
            }
            
            else if (d > detectionDistance && _isInside)
                _isInside = false;
        }

        public override void OnFixedUpdate()
        {
        }

        #endregion

        #region methodes

        [SerializeField] private float detectionDistance;
        
        [SerializeField] private Transform doorCenter;

        [SerializeField] private float suspicionAmount;
        
        private bool _isInside;

        #endregion
    }
}
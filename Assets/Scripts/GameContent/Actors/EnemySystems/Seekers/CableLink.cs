using UnityEngine;

namespace GameContent.Actors.EnemySystems.Seekers
{
    /// <summary>
    /// Pokemon mentioned
    /// </summary>
    public class CableLink : Actor
    {
        #region methodes
        
        public override void Init(Transform player)
        {
            playerTransform = player;
        }

        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnAction()
        {
            enemyCameraRef.IsActive = !enemyCameraRef.IsActive;
        }
        
        #endregion

        #region fields

        [SerializeField] private EnemyCamera enemyCameraRef;

        #endregion
    }
}
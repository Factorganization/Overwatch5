using UnityEngine;

namespace GameContent.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        #region properties
        
        public virtual bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        #endregion
        
        #region methodes

        public virtual void Init(Transform player)
        {
            playerTransform = player;
        }
        
        public abstract void OnUpdate();

        public abstract void OnFixedUpdate();
        
        public virtual void OnAction()
        {
        }
        
        #endregion

        #region fields

        protected Transform playerTransform;

        protected bool isActive;

        #endregion
    }
}
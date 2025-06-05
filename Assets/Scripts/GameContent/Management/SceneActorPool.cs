using GameContent.Actors;
using UnityEngine;

namespace GameContent.Management
{
    public class SceneActorPool : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            foreach (var a in managedActors)
                a.Init(playerTransform);

            foreach (var a in unmanagedActors)
                a.Init(playerTransform);
        }

        private void Update()
        {
            foreach (var a in managedActors)
            {
                if (!a.IsActive)
                    continue;
                
                a.OnUpdate();
            }

            foreach (var a in unmanagedActors)
            {
                a.OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (var a in managedActors)
            {
                if (!a.IsActive)
                    continue;

                a.OnFixedUpdate();
            }

            foreach (var a in unmanagedActors)
            {
                a.OnFixedUpdate();
            }
        }
        
        #endregion
        
        #region fields

        [SerializeField] private Transform playerTransform;
        
        [SerializeField] private Actor[] managedActors;

        [SerializeField] private Actor[] unmanagedActors;
        
        #endregion
    }
}
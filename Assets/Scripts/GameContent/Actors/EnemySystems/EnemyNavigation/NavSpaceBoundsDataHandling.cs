using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    [ExecuteAlways]
    public class NavSpaceBoundsDataHandling : MonoBehaviour
    {
        #region properties
        
        public Vector3 Position
        {
            get => position;
            set => position = value;
        }

        public Bounds Bounds
        {
            get
            {
                bounds.center = Position;
                return bounds;
            }

            set => bounds = value;
        }

        #endregion

        #region methodes

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0.35f, 1);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0.05f, 0.1f);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
        
        private void OnDestroy()
        {
            if (!Application.isPlaying && gameObject.scene.isLoaded)
                navSpaceGeneratorRef.Bounds.Remove(this);
        }

        #endregion
        
        #region fields

        [HideInInspector] [SerializeField] public NavSpaceGenerator navSpaceGeneratorRef;
        
        [HideInInspector] [SerializeField] private Bounds bounds;
        
        [HideInInspector] [SerializeField] private Vector3 position;

        #endregion
    }
}
using UnityEngine;

namespace RunTimeContent.SceneManagement
{
    public class SceneSelfLoader : MonoBehaviour
    {
        #region methodes
        
        public void Init(Transform player) => _playerTransform = player;
        
        private async void Update()
        {
            if (Vector3.Distance(_playerTransform.position, transform.position) < loadRadius && !_loaded)
            {
                _loaded = true;
                await SceneLoader.Loader.LoadSceneGroup(sceneIndex);
            }
            
            else if (Vector3.Distance(_playerTransform.position, transform.position) > unloadRadius && _loaded)
            {
                _loaded = false;
                await SceneLoader.Loader.UnloadSceneGroup(sceneIndex);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, loadRadius);
            
            Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
            Gizmos.DrawWireSphere(transform.position, unloadRadius);
        }
        
        #endregion

        #region fields
        
        [SerializeField] private int sceneIndex;
        
        [SerializeField] private float loadRadius;
        
        [SerializeField] private float unloadRadius;
        
        private Transform _playerTransform;
        
        private bool _loaded;

        #endregion
    }
}
using UnityEngine;

namespace RunTimeContent.SceneManagement
{
    public class SceneSelfLoader : MonoBehaviour
    {
        #region methodes
        
        public void Init(Transform player) => _playerTransform = player;
        
        private async void Update()
        {
            _delay += Time.deltaTime;
            
            if (_delay < 1f)
                return;

            _delay = 0;
            
            if ((Mathf.Abs(_playerTransform.position.x - transform.position.x) <= xLoadDistance && 
                 Mathf.Abs(_playerTransform.position.z - transform.position.z) <= zLoadDistance) 
                && !_loaded)
            {
                _loaded = true;
                await SceneLoader.Loader.LoadSceneGroup(sceneIndex);
            }
            
            else if ((Mathf.Abs(_playerTransform.position.x - transform.position.x) > xLoadDistance + unloadOffset && 
                      Mathf.Abs(_playerTransform.position.z - transform.position.z) > zLoadDistance + unloadOffset) 
                     && _loaded)
            {
                _loaded = false;
                await SceneLoader.Loader.UnloadSceneGroup(sceneIndex);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(xLoadDistance, 100, zLoadDistance));
            
            Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
            Gizmos.DrawWireCube(transform.position, new Vector3(xLoadDistance + unloadOffset, 100, zLoadDistance + unloadOffset));
        }
        
        #endregion

        #region fields
        
        [SerializeField] private int sceneIndex;
        
        [SerializeField] private float xLoadDistance;
        
        [SerializeField] private float zLoadDistance;
        
        [SerializeField] private float unloadOffset;
        
        private Transform _playerTransform;
        
        private bool _loaded;

        private float _delay;

        #endregion
    }
}
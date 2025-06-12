using UnityEngine;
using UnityEngine.ProBuilder;

namespace RunTimeContent.SceneManagement
{
    public class SceneSelfLoader : MonoBehaviour
    {
        #region properties

        public int Id => sceneIndex;
        
        public bool Loaded
        {
            get => _loaded;
            set => _loaded = value;
        }

        #endregion
        
        #region methodes

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !_loaded)
            {
                _loaded = true;
                SceneLoader.Loader.LoadSceneGroup(sceneIndex);
            }
        }
        
        #endregion

        #region fields
        
        [SerializeField] private int sceneIndex;
        
        private bool _loaded;

        #endregion
    }
}
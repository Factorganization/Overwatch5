using UnityEngine;

namespace RunTimeContent.SceneManagement
{
    public class SceneSelfLoaderManager : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            foreach (var ssl in sceneSelfLoaders)
            {
                ssl.Init(playerTransform);
            }
        }

        #endregion
        
        #region fields

        [SerializeField] private Transform playerTransform;
        
        [SerializeField] private SceneSelfLoader[] sceneSelfLoaders;

        #endregion
    }
}
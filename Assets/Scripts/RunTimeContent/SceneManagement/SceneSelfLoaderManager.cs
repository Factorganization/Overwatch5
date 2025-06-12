using UnityEngine;

namespace RunTimeContent.SceneManagement
{
    public class SceneSelfLoaderManager : MonoBehaviour
    {
        #region fields

        [SerializeField] private Transform playerTransform;
        
        [SerializeField] private SceneSelfLoader[] sceneSelfLoaders;

        #endregion
    }
}
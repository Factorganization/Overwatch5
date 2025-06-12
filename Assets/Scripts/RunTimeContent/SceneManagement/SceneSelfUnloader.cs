using UnityEngine;

namespace RunTimeContent.SceneManagement
{
    public class SceneSelfUnloader : MonoBehaviour
    {
        #region methodes

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && sceneSelfLoader.Loaded)
            {
                sceneSelfLoader.Loaded = false;
                SceneLoader.Loader.LoadSceneGroup(sceneSelfLoader.Id);
            }
        }

        #endregion

        #region fields

        [SerializeField] private SceneSelfLoader sceneSelfLoader;

        #endregion
    }
}
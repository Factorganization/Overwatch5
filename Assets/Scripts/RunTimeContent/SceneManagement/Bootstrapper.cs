using RunTimeContent.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunTimeContent.SceneManagement
{
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Init()
        {
            Debug.Log("Bootstrapper...");
            await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
        }
    }
}

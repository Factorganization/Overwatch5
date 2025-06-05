using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : PersistentSingleton<Bootstrapper>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private async void Init()
    {
        Debug.Log("Bootstrapper...");
        await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
    }
}

#if UNITY_EDITOR
using RunTimeContent.CustomSceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument mainMenu;
#if UNITY_EDITOR
    [SerializeField] private SceneListSo sceneList;
#endif
    
    private Button newGameButton, settingsButton, creditsButton, quitButton;

    private void Awake()
    {
        newGameButton = mainMenu.rootVisualElement.Q<Button>("NewGameButton");
        newGameButton.RegisterCallback<ClickEvent>(NewGame);
        
        settingsButton = mainMenu.rootVisualElement.Q<Button>("SettingsButton");
        settingsButton.RegisterCallback<ClickEvent>(OpenSettings);
        
        creditsButton = mainMenu.rootVisualElement.Q<Button>("CreditsButton");
        creditsButton.RegisterCallback<ClickEvent>(OpenCredits);
        
        quitButton = mainMenu.rootVisualElement.Q<Button>("QuitButton");
        quitButton.RegisterCallback<ClickEvent>(QuitGame);
    }

    private void OpenCredits(ClickEvent evt)
    {
        Debug.Log("Load Game");
    }

    public void NewGame(ClickEvent evt)
    {
        //SceneManager.LoadScene("Bootstrapper");
        //CustomSceneLoader.LoadSceneGroup("SecondPlayable", sceneList);
        SceneManager.LoadSceneAsync("SC_CollidersLA", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("SC_GA_01", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("SC_GA_VFX", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("SC_GD", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("MainMenu");
    }
    
    public void OpenSettings(ClickEvent evt)
    {
        Debug.Log("Open Settings");
    }
    
    public void QuitGame(ClickEvent evt)
    {
        Application.Quit();
    }
}

#if UNITY_EDITOR
using RunTimeContent.CustomSceneManagement;
#endif
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument mainMenu;
    [SerializeField] private GameObject settingsMenu, creditsMenu;
#if UNITY_EDITOR
    [SerializeField] private SceneListSo sceneList;
#endif
    
    private Button newGameButton, settingsButton, creditsButton, quitButton;
    
    private bool isMenuOpen;

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

    private void Start()
    {
        isMenuOpen = true;
        mainMenu.rootVisualElement.style.display = DisplayStyle.Flex;
        
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    private void OpenCredits(ClickEvent evt)
    {
        ToggleMainMenu();
        creditsMenu.SetActive(!creditsMenu.activeSelf);
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
    
    private void OpenSettings(ClickEvent evt)
    {
        ToggleMainMenu();
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }
    
    private void QuitGame(ClickEvent evt)
    {
        Application.Quit();
    }

    public void ToggleMainMenu()
    {
        isMenuOpen = !isMenuOpen;
        mainMenu.rootVisualElement.style.display = isMenuOpen ? DisplayStyle.Flex : DisplayStyle.None;
    }
    
    public void ReturnToMainMenu()
    {
        creditsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        ToggleMainMenu();
    }
}

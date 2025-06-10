using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    
    [Header("UI Panels")]
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _interactibleInfoUI;
    [SerializeField] private Settings _settingsMenu;
    [SerializeField] private HealthVisual _healthVisual;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _batteryText;
    [SerializeField] private TextMeshProUGUI _interactibleText;
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private Image _hackProgressImage;

    public GameObject PauseUI => _pauseMenu;
    public Image HackProgressImage => _hackProgressImage;
    public HealthVisual HealthVisual { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _gameUI.SetActive(true);
        _pauseMenu.SetActive(false);
    }
    
    public void UpdateText()
    {
        if (Hero.Instance.Health)
        {
            _healthText.text = $"Health: {Hero.Instance.Health.CurrentHealth}/{Hero.Instance.Health.MaxHealth}";
        }
        
        if (Hero.Instance.MultiToolObject)
        {
            _batteryText.text = $"Battery: {Hero.Instance.MultiToolObject.CurrentBattery}/{Hero.Instance.MultiToolObject.MaxBattery}";
        }
    }

    public void UpdateInteractibleUI(string name, bool onoff)
    {
        _interactibleText.text = name;
        _interactibleInfoUI.SetActive(onoff);
    }

    public void TogglePauseMenu()
    {
        bool isActive = _pauseMenu.activeSelf;
        _pauseMenu.SetActive(!isActive);
        
        Time.timeScale = isActive ? 1 : 0;
    }
    
    public void ShowNotification(string message, float duration = 2f)
    {
        _notificationText.text = message;
        _notificationText.gameObject.SetActive(true);
        Invoke(nameof(HideNotification), duration);
    }

    private void HideNotification()
    {
        _notificationText.gameObject.SetActive(false);
    }
    
    public void OpenPage(GameObject page)
    {
        page.SetActive(true);;
    }
    
    public void ClosePage(GameObject page)
    {
        page.SetActive(false);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

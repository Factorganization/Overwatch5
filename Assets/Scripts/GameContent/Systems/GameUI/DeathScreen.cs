using System.Collections;
using GameContent.Management;
using Systems;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Button _respawnButton, _returnButton;
    [SerializeField] private float _fadeDuration = 1f;

    public bool FallDeath;
    
    void Start()
    {
        _respawnButton.onClick.AddListener(() =>
        {
            GameManager.Instance.Respawn();
            Hide();
        });

        _backgroundImage.color = new Color(0, 0, 0, 0);
        _respawnButton.gameObject.SetActive(false);
        _returnButton.gameObject.SetActive(false);
    }
    
    public void Show()
    {
        _backgroundImage.gameObject.SetActive(true);
        StartCoroutine(FadeInDeathScreen());
    }
    
    private void Hide()
    {
        StopAllCoroutines();
        _backgroundImage.color = new Color(0, 0, 0, 0);
        _backgroundImage.gameObject.SetActive(false);
        _respawnButton.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Hero.Instance.Health.CurrentHealth = Hero.Instance.Health.MaxHealth;
        Time.timeScale = 1f;
    }

    IEnumerator FadeInDeathScreen()
    {
        float t = 0f;
        
        Time.timeScale = 0f;
        
        while (t < _fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            _backgroundImage.color = new Color(0, 0, 0, t / _fadeDuration);
            yield return null;
        }
        
        _backgroundImage.color = new Color(0, 0, 0, 1);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        _respawnButton.gameObject.SetActive(FallDeath);
        _returnButton.gameObject.SetActive(true);
    }
}

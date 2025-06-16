using UnityEngine;

public class HeroHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        GameUIManager.Instance.UpdateText();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log("mdg");
        if (_currentHealth <= 0)
        {
            Die();
        }
        GameUIManager.Instance.UpdateLifeText();
        GameUIManager.Instance.HealthVisual.HealthDamage();
    }
    
    public void Heal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        GameUIManager.Instance.UpdateText();
        GameUIManager.Instance.HealthVisual.HealthHeal();
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.MedkitUsed, transform.position);
    }

    private void Die()
    {
        GameUIManager.Instance.DeathScreen.DeathMessageText.text = "You have died!";
        //AudioManager.Instance.PlayOneShot(FMODEvents.Instance.AvatarDeath, transform.position);
        GameUIManager.Instance.DeathScreen.Show();
    }
}

using System;
using UnityEngine;

public class HeroHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        GameUIManager.Instance.UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
        GameUIManager.Instance.UpdateText();
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
    }

    private void Die()
    {
    }
}

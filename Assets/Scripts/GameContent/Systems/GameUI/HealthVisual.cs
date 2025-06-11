using Systems;
using UnityEngine;
using UnityEngine.UI;

public class HealthVisual : MonoBehaviour
{
    [SerializeField] private Image _healthBorder;

    public void HealthDamage()
    {
        Debug.Log("HealthVisual: HealthDamage called");
        float transparency = 1f - (Hero.Instance.Health.CurrentHealth / Hero.Instance.Health.MaxHealth);
        _healthBorder.color = new Color(_healthBorder.color.r, _healthBorder.color.g, _healthBorder.color.b, transparency);
    }
    
    public void HealthHeal()
    {
        float transparency = Hero.Instance.Health.CurrentHealth / Hero.Instance.Health.MaxHealth;
        _healthBorder.color = new Color(_healthBorder.color.r, _healthBorder.color.g, _healthBorder.color.b, transparency);
    }
}

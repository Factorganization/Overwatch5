using System.Collections;
using Systems;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthVisual : MonoBehaviour
{
    [SerializeField] private Volume _healthBorder;
    //[SerializeField] private Volume _healBorder;
    [SerializeField] private float _feedbackDuration = 0.2f;

    public void HealthDamage()
    {
        StartCoroutine(DamageFeedBack());
    }
    
    public void HealthHeal()
    {
        StartCoroutine(HealFeedBack());
    }

    IEnumerator DamageFeedBack()
    {
        _healthBorder.weight = 1f;
        yield return new WaitForSeconds(_feedbackDuration);
        _healthBorder.weight = 1f - Hero.Instance.Health.CurrentHealth / Hero.Instance.Health.MaxHealth;
    }
    
    IEnumerator HealFeedBack()
    {
        //_healBorder.weight = 1f;
        yield return new WaitForSeconds(_feedbackDuration);
        //_healBorder.weight = 0f;
        _healthBorder.weight = 1f - Hero.Instance.Health.CurrentHealth / Hero.Instance.Health.MaxHealth;
    }
}

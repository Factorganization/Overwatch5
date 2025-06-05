using Systems.Inventory;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    [SerializeField] ItemDetails itemDetails;
    [SerializeField] private float _maxBattery;
    [SerializeField] private float _currentBattery;

    public float CurrentBattery
    {
        get => _currentBattery;
        set => _currentBattery = value;
    }
    
    public float MaxBattery => _maxBattery;
    
    private void Start()
    {
        _maxBattery = 100f;
        _currentBattery = _maxBattery;
        GameUIManager.Instance.UpdateText();
    }
    
    public void ConsumeBattery(float amount)
    {
        _currentBattery -= amount;
        
        if (_currentBattery - amount < 0)
        {
            GameUIManager.Instance.UpdateText();
            return;
        }
        
        GameUIManager.Instance.UpdateText();
    }
    
    public void RechargeBattery(float amount)
    {
        _currentBattery += amount;
        
        if (_currentBattery >= _maxBattery)
        {
            _currentBattery = _maxBattery;
            GameUIManager.Instance.UpdateText();
            return;
        }
        
        GameUIManager.Instance.UpdateText();
    }
}

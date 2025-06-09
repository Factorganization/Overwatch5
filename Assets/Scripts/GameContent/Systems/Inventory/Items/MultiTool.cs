using GameContent.Actors;
using GameContent.Actors.EnemySystems.Seekers;
using Systems;
using Systems.Inventory;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    [SerializeField] ItemDetails itemDetails;
    [SerializeField] private float _maxBattery;
    [SerializeField] private float _currentBattery;
    
    [Header("Tool Settings")]
    [SerializeField] private float _toolRange = 10f;
    [SerializeField] private float _currentScanTimer;
    
    [Header("Device Information")]
    [SerializeField] private Actor _currentDevice;
    public bool isScanning;
    
    public Actor CurrentDevice
    {
        get => _currentDevice;
        set => _currentDevice = value;
    }
    
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

    public void ScanDevice()
    {
        if (Hero.Instance.CurrentEquipedItem.type == Type.MultiTool)
        {
            if (_currentDevice != null && _currentDevice is EnemyCamera enemyCamera)
            {
                if (enemyCamera != null && enemyCamera.IsActive)
                {
                    _currentScanTimer = 0f;
                    isScanning = true;
                        
                    GameUIManager.Instance.hackProgressImage.gameObject.SetActive(true);
                    GameUIManager.Instance.hackProgressImage.fillAmount = 0;
                }
            }
        }
    }

    public void Scanning()
    {
        if (!_currentDevice) return;
        
        _currentScanTimer += Time.deltaTime;

        if (_currentDevice is EnemyCamera enemyCamera)
        {
            if (enemyCamera.NetworkNode.hidden == false) return;
            
            float progress = _currentScanTimer / enemyCamera.ScanningTime;
            GameUIManager.Instance.hackProgressImage.fillAmount = Mathf.Clamp01(progress);
            
            if (_currentScanTimer >= enemyCamera.ScanningTime)
            {
                enemyCamera.NetworkNode.name.text = enemyCamera.NetworkNode.nodeId;
                enemyCamera.NetworkNode.hidden = false;
                CancelScan();
            }
        }
    }

    public void CancelScan()
    {
        isScanning = false;
        _currentScanTimer = 0f;
        _currentDevice = null;
        GameUIManager.Instance.hackProgressImage.fillAmount = 0;
        GameUIManager.Instance.hackProgressImage.gameObject.SetActive(false);
    }
}


using GameContent.Actors;
using GameContent.Actors.EnemySystems.Seekers;
using Systems;
using Systems.Inventory;
using UnityEngine;
using Type = Systems.Inventory.Type;

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
    
    public float ToolRange => _toolRange;
    
    public float MaxBattery => _maxBattery;
    
    private void Start()
    {
        _maxBattery = 100f;
        _currentBattery = _maxBattery;
        GameUIManager.Instance.UpdateText();
    }

    public void CheckDevice()
    {
        if (Hero.Instance.CurrentEquipedItem == null ||
            Hero.Instance.CurrentEquipedItem.type != Type.MultiTool)
        {
            return;
        }
        
        Ray ray = Hero.Instance.Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _toolRange))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Device"))
            {
                var device = hit.collider.GetComponent<Actor>();
                    
                if (device != null && device != _currentDevice && device is EnemyCamera enemyCamera && !enemyCamera.IsScanned)
                {
                    device = enemyCamera;
                    _currentDevice = device;
                    GameUIManager.Instance.UpdateInteractibleUI(enemyCamera.NetworkNode.nodeId, true);
                }
            }
        }
        else
        {
            if (_currentDevice != null)
            {
                _currentDevice = null;
                GameUIManager.Instance.UpdateInteractibleUI("", false);
            }
        }
    }

    public void ConsumeBattery(float amount)
    {
        _currentBattery -= amount;
        
        if (_currentBattery - amount < 0)
        {
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
            return;
        }
        
        GameUIManager.Instance.UpdateText();
    }

    public void ScanDevice()
    {
        if (Hero.Instance.CurrentEquipedItem.type == Type.MultiTool)
        {
            if (_currentDevice != null && 
                _currentDevice is EnemyCamera enemyCamera)
            {
                if (_currentDevice != null && _currentDevice.IsActive)
                {
                    _currentScanTimer = 0f;
                    isScanning = true;
                        
                    GameUIManager.Instance.HackProgressImage.gameObject.SetActive(true);
                    GameUIManager.Instance.HackProgressImage.fillAmount = 0;
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
            if (enemyCamera == null) return;
            if (enemyCamera.NetworkNode.hidden == false) return;
            
            float progress = _currentScanTimer / enemyCamera.ScanningTime;
            GameUIManager.Instance.HackProgressImage.fillAmount = Mathf.Clamp01(progress);
            
            if (_currentScanTimer >= enemyCamera.ScanningTime)
            {
                enemyCamera.NetworkNode.name.text = enemyCamera.NetworkNode.nodeId;
                enemyCamera.NetworkNode.hidden = false;
                enemyCamera.IsScanned = true;
                NetworkMapController.Instance.CheckAllHidden();
                CancelScan();
            }
        }
    }

    public void CancelScan()
    {
        isScanning = false;
        _currentScanTimer = 0f;
        _currentDevice = null;
        GameUIManager.Instance.HackProgressImage.fillAmount = 0;
        GameUIManager.Instance.HackProgressImage.gameObject.SetActive(false);
        GameUIManager.Instance.UpdateInteractibleUI("", false);
    }
}


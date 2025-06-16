using System.Collections.Generic;
using GameContent.Management;
using Systems;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class NetworkMapController : MonoBehaviour
{
    #region Variables

    public static NetworkMapController Instance;
    
    [SerializeField] private GameObject _networkMapUI;
    [SerializeField] private Button _validateButton, _resetButton;
    [SerializeField] private float unlinkCost, changeIDCost;
    [SerializeField] private List<RoomMap> _roomMaps = new List<RoomMap>();

    [SerializeField] private bool _isIDChanged;
    [SerializeField] private bool _isUnlinked;
    [SerializeField] private int numberOfChanges;
    [SerializeField] private int numberOfUnlinks;
    [SerializeField] private float totalHackingCost;

    private float tempVar;
    
    public float UnlinkCost => unlinkCost;
    public float ChangeIDCost => changeIDCost;
    public int NumberOfChanges
    {
        get => numberOfChanges;
        set => numberOfChanges = value;
    }
    
    public int NumberOfUnlinks
    {
        get => numberOfUnlinks;
        set => numberOfUnlinks = value;
    }
    
    public bool IsUnlinked
    {
        get => _isUnlinked;
        set => _isUnlinked = value;
    }
    
    public bool IsIDChanged
    {
        get => _isIDChanged;
        set => _isIDChanged = value;
    }
    
    public float TotalHackingCost
    {
        get => totalHackingCost;
        set => totalHackingCost = value;
    }

    #endregion
    
    private void Awake()
    {
        Instance = this;
        _validateButton.onClick.AddListener(ValidateChanges);
        _resetButton.onClick.AddListener(ResetNetworkMap);
    }

    private void Update()
    {
        if (_isIDChanged == false && _isUnlinked == false) return;

        tempVar += Time.deltaTime;
        
        if (_isIDChanged || _isUnlinked)
        {
            if (tempVar >= 1f)
            {
                Hero.Instance.MultiToolObject.ConsumeBattery(totalHackingCost);
                tempVar = 0f;
            }
        }
    }

    public void RevealRoom(RoomMap roomMap)
    {
       roomMap.gameObject.SetActive(true); 
    }
    
    public void OpenNetworkMap()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.MapOpen, 
            GameManager.Instance.playerTransform.position);
        _networkMapUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResetNetworkMap()
    {
        foreach (var roomMap in _roomMaps)
        {
            foreach (var mapLink in roomMap.MapLink)
            {
                mapLink.ResetLink();
            }
            
            _isIDChanged = false;
            numberOfChanges = 0;
        }
        
        _isIDChanged = false;
        _isUnlinked = false;
        numberOfUnlinks = 0;
        numberOfChanges = 0;
        totalHackingCost = 0f;
        tempVar = 0f;
    }
    
    public void CloseNetworkMap()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.MapClose, 
            GameManager.Instance.playerTransform.position);
        _networkMapUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ValidateChanges()
    {
        numberOfChanges = 0;
        numberOfUnlinks = 0;
        _isIDChanged = false;
        _isUnlinked = false;
        
        for (int i = 0; i < _roomMaps.Count; i++)
        {
            for (int j = 0; j < _roomMaps[i].MapLink.Count; j++)
            {
                if (_roomMaps[i].gameObject.activeSelf == false)
                {
                    return;
                }
                _roomMaps[i].MapLink[j].VerifyID(_roomMaps[i].MapLink[j].LinkNameInputField.text);
            }
        }
    }
    
    public void CheckAllHidden()
    {
        foreach (var roomMap in _roomMaps)
        {
            foreach (var mapLink in roomMap.MapLink)
            {
                mapLink.CheckHidden();
            }
        }
    }
}
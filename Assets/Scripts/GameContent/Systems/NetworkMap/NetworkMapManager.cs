using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class NetworkMapController : MonoBehaviour
{
    public static NetworkMapController Instance;
    
    [SerializeField] private Camera _networkMapCamera;
    [SerializeField] private GameObject _networkMapUI;
    [SerializeField] private Button _validateButton;
    [SerializeField] private List<RoomMap> _roomMaps = new List<RoomMap>();
    
    private void Awake()
    {
        Instance = this;
        _validateButton.onClick.AddListener(ValidateChanges);
    }

    public void RevealRoom(RoomMap roomMap)
    {
       roomMap.gameObject.SetActive(true); 
    }
    
    public void OpenNetworkMap()
    {
        _networkMapUI.SetActive(true);
        _networkMapCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void CloseNetworkMap()
    {
        _networkMapUI.SetActive(false);
        _networkMapCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ValidateChanges()
    {
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
}
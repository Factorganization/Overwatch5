using System.Collections;
using GameContent.Actors.EnemySystems.Seekers;
using GameContent.Management;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapLink : MonoBehaviour
{
     [SerializeField] private TMP_InputField _linkNameInputField;
     [SerializeField] private Button _sabotageButton;
     [SerializeField] private NetworkNode _linkedNode;
     [SerializeField] private float _suspicionValue;
     [SerializeField] private float _sabotageTime = 10f;
     
     private RoomMap _roomMap;
     private EnemyCamera _enemyCamera;

     public TMP_InputField LinkNameInputField => _linkNameInputField;
     
     public RoomMap RoomMap
     {
          get => _roomMap;
          set => _roomMap = value;
     }
     public EnemyCamera EnemyCamera { get { return _enemyCamera; } }

     private void Awake()
     {
          _sabotageButton.onClick.AddListener(UnlinkButton);
     }

     private void Start()
     {
          _enemyCamera = _linkedNode.actor;
          CheckHidden();
     }

     // Verify if the Id is correct, if it's not correct,
     // it's will change the information that the camera will send to the processor
     public void VerifyID(string playerInput)
     {
          if (_linkedNode.type != NodeType.Device) return;
          
          if (_linkNameInputField.text == _linkedNode.nodeId)
          {
               NetworkMapController.Instance.IsIDChanged = false;
               NetworkMapController.Instance.NumberOfChanges--;
               NetworkMapController.Instance.TotalHackingCost = NetworkMapController.Instance.ChangeIDCost * NetworkMapController.Instance.NumberOfChanges;
               return;
          }
          
          if (_linkedNode.hidden) return;
          
          if (_linkNameInputField.text != _linkedNode.nodeId)
          {
               foreach (var nodeID in _roomMap.MapLink)
               {
                    if (nodeID._linkedNode.nodeId == _linkNameInputField.text)
                    {
                         // Will change the information that the camera will send to the processor
                         _linkedNode.actor = nodeID._linkedNode.OriginalActor;
                         SuspicionManager.Manager.AddSuspicion(_suspicionValue);
                         return;
                    }
               }

               foreach (var nodes in _roomMap.Nodes)
               {
                    if (nodes.nodeId == _linkNameInputField.text)
                    {
                         return;
                    }
                    
                    SuspicionManager.Manager.StartTrackPlayer(Hero.Instance);
               }
               
               NetworkMapController.Instance.IsIDChanged = true;
               NetworkMapController.Instance.NumberOfChanges++;
               NetworkMapController.Instance.TotalHackingCost = NetworkMapController.Instance.ChangeIDCost * NetworkMapController.Instance.NumberOfChanges;
          }
     }
     
     public void UnlinkDevice()
     {
          if (_linkedNode.type != NodeType.Device) return;
          
          _enemyCamera = _linkedNode.actor as EnemyCamera;
          _enemyCamera!.IsActive = !_enemyCamera.IsActive;
          _sabotageButton.interactable = false;
     }

     public void CheckHidden()
     {
          if (_linkedNode.hidden)
          {
               _linkNameInputField.text = "...";
               _sabotageButton.interactable = false;
               _linkNameInputField.interactable = false;
          }
          else
          {
               _linkNameInputField.text = _linkedNode.nodeId;
               _sabotageButton.interactable = true;
               _linkNameInputField.interactable = true;
          }
     }

     private void UnlinkButton()
     {
          StartCoroutine(UnlinkDeviceCoroutine());
     }

     IEnumerator UnlinkDeviceCoroutine()
     {
          _enemyCamera = _linkedNode.actor as EnemyCamera;
          _enemyCamera!.IsActive = !_enemyCamera.IsActive;
          _sabotageButton.interactable = false;
          SuspicionManager.Manager.StartTrack(_linkedNode.actor as EnemyCamera);
          Hero.Instance.MultiToolObject.ConsumeBattery(NetworkMapController.Instance.UnlinkCost);
          yield return new WaitForSeconds(_sabotageTime);
          _sabotageButton.interactable = true;
          _enemyCamera!.IsActive = !_enemyCamera.IsActive;
     }
}

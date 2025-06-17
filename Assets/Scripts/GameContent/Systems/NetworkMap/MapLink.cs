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
     [SerializeField] private Toggle _sabotageToggle;
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

     private void Start()
     {
          _enemyCamera = _linkedNode.actor;
          _sabotageToggle.isOn = false;
          CheckHidden();
     }

     // Verify if the Id is correct, if it's not correct,
     // it's will change the information that the camera will send to the processor
     public void VerifyID(string playerInput)
     {
          if (_linkedNode.type != NodeType.Device) return;
          
          if (_linkedNode.hidden) return;
          
          if (_sabotageToggle.isOn)
          {
               UnlinkButton();
               
               NetworkMapController.Instance.NumberOfUnlinks++;
               
               if (NetworkMapController.Instance.NumberOfUnlinks > 0) 
               { NetworkMapController.Instance.IsUnlinked = true; }
               
               NetworkMapController.Instance.TotalHackingCost = 
                    NetworkMapController.Instance.ChangeIDCost * NetworkMapController.Instance.NumberOfChanges + 
                    NetworkMapController.Instance.UnlinkCost * NetworkMapController.Instance.NumberOfUnlinks;
          }
          else
          {
               _linkedNode.actor.IsActive = true;
          }
          
          if (_linkNameInputField.text == _linkedNode.nodeId) return;
          
          if (_linkNameInputField.text != _linkedNode.nodeId)
          {
               foreach (var nodeID in _roomMap.MapLink)
               {
                    
                    if (nodeID._linkedNode.nodeId == _linkNameInputField.text)
                    {
                         // Will change the information that the camera will send to the processor
                         _linkedNode.actor = nodeID._linkedNode.OriginalActor;
                         SuspicionManager.Manager.AddSuspicion(_suspicionValue);
                         NetworkMapController.Instance.NumberOfChanges++;
                         
                         if (NetworkMapController.Instance.NumberOfChanges > 0)
                         {
                              NetworkMapController.Instance.IsIDChanged = true;
                         }
                         
                         NetworkMapController.Instance.TotalHackingCost = 
                              NetworkMapController.Instance.ChangeIDCost * NetworkMapController.Instance.NumberOfChanges + 
                              NetworkMapController.Instance.UnlinkCost * NetworkMapController.Instance.NumberOfUnlinks;
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
               
               Debug.Log("AHHHHHHHHHHH");
               NetworkMapController.Instance.NumberOfChanges++;

               if (NetworkMapController.Instance.NumberOfChanges > 0)
               {
                    NetworkMapController.Instance.IsIDChanged = true;
               }
          }
          
          NetworkMapController.Instance.TotalHackingCost = 
               NetworkMapController.Instance.ChangeIDCost * NetworkMapController.Instance.NumberOfChanges + 
               NetworkMapController.Instance.UnlinkCost * NetworkMapController.Instance.NumberOfUnlinks;
     }
     
     public void UnlinkDevice()
     {
          if (_linkedNode.type != NodeType.Device) return;
          
          _enemyCamera = _linkedNode.actor as EnemyCamera;
          _enemyCamera!.IsActive = !_enemyCamera.IsActive;
          _sabotageToggle.interactable = false;
     }

     public void CheckHidden()
     {
          if (_linkedNode.hidden)
          {
               _linkNameInputField.text = "...";
               _sabotageToggle.interactable = false;
               _linkNameInputField.interactable = false;
          }
          else
          {
               _linkNameInputField.text = _linkedNode.nodeId;
               _sabotageToggle.interactable = true;
               _linkNameInputField.interactable = true;
          }
     }

     private void UnlinkButton()
     {
          _linkedNode.actor.IsActive = false;
     }

     public void ResetLink()
     {
          if (_linkedNode.hidden) return;
          
          _linkNameInputField.text = _linkedNode.nodeId;
          _linkedNode.actor = _linkedNode.OriginalActor;
          _sabotageToggle.isOn = false;
     }
}

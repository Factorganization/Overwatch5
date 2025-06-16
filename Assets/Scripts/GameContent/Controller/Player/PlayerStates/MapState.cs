using UnityEngine;

namespace GameContent.Controller.Player.PlayerStates
{
    public sealed class MapState : BasePlayerState
    {
        #region construuctors
        
        public MapState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }
        
         #endregion

         #region methodes

         public override void OnEnterState()
         {
             Cursor.visible = true;
             Cursor.lockState = CursorLockMode.None;
             rb.linearVelocity = Vector3.zero;
             
             NetworkMapController.Instance.OpenNetworkMap();
         }

         public override sbyte OnUpdate()
         {
             OnRelease();
             
             return 0;
         }

         public override sbyte OnFixedUpdate()
         {
             HandleGravity();
             
             return 0;
         }

         public override void OnExitState()
         {
             Cursor.visible = false;
             Cursor.lockState = CursorLockMode.Locked;
             playerMachine.PlayerModel.currentMoveMultiplier += 0.3f;
             
             NetworkMapController.Instance.CloseNetworkMap();
         }
         
         
         private void OnRelease()
         {
             if (dataSo.inputData.mapInput.action.WasPressedThisFrame())
                 stateMachine.SwitchState("move");
         }
         
         #endregion
    }
}
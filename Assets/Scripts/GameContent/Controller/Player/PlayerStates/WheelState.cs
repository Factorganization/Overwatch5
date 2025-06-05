using Systems.Inventory;
using UnityEngine;

namespace GameContent.Controller.Player.PlayerStates
{
    public sealed class WheelState : BasePlayerState
    {
        #region constructors
        
        public WheelState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            playerMachine.PlayerModel.currentMoveMultiplier -= 0.3f;
            _jumpCounter = 1;
            
            Inventory.Instance.RadialMenu.Open();
            
            OnSprint();
        }

        public override sbyte OnUpdate()
        {
            _jumpCounter += Time.deltaTime;
            
            HandleInputGather();
            OnRelease();
            
            if (dataSo.inputData.jumpInput.action.WasPressedThisFrame() && CheckGround())
                OnJump();
            
            OnSprint();
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            Move(playerMachine.PlayerModel.currentMoveMultiplier);
            
            if (_jumpCounter < GameConstants.AntiGroundGrabJumpTimer)
                return 0;

            HandleGravity();
            
            return 0;
        }

        public override void OnExitState()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerMachine.PlayerModel.currentMoveMultiplier += 0.3f;
            
            Inventory.Instance.RadialMenu.Close();
        }

        private void OnSprint()
        {
            if (playerMachine.PlayerModel.isCrouching)
                return;
            
            playerMachine.PlayerModel.currentMoveMultiplier = dataSo.inputData.sprintInput.action.IsPressed() ? dataSo.moveData.sprintMultiplier : 1;
        }
        
        private void OnJump()
        {
            _jumpCounter = 0;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * dataSo.jumpData.jumpStrength, ForceMode.VelocityChange);
            IsGrounded = false;
        }

        private void OnRelease()
        {
            if (!dataSo.inputData.wheelInput.action.IsPressed())
                stateMachine.SwitchState("move");
        }

        #endregion

        #region fields

        private float _jumpCounter;

        #endregion
    }
}
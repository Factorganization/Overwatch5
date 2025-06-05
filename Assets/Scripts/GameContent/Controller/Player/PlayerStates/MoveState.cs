using UnityEngine;

namespace GameContent.Controller.Player.PlayerStates
{
    public sealed class MoveState : BasePlayerState
    {
        #region constructors

        public MoveState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }

        #endregion

        #region methodes

        public override void OnEnterState()
        {
            OnSprint();
            Move(playerMachine.PlayerModel.currentMoveMultiplier);
            playerMachine.PlayerModel.isGrounded = true;
            playerMachine.PlayerModel.coyoteTime = dataSo.jumpData.jumpCoyoteTime;
            //TODO anims
        }

        public override sbyte OnUpdate()
        {
            HandleInputGather();
            HandleRotateInputGather();
            
            OnFall();
            OnIdle();
            OnJump();
            OnMap();
            OnWheel();
            
            OnSprint();
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            HandleGravity();
            Move(playerMachine.PlayerModel.currentMoveMultiplier);
            Look();
            return 0;
        }

        private void OnJump()
        {
            if (playerMachine.PlayerModel.jumpBufferTime > 0)
                stateMachine.SwitchState("jump");
        }
        
        private void OnSprint()
        {
            if (playerMachine.PlayerModel.isCrouching)
                return;
            
            playerMachine.PlayerModel.currentMoveMultiplier = dataSo.inputData.sprintInput.action.IsPressed() ? dataSo.moveData.sprintMultiplier : 1;
        }
        
        private void OnIdle()
        {
            if (inputDir.sqrMagnitude <= 0)
                stateMachine.SwitchState("idle");
        }

        private void OnMap()
        {
            if (dataSo.inputData.mapInput.action.WasPressedThisFrame())
                stateMachine.SwitchState("map");
        }
        
        private void OnWheel()
        {
            if (dataSo.inputData.wheelInput.action.WasPressedThisFrame())
                stateMachine.SwitchState("wheel");
        }
        
        private void OnFall()
        {
            if (CheckGround())
                return;
            
            stateMachine.SwitchState("fall");
        }
        
        #endregion
    }
}
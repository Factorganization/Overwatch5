using UnityEngine;

namespace GameContent.Controller.Player.PlayerStates
{
    public sealed class FallState : BasePlayerState
    {
        #region constructors
        
        public FallState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            IsGrounded = false;
        }
        
        public override sbyte OnUpdate()
        {
            HandleInputGather();
            HandleRotateInputGather();
            
            playerMachine.PlayerModel.coyoteTime -= Time.deltaTime;
            OnJump();
            
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            OnGrounded();
            
            HandleGravity();
            Move(playerMachine.PlayerModel.currentMoveMultiplier);
            Look();
            return 0;
        }

        private void OnJump()
        {
            if (playerMachine.PlayerModel.coyoteTime > 0
                && playerMachine.PlayerModel.jumpBufferTime > 0)
                stateMachine.SwitchState("Jump");
        }
        
        private void OnGrounded()
        {
            if (CheckGround())
                stateMachine.SwitchState("move");
        }

        #endregion
    }
}
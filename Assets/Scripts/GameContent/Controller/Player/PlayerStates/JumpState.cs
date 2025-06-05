using UnityEngine;

namespace GameContent.Controller.Player.PlayerStates
{
    public sealed class JumpState : BasePlayerState
    {
        #region constructors
        
        public JumpState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            
            playerMachine.PlayerModel.jumpBufferTime = 0;
            
            playerMachine.PlayerModel.castAddLength = 0;
            rb.AddForce(Vector3.up * dataSo.jumpData.jumpStrength, ForceMode.VelocityChange);
            IsGrounded = false;
        }

        public override sbyte OnUpdate()
        {
            _jumpCounter += Time.deltaTime;

            HandleInputGather();
            HandleRotateInputGather();
            
            OnFall();
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            Move(playerMachine.PlayerModel.currentMoveMultiplier);
            Look();
            
            if (_jumpCounter < GameConstants.AntiGroundGrabJumpTimer)
                return 0;
         
            OnGrounded();
            HandleGravity();
            return 0;
        }

        public override void OnExitState()
        {
            _jumpCounter = 0;
        }
        
        private void OnFall()
        {
            if (rb.linearVelocity.y < 0)
                stateMachine.SwitchState("fall");
        }

        private void OnGrounded()
        {
            if (CheckGround())
                stateMachine.SwitchState("move");
        }
        
        #endregion

        #region fields

        private float _jumpCounter;

        #endregion
    }
}
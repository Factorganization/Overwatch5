using UnityEngine;

namespace GameContent.Controller.Player.PlayerStates
{
    public sealed class CinematicState : BasePlayerState
    {
        #region constructors

        public CinematicState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }

        #endregion

        #region methodes

        public override void OnEnterState()
        {
        }

        public override sbyte OnUpdate()
        {
            HandleRotateInputGather();
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            Look();
            return 0;
        }
        
        public override void OnExitState()
        {
            rb.isKinematic = false;
            playerMachine.IsCinematic = false;
        }
        
        #endregion
    }
}
using UnityEngine;

namespace GameContent.Controller.Player.PlayerStates
{
    public sealed class MenuState : BasePlayerState
    {
        #region constructors
        
        public MenuState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            GameUIManager.Instance.TogglePauseMenu();
        }

        public override sbyte OnUpdate()
        {
            ChangeStateToIdle();
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            return 0;
        }

        public override void OnExitState()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void ChangeStateToIdle()
        {
            if (GameUIManager.Instance.PauseUI.gameObject.activeSelf == false)
            {
                stateMachine.SwitchState("idle");
            }
        }

        #endregion
    }
}

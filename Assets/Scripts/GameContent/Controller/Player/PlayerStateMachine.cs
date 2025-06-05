using System;
using System.Collections.Generic;
using GameContent.Controller.BaseMachine;
using GameContent.Controller.Player.PlayerStates;
using UnityEngine;

namespace GameContent.Controller.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region properties

        public PlayerDataSo DataSo => dataSo;

        public Transform CamRef => camRef;
        
        public Animator Animator => animator;

        public PlayerModel PlayerModel => _playerModel;
        
        #endregion
        
        #region methodes

        private void Start()
        {
            _playerModel = new PlayerModel(dataSo);
            _stateMachine = new GenericStateMachine(Enum.GetNames(typeof(ControllerState)).Length);
            var go = gameObject;

            var pSD = new Dictionary<string, BaseState>
            {
                {"idle", new IdleState(go, ControllerState.Idle, this)},
                {"move", new MoveState(go, ControllerState.Move, this)},
                {"jump", new JumpState(go, ControllerState.Jump, this)},
                {"fall", new FallState(go, ControllerState.Fall, this)},
                {"wheel", new WheelState(go, ControllerState.Wheel,  this)},
                {"map", new MapState(go, ControllerState.Map, this)},
                {"menu", new MenuState(go, ControllerState.Menu, this)}
            };
            
            _stateMachine.SetCallBacks((byte)ControllerState.Idle, "idle", pSD["idle"].OnInit, pSD["idle"].OnEnterState, 
                pSD["idle"].OnUpdate, pSD["idle"].OnFixedUpdate, pSD["idle"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Move, "move", pSD["move"].OnInit, pSD["move"].OnEnterState,
                pSD["move"].OnUpdate, pSD["move"].OnFixedUpdate, pSD["move"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Jump, "jump", pSD["jump"].OnInit, pSD["jump"].OnEnterState,
                pSD["jump"].OnUpdate, pSD["jump"].OnFixedUpdate, pSD["jump"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Fall, "fall", pSD["fall"].OnInit, pSD["fall"].OnEnterState, 
                pSD["fall"].OnUpdate, pSD["fall"].OnFixedUpdate, pSD["fall"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Wheel, "wheel", pSD["wheel"].OnInit, pSD["wheel"].OnEnterState,
                pSD["wheel"].OnUpdate, pSD["wheel"].OnFixedUpdate, pSD["wheel"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Map, "map", pSD["map"].OnInit, pSD["map"].OnEnterState,
                pSD["map"].OnUpdate, pSD["map"].OnFixedUpdate, pSD["map"].OnExitState, null);
            
            _stateMachine.SetCallBacks((byte)ControllerState.Menu, "menu", pSD["menu"].OnInit, pSD["menu"].OnEnterState,
                pSD["menu"].OnUpdate, pSD["menu"].OnFixedUpdate, pSD["menu"].OnExitState, null);
            
            _stateMachine.InitMachine();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            //Non = 0.05f;
            
            if (!_playerModel.isDead)
                _stateMachine.UpdateMachine();
        }

        private void FixedUpdate()
        {
            if (!_playerModel.isDead)
                _stateMachine.FixedUpdateMachine();
        }
        
        #endregion

        #region fields

        [SerializeField] private PlayerDataSo dataSo;
        
        [SerializeField] private Transform camRef;
        
        [SerializeField] private Animator animator;
        
        private GenericStateMachine _stateMachine;

        private PlayerModel _playerModel;

        private float Non
        {
            set
            {
                if (Mathf.Approximately(dataSo.cameraData.camSensitivity, 0.05f))
                    return;
                
                var color = Color.red;
                Debug.LogError(
                    $"<color=#{(byte)(color.r * 255f):X2}{(byte)(color.g * 255f):X2}{(byte)(color.b * 255f):X2}>azy elle est horrible votre sensi</color>");
                
            }
        }

        #endregion
    }
}
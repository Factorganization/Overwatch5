using System.Collections;
using Systems;
using GameContent.Controller.BaseMachine;
using Systems.Inventory;
using UnityEngine;

namespace GameContent.Controller.Player
{
    public class BasePlayerState : BaseState
    {
        #region properties

        protected bool IsGrounded
        {
            get => playerMachine.PlayerModel.isGrounded;
            set => playerMachine.PlayerModel.isGrounded = value;
        }

        public ControllerState StateFlag { get; }
        
        #endregion

        #region constructors

        protected BasePlayerState(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go)
        {
            StateFlag = state;
            this.playerMachine = playerMachine;
            dataSo = playerMachine.DataSo;
            rb = go.GetComponent<Rigidbody>();
            camRef = playerMachine.CamRef;
            animator = playerMachine.Animator;
        }

        #endregion

        #region methodes

        protected static float ClampSymmetric(float val, float clamper) => Mathf.Clamp(val, -clamper, clamper);

        protected void HandleInputGather()
        {
            inputDir = dataSo.inputData.moveInput.action.ReadValue<Vector2>();
            
            playerMachine.PlayerModel.jumpBufferTime -= Time.deltaTime;
            
            if (dataSo.inputData.jumpInput.action.WasPressedThisFrame())
                playerMachine.PlayerModel.jumpBufferTime = dataSo.jumpData.jumpBufferTime;

            /*if (dataSo.inputData.actionInput.action.WasPressedThisFrame())
            {
                var r = Physics.Raycast(camRef.position, camRef.forward, out var hit, 1.5f, LayerMask.GetMask("Actor"));
                if (r)
                    hit.transform.GetComponent<Actor>().OnAction(); //TODO a vomir et a changer
            }*/
            
            //TODO passer en interactState, no cam rota et move ralentit
            if (dataSo.inputData.actionInput.action.WasPressedThisFrame())
            {
                Hero.Instance.TryInteract();
            }

            if (dataSo.inputData.useInput.action.WasPressedThisFrame() && !Inventory.Instance.RadialMenu.isOpen)
            {
                Hero.Instance.UseEquippedItem();
            }

            if (dataSo.inputData.actionInput.action.IsPressed() && Hero.Instance.IsHacking)
            {
                Hero.Instance.ContinueHack();
            }
            else if (dataSo.inputData.actionInput.action.WasReleasedThisFrame() && Hero.Instance.IsHacking)
            {
                Hero.Instance.CancelHack();
            }
            
            if (dataSo.inputData.crouchInput.action.IsPressed() && playerMachine.PlayerModel.currentHeightTarget > dataSo.moveData.crouchHeight - 1)
            {
                playerMachine.PlayerModel.isCrouching = true;
                playerMachine.PlayerModel.currentHeightTarget = dataSo.moveData.crouchHeight - 1;
                playerMachine.PlayerModel.currentMoveMultiplier = dataSo.moveData.crouchMultiplier;
            }
            else if (!dataSo.inputData.crouchInput.action.IsPressed() && playerMachine.PlayerModel.currentHeightTarget < dataSo.groundCheckData.castBaseLength)
            {
                playerMachine.PlayerModel.isCrouching = false;
                playerMachine.PlayerModel.currentHeightTarget = dataSo.groundCheckData.castBaseLength;
                playerMachine.PlayerModel.currentMoveMultiplier = 1;
            }
        }

        protected void HandleRotateInputGather()
        {
            lookDir = dataSo.inputData.lookInput.action.ReadValue<Vector2>() / Time.deltaTime; //Deja corrigé sur correction de frame jump
        }
        
        protected void Move(float moveMultiplier)
        {
            playerMachine.PlayerModel.acceleration = (goRef.transform.forward * inputDir.y + goRef.transform.right * inputDir.x) * (dataSo.moveData.playerSpeed * moveMultiplier * GameConstants.ConstFixedDeltaTime);

            playerMachine.PlayerModel.tempLinearVelocity = rb.linearVelocity;
            playerMachine.PlayerModel.tempLinearVelocity.y = 0;

            playerMachine.PlayerModel.targetDir = playerMachine.PlayerModel.acceleration - playerMachine.PlayerModel.tempLinearVelocity;

            rb.AddForce(playerMachine.PlayerModel.targetDir * dataSo.moveData.accelDecelMultiplier, ForceMode.Acceleration);
        }

        protected void Look()
        {
            playerMachine.PlayerModel.camYaw += lookDir.x * dataSo.cameraData.camSensitivity * Time.fixedDeltaTime;
            playerMachine.PlayerModel.camPitch -= lookDir.y * dataSo.cameraData.camSensitivity * Time.fixedDeltaTime;
            playerMachine.PlayerModel.camPitch = ClampSymmetric(playerMachine.PlayerModel.camPitch, dataSo.cameraData.maxPitchAngle);
            
            camRef.Rotate(new Vector3(-lookDir.y * dataSo.cameraData.camSensitivity, 0, 0));
            camRef.localRotation = Quaternion.Euler(playerMachine.PlayerModel.camPitch, 0, 0);
            rb.rotation = Quaternion.Euler(0, playerMachine.PlayerModel.camYaw, 0);
            //rb.angularVelocity = new Vector3(0, lookDir.x * dataSo.cameraData.camSensitivity, 0);
        }
        
        //TODO refactor en separation ground Check et gravité
        protected void HandleGravity()
        {
            var sphereGroundCheck = Physics.SphereCast(goRef.transform.position,
                dataSo.groundCheckData.sphereCastRadius,
                Vector3.down,
                out var hit1,
                playerMachine.PlayerModel.currentHeightTarget + playerMachine.PlayerModel.castAddLength,
                dataSo.groundCheckData.groundLayer);

            if (!sphereGroundCheck)
            {
                playerMachine.PlayerModel.castAddLength = 0;
                playerMachine.PlayerModel.vertVelocity -= Time.deltaTime * dataSo.gravityData.fallAccelerationMultiplier;
                
                playerMachine.PlayerModel.vertVelocity = ClampSymmetric(playerMachine.PlayerModel.vertVelocity, dataSo.gravityData.maxFallSpeed * Time.fixedDeltaTime);
                rb.linearVelocity += Vector3.up * playerMachine.PlayerModel.vertVelocity;
            }

            else
            {
                playerMachine.PlayerModel.vertVelocity = 0;
                playerMachine.PlayerModel.castAddLength = dataSo.groundCheckData.additionalCastLength;

                var pointGroundCheck = Physics.Raycast(goRef.transform.position,
                    Vector3.down,
                    out var hit2,
                    playerMachine.PlayerModel.currentHeightTarget + 0.5f + playerMachine.PlayerModel.castAddLength, // 0.5f pour compenser le sphereCast radius en Rey
                    dataSo.groundCheckData.groundLayer);
                
                if (pointGroundCheck)
                {
                    var d = playerMachine.PlayerModel.currentHeightTarget + 0.5f - Mathf.Abs(goRef.transform.position.y - hit2.point.y);
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, d * dataSo.gravityData.slopeClosingSpeedMultiplier, rb.linearVelocity.z);
                    return;
                }
                
                var d2 = playerMachine.PlayerModel.currentHeightTarget + 0.5f - Mathf.Abs(goRef.transform.position.y - hit1.point.y);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, d2 * dataSo.gravityData.slopeClosingSpeedMultiplier, rb.linearVelocity.z);
            }
        }
        
        protected bool CheckGround()
        {
            var sphereGroundCheck = Physics.SphereCast(goRef.transform.position,
                dataSo.groundCheckData.sphereCastRadius,
                Vector3.down,
                out _,
                playerMachine.PlayerModel.currentHeightTarget + playerMachine.PlayerModel.castAddLength,
                dataSo.groundCheckData.groundLayer);

            return sphereGroundCheck;
        }
        
        #region to herit

        public override void OnInit(GenericStateMachine machine)
        {
            stateMachine = machine;
        }

        public override void OnEnterState()
        {
        }

        public override sbyte OnUpdate()
        {
            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            return 0;
        }

        public override void OnExitState()
        {
        }

        public override IEnumerator OnCoroutine()
        {
            yield return null;
        }

        #endregion

        #endregion

        #region fields

        protected readonly PlayerDataSo dataSo;
        
        protected readonly PlayerStateMachine playerMachine;

        protected readonly Rigidbody rb;

        protected readonly Transform camRef;
        
        protected readonly Animator animator;

        protected Vector2 inputDir;

        protected Vector2 lookDir;

        #endregion
    }
}
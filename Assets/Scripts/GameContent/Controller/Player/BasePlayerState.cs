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
            
            // Scanning Action
            if (dataSo.inputData.actionInput.action.WasReleasedThisFrame() && Hero.Instance.MultiToolObject.isScanning)
            {
                Hero.Instance.MultiToolObject.ScanDevice();
            }
            
            if (dataSo.inputData.actionInput.action.IsPressed() && Hero.Instance.MultiToolObject.isScanning)
            {
                Hero.Instance.MultiToolObject.Scanning();
            }
            else
            {
                Hero.Instance.MultiToolObject.CancelScan();
            }
            
            // Hacking Action
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

        protected void HandleSwayInputGather()
        {
            playerMachine.PlayerModel.walkInput = dataSo.inputData.moveInput.action.ReadValue<Vector2>().normalized;
            playerMachine.PlayerModel.lookInput = dataSo.inputData.lookInput.action.ReadValue<Vector2>() / Time.deltaTime;
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

        protected void UpdateSway()
        {
            Sway();
            SwayRotation();
            BobOffset();
            BobRotation();
            
            CompositePositionRotation();
        }

        private void Sway()
        {
            var invertLook = playerMachine.PlayerModel.lookInput * -dataSo.swayData.step;
            invertLook.x = Mathf.Clamp(invertLook.x, -dataSo.swayData.maxStepDistance, dataSo.swayData.maxStepDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -dataSo.swayData.maxStepDistance, dataSo.swayData.maxStepDistance);
            
            playerMachine.PlayerModel.swayPos = invertLook;
        }

        private void SwayRotation()
        {
            var invertLook = playerMachine.PlayerModel.lookInput * -dataSo.swayData.rotationStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -dataSo.swayData.maxRotationStep, dataSo.swayData.maxRotationStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -dataSo.swayData.maxRotationStep, dataSo.swayData.maxRotationStep);
            playerMachine.PlayerModel.swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
        }

        private void CompositePositionRotation()
        {
            playerMachine.HandRef.localPosition = Vector3.Lerp(playerMachine.HandRef.localPosition, playerMachine.PlayerModel.swayPos + playerMachine.PlayerModel.bobPosition, Time.deltaTime * SwayData.smooth);
            playerMachine.HandRef.localRotation = Quaternion.Slerp(playerMachine.HandRef.localRotation, Quaternion.Euler(playerMachine.PlayerModel.swayEulerRot) * Quaternion.Euler(playerMachine.PlayerModel.bobEulerRotation), Time.deltaTime * SwayData.smoothRot);
        }

        private void BobOffset()
        {
            playerMachine.PlayerModel.speedCurve += playerMachine.PlayerModel.walkInput.magnitude > 0.1f ? Time.deltaTime * ((playerMachine.PlayerModel.walkInput.x + playerMachine.PlayerModel.walkInput.y) * dataSo.swayData.bobExaggeration) + 0.01f * dataSo.swayData.moveAccel 
                : Time.deltaTime * ((playerMachine.PlayerModel.walkInput.x + playerMachine.PlayerModel.walkInput.y) * dataSo.swayData.bobExaggeration) + 0.01f;
                
            playerMachine.PlayerModel.bobPosition.x = playerMachine.PlayerModel.CurveCos * SwayData.bobLimit.x - playerMachine.PlayerModel.walkInput.x * SwayData.travelLimit.x;
            playerMachine.PlayerModel.bobPosition.y = playerMachine.PlayerModel.CurveSin * SwayData.bobLimit.y - playerMachine.PlayerModel.walkInput.y * SwayData.travelLimit.y;
            playerMachine.PlayerModel.bobPosition.z = -(playerMachine.PlayerModel.walkInput.y * SwayData.travelLimit.z);
        }

        private void BobRotation()
        {
            playerMachine.PlayerModel.bobEulerRotation.x = playerMachine.PlayerModel.walkInput.magnitude < 0.1f ? dataSo.swayData.multiplier.x * Mathf.Sin(2 * playerMachine.PlayerModel.speedCurve) : dataSo.swayData.multiplier.x * (Mathf.Sin(2 * playerMachine.PlayerModel.speedCurve) / 2);
            playerMachine.PlayerModel.bobEulerRotation.y = playerMachine.PlayerModel.walkInput.magnitude < 0.1f ? dataSo.swayData.multiplier.y * playerMachine.PlayerModel.CurveCos : 0;
            playerMachine.PlayerModel.bobEulerRotation.z = playerMachine.PlayerModel.walkInput.magnitude < 0.1f ? dataSo.swayData.multiplier.z * playerMachine.PlayerModel.CurveCos * playerMachine.PlayerModel.walkInput.x : 0;
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
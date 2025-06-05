using System;
using GameContent.ActorViews.Player;
using GameContent.Management;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace GameContent.Actors.EnemySystems.Seekers
{
    public class EnemyCamera : Actor
    {
        #region properties
        
        public override bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                coneRenderer.material.color = !isActive ? Color.clear : new Color(1, 1, 1, 0.2f);
            }
        }

        public Transform BaitTarget => baitTarget ?? transform;

        #endregion
        
        #region methodes

        public override void Init(Transform player)
        {
            playerTransform = player;
            _playerView = playerTransform.GetComponent<PlayerView>();
            IsActive = true;

            foreach (var c in cameraRotations)
                c.Init(gameObject);

            _pBDecal = new MaterialPropertyBlock();
            _pBCone = new MaterialPropertyBlock();
            
            coneRenderer.GetPropertyBlock(_pBCone);
        }

        public override void OnUpdate()
        {
            HandleCameraRotation();
            
            var s = HasPlayerInSight();
            switch (s)
            {
                case true when !_inSight:
                    _inSight = true;
                    coneRenderer.material.color = new Color(1, 0, 0, 0.2f);
                    _playerView.SightCount++;
                    break;
                case false when _inSight:
                    _inSight = false;
                    coneRenderer.material.color = new Color(1, 1, 1, 0.2f);
                    _playerView.SightCount--;
                    break;
            }

            if (s)
            {
                SuspicionManager.Manager.DetectionTime += Time.deltaTime;
            }
        }

        public override void OnFixedUpdate()
        {
            HandleGraphicAdaptation();
        }

        private bool HasPlayerInSight()
        {
            if (Vector3.Distance(transform.position, playerTransform.position) > range)
                return false;
            
            if (Vector3.Dot(transform.forward, (playerTransform.position - transform.position).normalized) < angle)
                return false;
            
            var r = Physics.Raycast(transform.position, playerTransform.position - transform.position, out var hit, range, collisionLayer);

            return r && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");
        }

        private void HandleCameraRotation()
        {
            foreach (var c in cameraRotations)
            {
                switch (c.rotationType)
                {
                    case RotationType.Continuous:
                        HandleContinuousRotation(c);
                        break;
                    
                    case RotationType.Step:
                        HandleStepRotation(c);
                        break;
                    
                    case RotationType.None:
                    default:
                        break;
                }
            }
        }

        private void HandleContinuousRotation(CameraRotation cameraRotation)
        {
            switch (cameraRotation.rotationAxis)
            {
                case RotationAxis.X:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.right : Vector3.right,
                        cameraRotation.rotationSpeed * Time.deltaTime);
                    break;
                case RotationAxis.Y:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.up : Vector3.up,
                        cameraRotation.rotationSpeed * Time.deltaTime);
                    break;
                case RotationAxis.Z:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.forward : Vector3.forward,
                        cameraRotation.rotationSpeed * Time.deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleStepRotation(CameraRotation cameraRotation)
        {
            if (cameraRotation.currentWaitTime > 0)
            {
                cameraRotation.currentWaitTime -= Time.deltaTime;
                return;
            }
            
            cameraRotation.angleRemains -= cameraRotation.rotationSpeed * Time.deltaTime * cameraRotation.currentSpeedSign;

            if (Mathf.Abs(cameraRotation.angleRemains) < Mathf.Abs(cameraRotation.rotationSpeed) * Time.deltaTime +
                GameConstants.FloatPointComparisonValue)
            {
                OnSwitchRotationTarget(cameraRotation);
                //Debug.Log($"{name} _ {cameraRotation.angleRemains}");
            }
            //Debug.Log($"{cameraRotation.angleRemains}");
            switch (cameraRotation.rotationAxis)
            {
                case RotationAxis.X:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.right : Vector3.right,
                        cameraRotation.rotationSpeed * Time.deltaTime * cameraRotation.currentSpeedSign);
                    break;
                case RotationAxis.Y:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.up : Vector3.up,
                        cameraRotation.rotationSpeed * Time.deltaTime * cameraRotation.currentSpeedSign);
                    break;
                case RotationAxis.Z:
                    transform.RotateAround(transform.position,
                        cameraRotation.rotationRef is RotationReferential.Local ? transform.forward : Vector3.forward,
                        cameraRotation.rotationSpeed * Time.deltaTime * cameraRotation.currentSpeedSign);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void OnSwitchRotationTarget(CameraRotation cameraRotation)
        {
            cameraRotation.currentWaitTime = cameraRotation.additionalStepData.waitTime;
            cameraRotation.currentStep = (cameraRotation.currentStep + 1) % cameraRotation.additionalStepData.stepNumbers;

            if (cameraRotation.currentStep == 0)
            {
                cameraRotation.currentStep = 1;
                
                if (cameraRotation.additionalStepData.loopAround)
                {
                    cameraRotation.angleRemains = 360 * cameraRotation.currentSpeedSign - cameraRotation.additionalStepData.stepNumbers * cameraRotation.additionalStepData.angle;
                    return;
                }
                
                cameraRotation.angleRemains = cameraRotation.currentSpeedSign * cameraRotation.additionalStepData.stepNumbers * cameraRotation.additionalStepData.angle;
                cameraRotation.currentSpeedSign *= -1;
                return;
            }
            
            if (!Mathf.Approximately(cameraRotation.currentSpeedSign, cameraRotation.startingSpeedSign))
                cameraRotation.currentSpeedSign *= -1;
            /*if (cameraRotation.currentSpeedSign < 0) //Pas joli mais marche
                cameraRotation.currentSpeedSign = Mathf.Sign(cameraRotation.angleRemains);*/
            
            cameraRotation.angleRemains = cameraRotation.additionalStepData.angle;
        }

        private void HandleGraphicAdaptation()
        {
            if (SuspicionManager.Manager.IsTracking)
            {
                _pBCone.SetFloat(SpeedFlash, 20);
                coneRenderer.SetPropertyBlock(_pBCone);
                camDecalProjector.material = flashCamDecal;
            }
            else
            {
                _pBCone.SetFloat(SpeedFlash, 0);
                coneRenderer.SetPropertyBlock(_pBCone);
                camDecalProjector.material = baseCamDecal;
            }
        }
        
        #endregion

        #region fields

        [SerializeField] private Transform baitTarget;
        
        [SerializeField] private MeshRenderer coneRenderer;

        [SerializeField] private DecalProjector camDecalProjector;
        
        [SerializeField] private Material baseCamDecal;
        
        [SerializeField] private Material flashCamDecal;
        
        [SerializeField] private CableLink cableLinkRef;

        [SerializeField] private float range;

        [Range(0f, 1f)]
        [SerializeField] private float angle;
        
        [SerializeField] private LayerMask collisionLayer;

        [SerializeField] private CameraRotation[] cameraRotations;
        
        private PlayerView _playerView;
        
        private MaterialPropertyBlock _pBCone;
        
        private MaterialPropertyBlock _pBDecal;
        
        private bool _inSight;
        
        private static readonly int SpeedFlash = Shader.PropertyToID("_Speed_Flash");

        #endregion
    }

    #region Cam Data
    
    [Serializable]
    internal class CameraRotation
    {
        #region methodes
        
        public void Init(GameObject actor)
        {
            angleRemains = additionalStepData.angle;
            currentStep = 1;
            currentSpeedSign = Mathf.Sign(angleRemains);
            startingSpeedSign = Mathf.Sign(angleRemains);
            currentWaitTime = 0;
        }
        
        #endregion
        
        #region fields
        
        public RotationType rotationType;

        public RotationReferential rotationRef;
        
        public RotationAxis rotationAxis;

        public float rotationSpeed;
        
        public CameraRotationAdditionalStepData additionalStepData;
        
        internal int currentStep;
        
        internal float currentWaitTime;

        internal float startingSpeedSign;
        
        internal float currentSpeedSign;

        internal float angleRemains;

        #endregion
    }

    [Serializable]
    internal class CameraRotationAdditionalStepData
    {
        #region fields
        
        public float angle;

        [Range(1, 5)]
        public int stepNumbers;
        
        public float waitTime;

        public bool loopAround;
        
        #endregion
    }
    
    internal enum RotationType : byte
    {
        None,
        Continuous,
        Step,
    }

    internal enum RotationReferential : byte
    {
        Local,
        Global,
    }
    
    internal enum RotationAxis : byte
    {
        X, Y, Z
    }
    
    #endregion
}
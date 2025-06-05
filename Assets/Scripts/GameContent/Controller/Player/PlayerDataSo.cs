using UnityEngine;
using UnityEngine.InputSystem;

namespace GameContent.Controller.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData")]
    public class PlayerDataSo : ScriptableObject
    {
        public InputData inputData;
        
        public MoveData moveData;
        
        public CameraData cameraData;
        
        public GroundCheckData groundCheckData;
        
        public GravityData gravityData;
        
        public JumpData jumpData;
    }

    [System.Serializable]
    public class InputData
    {
        public InputActionReference moveInput;
        
        public InputActionReference lookInput;

        public InputActionReference jumpInput;
        
        public InputActionReference sprintInput;
        
        public InputActionReference crouchInput;

        public InputActionReference actionInput;
        
        public InputActionReference mapInput;
        
        public InputActionReference wheelInput;

        public InputActionReference useInput;

        public InputActionReference menuInput;
    }

    [System.Serializable]
    public class MoveData
    {
        [Range(0.5f, 2f)]
        public float playerHeight;
        
        public float playerSpeed;
        
        [Range(1f, 2f)]
        public float sprintMultiplier;
        
        [Range(0f, 1f)]
        public float crouchMultiplier;
        
        [Range(1f, 2f)]
        public float crouchHeight;
        
        public float accelDecelMultiplier;
    }
    
    [System.Serializable]
    public class CameraData
    {
        [Range(0f, 1f)]
        public float camSensitivity;
        
        public float maxPitchAngle;
    }

    [System.Serializable]
    public class GroundCheckData
    {
        public LayerMask groundLayer;
        
        public float sphereCastRadius;

        public float castBaseLength;

        public float additionalCastLength;
    }
    
    [System.Serializable]
    public class GravityData
    {
        public float slopeClosingSpeedMultiplier;
        
        public float fallAccelerationMultiplier;

        public float maxFallSpeed;
    }
    
    [System.Serializable]
    public class JumpData
    {
        public float jumpStrength;
        
        public float jumpCoyoteTime;
        
        public float jumpBufferTime;
    }
}
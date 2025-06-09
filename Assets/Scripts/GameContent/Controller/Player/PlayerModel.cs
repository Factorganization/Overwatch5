using UnityEngine;

namespace GameContent.Controller.Player
{
    public class PlayerModel
    {
        #region properties

        #region hand properties

        public float CurveSin => Mathf.Sin(speedCurve);
        
        public float CurveCos => Mathf.Cos(speedCurve);

        #endregion

        #endregion
        
        #region constructors

        public PlayerModel(PlayerDataSo player)
        {
            currentHeightTarget = player.groundCheckData.castBaseLength;
        }

        #endregion
        
        #region fields
        
        #region main model
        
        public bool isDead = false;

        public bool isGrounded = true;
        
        public Vector3 targetDir = Vector3.zero;

        public Vector3 tempLinearVelocity = Vector3.zero;

        public Vector3 acceleration = Vector3.zero;

        public float camYaw = 0;

        public float camPitch = 0;

        public float castAddLength = 0;

        public float vertVelocity = 0;

        public float currentMoveMultiplier = 1;

        public float jumpBufferTime = 0;
        
        public float coyoteTime = 0;

        public bool isCrouching = false;
        
        public float currentHeightTarget;
        
        #endregion

        #region hand model

        public Vector2 walkInput;
        
        public Vector2 lookInput;
        
        public Vector3 swayPos;
        
        public Vector3 swayEulerRot;

        public Vector3 bobPosition;
        
        public Vector3 bobEulerRotation;
        
        public float speedCurve;

        #endregion

        #endregion
    }
}
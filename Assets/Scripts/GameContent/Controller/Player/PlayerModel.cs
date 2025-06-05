using UnityEngine;

namespace GameContent.Controller.Player
{
    public class PlayerModel
    {
        #region constructors

        public PlayerModel(PlayerDataSo player)
        {
            currentHeightTarget = player.groundCheckData.castBaseLength;
        }

        #endregion
        
        #region fields
        
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
    }
}
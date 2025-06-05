using UnityEngine;
using UnityEngine.Rendering;

namespace GameContent.ActorViews.Player
{
    public class PlayerView : MonoBehaviour
    {
        #region properties

        public int SightCount { get; set; }

        #endregion
        
        #region methodes

        private void Update()
        {
            if (SightCount <= 0)
                playerViewVolume.weight -= Time.deltaTime * 10;
            
            else
                playerViewVolume.weight += Time.deltaTime * 10;
            
            playerViewVolume.weight = Mathf.Clamp(playerViewVolume.weight, 0, 1);
        }

        #endregion

        #region fields

        [SerializeField] private Volume playerViewVolume;

        #endregion
    }
}
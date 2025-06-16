using UnityEngine;

namespace GameContent.Systems.GameSystem
{
    public class SunToggle : MonoBehaviour
    {
        #region methodes

        private void OnTriggerEnter(Collider other)
        {
            sun.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            sun.SetActive(false);
        }

        #endregion
        
        #region fields

        [SerializeField] private GameObject sun;

        #endregion
    }
}
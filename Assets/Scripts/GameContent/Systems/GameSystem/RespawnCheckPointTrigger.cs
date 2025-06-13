using GameContent.Management;
using UnityEngine;

public class RespawnCheckPointTrigger : MonoBehaviour
{
    #region variables
    
    [SerializeField] private Transform _respawnPoint;

    #endregion
    
    #region methods
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.respawnPoint == _respawnPoint) return;
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.respawnPoint = _respawnPoint;
        }
    }
    #endregion
}

using Systems;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameUIManager.Instance.DeathScreen.FallDeath = true;
            Hero.Instance.Health.TakeDamage(100);
        }
    }
}

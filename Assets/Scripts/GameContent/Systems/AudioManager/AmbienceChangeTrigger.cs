using UnityEngine;

public class AmbienceChangeTrigger : MonoBehaviour
{
    [Header("Ambience")]
    
    [SerializeField] private string parameterName;
    [SerializeField] private float parameterValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.SetAmbienceParameter(parameterName, parameterValue);
        }
    }
}

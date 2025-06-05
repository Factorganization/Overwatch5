using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{
    [Header("Area")]
    
    [SerializeField] private MusicArea musicArea;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.SetMusicArea(musicArea);
        }
    }
}

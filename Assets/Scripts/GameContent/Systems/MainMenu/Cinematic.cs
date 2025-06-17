using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic : MonoBehaviour
{
    [SerializeField] private Animator towerFall;
    
    private void Start()
    {
        StartCoroutine(LaunchCinematic());
    }

    IEnumerator LaunchCinematic()
    {
        yield return new WaitForSeconds(4f);
        towerFall.enabled = true;
        yield return new WaitForSeconds(6.15f);
        SceneManager.LoadScene("SC_CinematicMP4");
    }
}
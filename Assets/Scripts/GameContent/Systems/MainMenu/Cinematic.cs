using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LaunchCinematic());
    }

    IEnumerator LaunchCinematic()
    {
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene("SC_CinematicMP4");
    }
}
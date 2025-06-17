using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic : MonoBehaviour
{
    [SerializeField] private Animator towerFall;
    [SerializeField] private GameObject fade;
    
    private void Start()
    {
        fade.SetActive(false);
        StartCoroutine(LaunchCinematic());
    }

    IEnumerator LaunchCinematic()
    {
        yield return new WaitForSeconds(4f);
        towerFall.enabled = true;
        yield return new WaitForSeconds(6.15f);
        fade.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("SC_CinematicMP4");
    }
}
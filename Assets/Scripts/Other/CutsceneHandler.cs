using RunTimeContent.SceneManagement;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneHandler : MonoBehaviour
{
    public VideoPlayer VideoPlayer;

    void Start()
    {
        VideoPlayer.loopPointReached += OnVideoEnd;
        VideoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneLoader.Loader.LoadSceneGroup(0);
    }
}
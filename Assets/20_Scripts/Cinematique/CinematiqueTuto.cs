using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CinematiqueTuto : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public string SceneName;

    void Start()
    {
        VideoPlayer.loopPointReached += LoadScene;
    }
    void LoadScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneName);
    }
}

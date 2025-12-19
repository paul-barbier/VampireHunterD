using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CinematiqueTuto : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    [SerializeField] private string SceneName;
    [SerializeField] private string _spawnPointName;


    void Start()
    {
        VideoPlayer.loopPointReached += LoadScene;
    }
    void LoadScene(VideoPlayer vp)
    {
        SpawnManager.NextSpawnPoint = _spawnPointName;

        //TransiSceneAnimator.Instance.ChangeScene();
        SceneManager.LoadScene(SceneName);
    }
}

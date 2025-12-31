using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class CinematiqueTuto : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string sceneName;
    [SerializeField] private string spawnPointName;

    private void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void Start()
    {
        StartCoroutine(PrepareAndPlay());
    }

    private IEnumerator PrepareAndPlay()
    {
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = false;
        videoPlayer.skipOnDrop = true;

        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);

        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SpawnManager.NextSpawnPoint = spawnPointName;
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        yield return null;

        op.allowSceneActivation = true;
    }
}

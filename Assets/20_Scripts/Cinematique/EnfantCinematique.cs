using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EnfantCinematique : MonoBehaviour
{
    public VideoPlayer _videoPlayer;
    public GameObject _videoPlayerObject;
    public GameObject _health;

    public void Start()
    {
        _videoPlayer.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _health.gameObject.SetActive(false);
        _videoPlayer.gameObject.SetActive(true);

        _videoPlayer.loopPointReached += EndVideo;
    }

    private void EndVideo(VideoPlayer vp)
    {
        _health.gameObject.SetActive(true);
        Destroy(_videoPlayer);
        Destroy(this.gameObject);
    }
}

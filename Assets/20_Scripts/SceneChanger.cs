using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string _wantedScene;
    [SerializeField] private string _spawnPointName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        SpawnManager.NextSpawnPoint = _spawnPointName;

        TransiSceneAnimator.Instance.OnFadeBlackSwitch.RemoveAllListeners();
        TransiSceneAnimator.Instance.OnFadeBlackSwitch.AddListener(ChangeSceneNow);

        if (gameObject.CompareTag("Cinematic"))
        {
            TransiSceneAnimator.Instance.DestroyCinematiqueTrigger.RemoveAllListeners();
            TransiSceneAnimator.Instance.DestroyCinematiqueTrigger.AddListener(DeleteCinematiqueTrigger);
        }

        TransiSceneAnimator.Instance.ChangeScene();
    }

    public void ChangeSceneNow()
    {
        SceneManager.LoadScene(_wantedScene);
    }

    public void DeleteCinematiqueTrigger()
    {
        Destroy(gameObject);
    }
}


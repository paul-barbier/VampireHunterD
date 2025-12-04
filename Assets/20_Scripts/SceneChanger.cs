using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string _wantedScene;
    [SerializeField] private string _spawnPointName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Ok" + _spawnPointName);
            SpawnManager.NextSpawnPoint = _spawnPointName;
            SceneManager.LoadScene(_wantedScene);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager1 : MonoBehaviour
{
    public static SpawnManager1 Instance;

    public static string NextSpawnPoint;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(NextSpawnPoint)) return;

        GameObject spawn = GameObject.Find(NextSpawnPoint);
        if (spawn == null)
        {
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = spawn.transform.position;

        NextSpawnPoint = null;
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip HubMusic;
    [SerializeField] private AudioClip CataMusic;
    [SerializeField] private AudioClip HorlogeMusic;
    [SerializeField] private AudioClip BossMusic;

    private AudioSource audioSource;

    public static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        audioSource.Stop();

        if (sceneName == "Niv_HUB")
        {
            audioSource.clip = HubMusic;
        }
        else if (sceneName == "Niv_3_Crypte 1")
        {
            audioSource.clip = CataMusic;
        }
        else if (sceneName == "Salle3")
        {
            audioSource.clip = HorlogeMusic;
        }
        else if (sceneName == "Salle4")
        {
            audioSource.clip = BossMusic;
        }
        else
        {
            audioSource.clip = null;
        }
        audioSource.loop = true;

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
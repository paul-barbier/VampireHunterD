using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Music Clips")]
    [SerializeField] private AudioClip HubMusic;
    [SerializeField] private AudioClip CataMusic;
    [SerializeField] private AudioClip HorlogeMusic;
    [SerializeField] private AudioClip BossMusic;

    [Header("Volume")]
    [SerializeField] private float MusiqueVolume = 0.5f;

    [SerializeField] private Slider volumeSlider;

    private AudioSource audioSource;
    public static MusicManager instance;

    private Coroutine fadeCoroutine;
    [SerializeField] private float fadeDuration = 1.5f; // Durée du fade en secondes

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
    private void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
        if (volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        audioSource.volume = MusiqueVolume; // Valeur par défaut du volume
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip clipToPlay = null;

        if (sceneName == "Niv_HUB")
            clipToPlay = HubMusic;
        else if (sceneName == "Niv_3_Crypte 1")
            clipToPlay = CataMusic;
        else if (sceneName == "Salle3")
            clipToPlay = HorlogeMusic;
        else if (sceneName == "Salle4")
            clipToPlay = BossMusic;

        audioSource.loop = true;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusicCoroutine(clipToPlay, fadeDuration));
    }

    private IEnumerator FadeMusicCoroutine(AudioClip newClip, float duration)
    {
        float startVolume = MusiqueVolume;
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();

        audioSource.clip = newClip;

        if (newClip != null)
        {
            audioSource.Play();

            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                audioSource.volume = Mathf.Lerp(0f, startVolume, t / duration);
                yield return null;
            }
            audioSource.volume = startVolume;
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayBank : MonoBehaviour
{
    [SerializeField] Vector2 randomPitchRange = new Vector2(0.85f, 1.15f);
    [SerializeField] Vector2 volumeRandom = new Vector2(0.85f, 1.15f);
    private AudioSource audioSource;
    private SoundBank soundBank;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        soundBank = FindFirstObjectByType<SoundBank>();
    }

    public void playSound(string soundName)
    {
        AudioClip clip = soundBank.GetSound(soundName);
        if (clip == null)
        {
            Debug.LogWarning("No sound found with name: " + soundName);
            return;
        }

        //audioSource.pitch = UnityEngine.Random.Range(randomPitchRange.x, randomPitchRange.y);
        //audioSource.volume = UnityEngine.Random.Range(volume.x, volume.y);


        if (soundName == "Jump")
        {
            audioSource.volume = 0.3f;
        }
        else if (soundName == "MobExplo")
        {
            audioSource.volume = 0.1f;
            audioSource.pitch = Random.Range(randomPitchRange.x, randomPitchRange.y);
        }
        
        audioSource.PlayOneShot(clip);
    }

    public void PlayFootStep()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 2.5f, LayerMask.GetMask("Default"));
        string groundType = hit.collider.gameObject.tag;

        if (groundType == null)
            return;

        AudioClip clip = soundBank.GetSoundGround(groundType);
        if (clip == null)
        {
            Debug.LogWarning("No footstep sound found for ground type: " + groundType);
            return;
        }

        audioSource.pitch = UnityEngine.Random.Range(randomPitchRange.x, randomPitchRange.y);

        audioSource.PlayOneShot(clip);
    }

}

using UnityEngine;

public class LoopDelay : MonoBehaviour
{
    [SerializeField]AudioSource audioSource;

    public void Update()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.volume = Random.Range(0.3f, 0.5f);
            audioSource.PlayDelayed(Random.Range(1.0f, 2.0f));
        }
    }
}

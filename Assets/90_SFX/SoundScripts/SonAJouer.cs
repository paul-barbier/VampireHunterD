using UnityEngine;
using UnityEngine.Rendering;

public class SonAJouer : MonoBehaviour
{
    public void PlaySound(SoundType sound, float volume = 1)
    {
        AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)sound].Sounds[0], Camera.main.transform.position, volume);
    }
}

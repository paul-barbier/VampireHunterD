using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    private void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.JacketSound].Sounds[0], transform.position, 0.5f);
    }
}

using UnityEngine;

public class TpSounDmana : MonoBehaviour
{
    public void PlaySound()
    {
        AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.TP].Sounds[0], transform.position, 0.5f);
    }
}

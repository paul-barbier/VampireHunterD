using UnityEngine;

public class TpSounDmana : MonoBehaviour
{
    public void LoopSound()
    {
        AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.TP].Sounds[0], transform.position, 1f);
    }
}

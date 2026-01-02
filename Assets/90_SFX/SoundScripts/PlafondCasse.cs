using UnityEngine;

public class PlafondCasse : MonoBehaviour
{
    public void PlayPlafondCasse()
    {
        if (SoundManager.instance == null)
            return;

            AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.PlafondCasse].Sounds[0], transform.position, 1f);
    }
}

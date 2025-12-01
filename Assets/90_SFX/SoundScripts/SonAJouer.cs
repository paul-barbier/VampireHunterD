using UnityEngine;
using UnityEngine.Rendering;

public class SonAJouer : MonoBehaviour
{

    public void PlayMobFoot()
    {
        AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.MobFootsteps].Sounds[0], transform.position, 1f);
    }
}

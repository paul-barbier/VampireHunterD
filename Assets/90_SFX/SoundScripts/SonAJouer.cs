using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
public class SonAJouer : MonoBehaviour
{
    //[SerializeField] private UnityEvent PlaySoundEvent;
    public void PlayMobFoot()
    {
        //PlaySoundEvent.Invoke();
        if (SoundManager.instance == null) return;

        AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.MobFootsteps].Sounds[0], transform.position, 0.3f);
    }
}

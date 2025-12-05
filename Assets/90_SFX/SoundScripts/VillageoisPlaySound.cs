using UnityEngine;

public class VillageoisPlaySound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayMobFoot()
    {
        //PlaySoundEvent.Invoke();
        AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.StepsVillageois].Sounds[0], transform.position, 0.3f);
    }
}

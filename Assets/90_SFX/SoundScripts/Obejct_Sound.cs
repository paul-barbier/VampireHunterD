using UnityEngine;

public class Obejct_Sound : MonoBehaviour
{
    bool AlreadyPlayed = false;
    public SoundType SoundType;

    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.GetComponent<PlayerCharacter>();
        if (collision && AlreadyPlayed == false)
        { 
            AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.BatVFX].Sounds[0], transform.position, 0.4f);
            AlreadyPlayed = true;
        }

        else 
            return;
    }
}

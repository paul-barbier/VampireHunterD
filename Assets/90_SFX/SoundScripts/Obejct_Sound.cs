using UnityEngine;

public class Obejct_Sound : MonoBehaviour
{
    bool AlreadyPlayed = false;
    public SoundType SoundType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerCharacter>();
        if (collision && AlreadyPlayed == false)
        { 
            SoundManager.PlaySound(SoundType);
            AlreadyPlayed = true;
        }

        else 
            return;
    }
}

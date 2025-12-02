using UnityEngine;

public class Trigger2D : MonoBehaviour
{
    public class Obejct_Sound : MonoBehaviour
    {
        bool AlreadyPlayed = false;
        public SoundType SoundType;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.gameObject.GetComponent<PlayerCharacter>();
            if (collision && AlreadyPlayed == false)
            {
                SoundManager.PlaySound(SoundType, 1f);
                AlreadyPlayed = true;
            }

            else
                return;
        }
    }
}

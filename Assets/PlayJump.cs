using UnityEngine;

public class PlayJump : MonoBehaviour
{
    public void PlaySoundJump()
    {
        SoundManager.PlaySound(SoundType.Jump, 3f);
    }
}

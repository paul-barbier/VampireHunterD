using System.Collections.Generic;
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    private void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.BatExplosion].Sounds[0], transform.position, 10f);
    }
    
    
        
    
}

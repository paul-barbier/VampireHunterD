using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
public class SonAJouer : MonoBehaviour
{
    //[SerializeField] private UnityEvent PlaySoundEvent;

    [SerializeField] private RespawnManager _respawnManager;

    private void Awake()
    {
        Transform respawnManager = transform.parent.Find("RespawnManager");
        if (respawnManager != null)
            _respawnManager = respawnManager.GetComponent<RespawnManager>();
    }

    public void PlayMobFoot()
    {
        if (SoundManager.instance == null) 
            return;
        if(_respawnManager._isRespawn == false)
        {
            AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.MobFootsteps].Sounds[0], transform.position, 0.1f);
        }
        else
        {
            return;
        }
    }
}

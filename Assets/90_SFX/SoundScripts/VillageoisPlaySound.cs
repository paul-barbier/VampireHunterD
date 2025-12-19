using UnityEngine;

public class VillageoisPlaySound : MonoBehaviour
{
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
        if (_respawnManager._isRespawn == false)
        {
            AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.StepsVillageois].Sounds[0], transform.position, 1f);
        }
        else
        {
            return;
        }
    }
}

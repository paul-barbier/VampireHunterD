using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _objectToRespawn;
    [SerializeField] private float _respawnDelay;

    public void RespawnFonction()
    {
        if (!_objectToRespawn.activeInHierarchy)
        {
            StartCoroutine(RespawnDelay());
        }
    }

    IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(_respawnDelay);
        _objectToRespawn.SetActive(true);
    }
}

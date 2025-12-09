using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _objectToRespawn;
    [SerializeField] private float _respawnDelay;

    public bool _isRespawn = false;
    float _respawnTime = 0;

    private void Awake()
    {
        // Si non assigné, cherche automatiquement l’objet "Visual" dans le parent
        if (_objectToRespawn == null)
        {
            _objectToRespawn = GetComponent<SpriteRenderer>();
        }
    }
    private void Start()
    {
        _objectToRespawn.enabled = true;
    }

    private void Update()
    {
        _respawnTime += Time.deltaTime;
        if ( !_isRespawn )
            return;


        else if (_objectToRespawn.enabled == false && _respawnTime >= _respawnDelay )
        {
            _objectToRespawn.enabled = true;
            _respawnTime = 0f; 
            _isRespawn = false;
        }
    }

    public void RespawnFonction()
    {
        Debug.Log($"RespawnFonction appelée sur : {gameObject.name}");
        if (_objectToRespawn.enabled == false)
        {
            Debug.Log("Respawn called on : " + gameObject.name);
            _isRespawn = true;
            _respawnTime = 0f;
        }
    }
}

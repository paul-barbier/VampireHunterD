using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _objectToRespawn;
    [SerializeField] private float _respawnDelay;
    [SerializeField] private Collider2D _dashEnnemiHitbox;
    [SerializeField] private Collider2D _ennemiAttackHitbox;

    public bool _isRespawn = false;
    float _respawnTime = 0;

    private void Awake()
    {
        if (_dashEnnemiHitbox == null)
        {
            Transform visual = transform.parent.Find("Visual");
            if (visual != null)
                _dashEnnemiHitbox = visual.GetComponent<Collider2D>();
        }

        if (_ennemiAttackHitbox == null)
        {
            Transform attackZone = transform.parent.Find("Visual/AttackZone");
            if (attackZone != null)
                _ennemiAttackHitbox = attackZone.GetComponent<Collider2D>();
        }
        if (_objectToRespawn == null)
        {
            Transform visual = transform.parent.Find("Visual");
            if (visual != null)
                _objectToRespawn = visual.GetComponent<SpriteRenderer>();
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
            _dashEnnemiHitbox.enabled = true;
            _ennemiAttackHitbox.enabled = true;
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

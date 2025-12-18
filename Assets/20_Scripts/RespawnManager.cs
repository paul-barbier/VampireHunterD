using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _objectToRespawn;
    [SerializeField] private ParticleSystem RespawnAnim;
    [SerializeField] private float _respawnDelay;
    [SerializeField] private Collider2D _dashEnnemiHitbox;
    [SerializeField] private Collider2D _ennemiAttackHitbox;

    private Vector3 EnnemyPos;
    float timerAfterRespawn = 0f;
    public bool _isRespawn = false;
    float _respawnTime = 0;

    [SerializeField] private float dissolveDuration = 1f;
    [SerializeField] private float dissolveDelay = 0f;

    private bool respawnAnimPlayed = false;


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
        if (!_isRespawn)
            return;

        _respawnTime += Time.deltaTime;


        if (_objectToRespawn.enabled == false && _respawnTime >= _respawnDelay)
        {
            timerAfterRespawn += Time.deltaTime;

            if (!respawnAnimPlayed && timerAfterRespawn >= 0.1f)
            {
                Instantiate(RespawnAnim, new Vector3(_objectToRespawn.transform.position.x, _objectToRespawn.transform.position.y - 1.5f, _objectToRespawn.transform.position.z), Quaternion.identity);
                respawnAnimPlayed = true;
            }
            if (timerAfterRespawn > 0.5f)
            {
                _objectToRespawn.enabled = true;
                StartCoroutine(DissolveEffect());
                _dashEnnemiHitbox.enabled = true;
                _ennemiAttackHitbox.enabled = true;
                _respawnTime = 0f;
                _isRespawn = false;
                respawnAnimPlayed = false;
                timerAfterRespawn = 0f;
            }
        }
    }

    public void RespawnFonction()
    {
        Debug.Log($"RespawnFonction appelée sur : {_objectToRespawn.transform.parent.name}");
        if (_objectToRespawn.enabled == false)
        {
            Debug.Log("Respawn called on : " + gameObject.name);
            _isRespawn = true;
            _respawnTime = 0f;
        }
    }

    private static readonly int _dissolveID = Shader.PropertyToID("_Dissolve");
    private IEnumerator DissolveEffect()
    {
        if (_objectToRespawn == null)
            yield break;
        Material mat = _objectToRespawn.material;
        mat.SetFloat(_dissolveID, 0f);
        _objectToRespawn.enabled = true;

        float eslepsedTime = 0f;

        while (eslepsedTime < dissolveDuration)
        {
            eslepsedTime += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(1f, 0f, eslepsedTime / dissolveDuration);
            mat.SetFloat(_dissolveID, dissolveValue);
            yield return null;
        }
        mat.SetFloat(_dissolveID, 0f);
    }
}

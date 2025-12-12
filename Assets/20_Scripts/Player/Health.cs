using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private PlayerCharacter _character;
    public CheckPoints checkpoint;

    
    public bool _isInvincible = false;
    public bool _isDying;


    //Visuel
    [SerializeField] private Animator HpAnime;
    [SerializeField] private Animator AnimMort;

    [Header("Time Stats")]
    [SerializeField] private float _hurtDisplayTime = 0.2f;
    [SerializeField] private float _hurtFadeOutTime = 0.2f;

    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _hurtEffect;
    [SerializeField] private Material _Material;

    [Header("Intensity Stats")]
    [SerializeField] private float _maxIntensity = 1.0f;
    [SerializeField] private float _vignetteIntensityStat = 0.1f;

    private int _voronoiIntensity = Shader.PropertyToID("_NoiseIntensity");
    private int _vignetteIntensity = Shader.PropertyToID("_Intensity");

    private const float VIGNETTE_BASE_INTENSITY = 0.2f;
    private const float VORONOI_BASE_INTENSITY = 0.0f;
    private Material _hurtShader;

    private void Start()
    {
        _hurtShader = _character._mesh.gameObject.GetComponent<Renderer>().material;
        _currentHealth = _maxHealth;
        _character = GetComponent<PlayerCharacter>();
        _hurtEffect.SetActive(false);
        AnimMort.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (_isDying)
            _character.DisableAllMovement();
    }

    public void TakeDamage(int damages)
    {
        if (_isInvincible)
            return;
        _currentHealth -= damages;
        UpdateBar();
        _hurtShader.SetFloat("_Damaged", 1.0f);

        if (_currentHealth <= 0)
            Die();
    }

    public void UpdateBar()
    {
        float ratio = (float)_currentHealth / _maxHealth;

        StartCoroutine(Hurt());
        SoundManager.PlaySound(SoundType.D_Dmg, 5f);

        int state = 0;
        if (ratio > 0.75f)
        {
            state = 0;
        }
        else if (ratio > 0.50f)
        {
            state = 1;
        }
        else if (ratio > 0.25f)
        {
            state = 2;
        }
        else if (ratio > 0f)
        {
            state = 3;
        }
        else
        {
            state = 4;
        }

        HpAnime.SetInteger("HealthState", state);
    }

    public void GetHeal(int heal)
    {
        _currentHealth += heal;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        UpdateBar();
    }

    public int GetHealth()
    {
        return _currentHealth;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    private IEnumerator Hurt()
    {
        _hurtEffect.SetActive(true);
        _isInvincible = true;
        yield return new WaitForSeconds(1.5f);
        _Material.SetFloat(_vignetteIntensity, VIGNETTE_BASE_INTENSITY);
        _Material.SetFloat(_voronoiIntensity, VORONOI_BASE_INTENSITY);

        yield return new WaitForSeconds(_hurtDisplayTime);

        //float elapsedTime = 0.0f;
        //while (elapsedTime < _hurtFadeOutTime)
        //{
        //    elapsedTime += Time.deltaTime;


        //    float vignetteIntensity = Mathf.Lerp(VIGNETTE_BASE_INTENSITY, 0f, elapsedTime / _hurtFadeOutTime);
        //    float voronoiIntensity = Mathf.Lerp(VORONOI_BASE_INTENSITY, 0f, elapsedTime / _hurtFadeOutTime);

        //    _Material.SetFloat(_vignetteIntensity, vignetteIntensity);
        //    _Material.SetFloat(_voronoiIntensity, voronoiIntensity);

        //    yield return null;
        //}
        Debug.Log("End Hurt");
        _isInvincible = false;

        _hurtShader.SetFloat("_Damaged", 0.0f);

        _hurtEffect.SetActive(false);
    }

    public void Die()
    {
        _isDying = true;
        _character._mesh.rotation = Quaternion.Euler(0, _character._currentMeshRotation.y, 0);
        StartCoroutine(Dying());
    }

    IEnumerator Dying()
    {

        _character._DAnimation.SetBool("IsDying", true);
        _isInvincible = true;

        yield return new WaitForSeconds(1.0f);
        AnimMort.gameObject.SetActive(true);
        AnimMort.SetBool("AnimDeath", true);
        yield return new WaitForSeconds(1.5f);
        Respawn();
    }

    private void Respawn()
    {
        if (checkpoint)
        {
            AnimMort.SetBool("AnimDeath", false);
            AnimMort.gameObject.SetActive(false);

            _character.transform.position = checkpoint.respawnPoint.position;

            GetHeal(GetMaxHealth());
            UpdateBar();
            _character._isDashing = false;
            _character._isJumping = false;
            _character._movementDisabled = false;
            _character._DAnimation.SetBool("IsDying", false);
            _isDying = false;
            _isInvincible = false;
        }
    }
}

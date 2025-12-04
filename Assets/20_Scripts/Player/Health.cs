using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    PlayerCharacter _character;

    //Visuel
    [SerializeField] private Animator HpAnime;

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


    private void Start()
    {
        _currentHealth = _maxHealth;
        _character = GetComponent<PlayerCharacter>();
        _hurtEffect.SetActive(false);
    }

    public void TakeDamage(int damages)
    {
        _currentHealth -= damages;
        if (_currentHealth <= 0)
            _character.Die();
        UpdateBar();
    }

    public void UpdateBar()
    {
        float ratio = (float)_currentHealth / _maxHealth;

        int state = 0;
        if (ratio > 0.75f)
        {
            state = 0;
            SoundManager.PlaySound(SoundType.D_Dmg, 5f);
            StartCoroutine(Hurt());

        }
        else if (ratio > 0.50f)
        {
            state = 1;
            SoundManager.PlaySound(SoundType.D_Dmg, 5f);
            StartCoroutine(Hurt());
        }
        else if (ratio > 0.25f)
        {
            state = 2;
            SoundManager.PlaySound(SoundType.D_Dmg, 5f);
            StartCoroutine(Hurt());
        }
        else
        {
            state = 3;
            SoundManager.PlaySound(SoundType.D_Dmg, 5f);
            StartCoroutine(Hurt());
        }

        HpAnime.SetInteger("HealthState", state);
    }

    public IEnumerator CdDmg()
    {
        yield return new WaitForSeconds(5.0f);
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
        yield return new WaitForSeconds(0.5f);
        //_Material.SetFloat(_vignetteIntensity, VIGNETTE_BASE_INTENSITY);
        //_Material.SetFloat(_voronoiIntensity, VORONOI_BASE_INTENSITY);

        //yield return new WaitForSeconds(_hurtDisplayTime);

        //float elapsedTime = 0.0f;
        //while (elapsedTime < _hurtFadeOutTime)
        //{
        //    elapsedTime += Time.deltaTime;


        //    float vignetteIntensity = Mathf.Lerp(VIGNETTE_BASE_INTENSITY, 0f, elapsedTime / _hurtFadeOutTime);
        //    float voronoiIntensity = Mathf.Lerp(VORONOI_BASE_INTENSITY, 0f,elapsedTime/_hurtFadeOutTime);

        //    _Material.SetFloat(_vignetteIntensity, vignetteIntensity);
        //    _Material.SetFloat(_voronoiIntensity, voronoiIntensity);

        //    yield return null;
        //}
        Debug.Log("End Hurt");
            _hurtEffect.SetActive(false);
    }
}

using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        Init();
    }

    private void Init()
    {
        _materials = new Material[_spriteRenderers.Length];

        // Assign sprite renderer materials to _materials
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        // Set the color
        SetFlashColor();

        //Lerp the flash amount
        float currentFlashAmount = 0f;
        float elapsedTime = 0f; 
        while (elapsedTime < _flashTime)
        {
            // iterate elapsedTime
            elapsedTime += Time.deltaTime;

            // Lerp the flash amount
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }
    }

    private void SetFlashColor()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        //Set the flash amount
        for (int i =0; i< _materials.Length;i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}

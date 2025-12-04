using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Diss : MonoBehaviour
{

    [SerializeField] private float _dissolveSpeed = 1f;
    [SerializeField] private float _dissolveDelay = 0f;
    [SerializeField] private PlayerCharacter _playerCharacter;
    private SpriteRenderer[] _spriteRenderer;
    private Material[] _materials;

    private int _dissolveAmount = Shader.PropertyToID("_Dissolve");

    private void Start()
    {
        _spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        _materials = new Material[_spriteRenderer.Length];
        for (int i = 0; i < _spriteRenderer.Length; i++)
        {
            _materials[i] = _spriteRenderer[i].material;
        }
    }

    private IEnumerator Vanish(bool useDiss)
    {
        float elaspedTime = 0f;
        while (elaspedTime < _dissolveSpeed)
        {
            elaspedTime += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(0f, 1.1f, elaspedTime / _dissolveSpeed);
            for (int i = 0; i < _materials.Length; i++)
            {
                if (useDiss)
                    _materials[i].SetFloat(_dissolveAmount, dissolveValue);
            }
            yield return null;
        }
    }

    private IEnumerator Appear(bool useDiss)
    {
        float elaspedTime = 0f;
        while (elaspedTime < _dissolveSpeed)
        {
            elaspedTime += Time.deltaTime;
            float dissolveValue = Mathf.Lerp(1.1f, 0f, elaspedTime / _dissolveSpeed);
            for (int i = 0; i < _materials.Length; i++)
            {
                if (useDiss)
                    _materials[i].SetFloat(_dissolveAmount, dissolveValue);
            }
            yield return null;
        }
    }

    private void Update()
    {
        if (_playerCharacter.ChauveSourisD.activeSelf) 
        {
            StartCoroutine(Appear(true));
        }
        else 
        {
            StartCoroutine(Vanish(true));
        }
    }


}

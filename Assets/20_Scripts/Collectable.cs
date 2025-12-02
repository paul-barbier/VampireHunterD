using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    PlayerCharacter character;

    [SerializeField] private float _oscillationAmplitude = 0.0f;
    [SerializeField] private float _oscillationFrequency = 0.0f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject CollectibleUI;
    [SerializeField] private GameObject _playerCharacterMovement;

    private Vector3 _basePosition = Vector3.zero;

    [SerializeField] private bool _hasBeenCollected;

    private void Awake()
    {
        _basePosition = transform.position;
    }

    private void Start()
    {
        CollectibleUI.SetActive(false);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hasBeenCollected = false;
        _spriteRenderer.enabled = true;
    }

    private void Update()
    {
        float oscillation = Mathf.Sin(Time.time * _oscillationFrequency);
        oscillation = (oscillation + 1.0f) / 2.0f;
        oscillation *= _oscillationAmplitude;
        transform.position = _basePosition + new Vector3(0.0f, oscillation, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        character = collision.GetComponent<PlayerCharacter>();

        Debug.Log("Collectible pris");
        _hasBeenCollected = true;
        _spriteRenderer.enabled = false;
        StartCoroutine(WaitForGrounded());
    }

    private IEnumerator WaitForGrounded()
    {
        // Tant que le joueur n'est pas au sol, on attend
        yield return new WaitUntil(() => character.IsGrounded);

        Debug.Log("IsGrounded Collectible");
        _playerCharacterMovement.SetActive(false);
        CollectibleUI.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    PlayerCharacter character;
    [SerializeField] Dialogue dialogue;

    [SerializeField] private float _oscillationAmplitude = 0.0f;
    [SerializeField] private float _oscillationFrequency = 0.0f;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _collectibleUI;
    [SerializeField] private GameObject _playerCharacterMovement;
    public float _skipDelay = 0.0f;
    private Collider2D _collider;

    private Vector3 _basePosition = Vector3.zero;
    private bool _hasBeenCollected;
    public bool _CollectibleUIShowing = false;

    private void Awake()
    {
        _basePosition = transform.position;
    }

    private void Start()
    {
        _collectibleUI.SetActive(false);
        _collider = GetComponent<Collider2D>();
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
        SkipCollectible();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        character = collision.GetComponent<PlayerCharacter>();

        Debug.Log("Collectible pris");
        _hasBeenCollected = true;
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        StartCoroutine(WaitForGrounded());
    }

    private IEnumerator WaitForGrounded()
    {
        // Tant que le joueur n'est pas au sol, on attend
        yield return new WaitUntil(() => character.IsGrounded);

        Debug.Log("IsGrounded Collectible");
        _playerCharacterMovement.SetActive(false);
        _collectibleUI.SetActive(true);
        _CollectibleUIShowing = true;
        StartCoroutine(dialogue.SkipDelay());
    }

    private void SkipCollectible()
    {
        if (dialogue._skipCollectible == true && _CollectibleUIShowing == true && _hasBeenCollected == true)
        {
            _collectibleUI.SetActive(false);
            _playerCharacterMovement.SetActive(true);
            _CollectibleUIShowing = false;
            _hasBeenCollected = false;
            dialogue._skipCollectible = false;
        }
    }
}

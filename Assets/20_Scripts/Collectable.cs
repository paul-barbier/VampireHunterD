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

    bool _waitingGround = false;

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

        character = FindFirstObjectByType<PlayerCharacter>();

    }

    private void Update()
    {
        float oscillation = Mathf.Sin(Time.time * _oscillationFrequency);
        oscillation = (oscillation + 1.0f) / 2.0f;
        oscillation *= _oscillationAmplitude;
        transform.position = _basePosition + new Vector3(0.0f, oscillation, 0.0f);
        SkipCollectible();

        WaitForGrounded();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Collectible pris");
            _hasBeenCollected = true;
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            _waitingGround = true;
        }

    }

    private void WaitForGrounded()
    {
        if (!_waitingGround || !character.IsGrounded) 
            return;
        _waitingGround = false;

        Debug.Log("IsGrounded Collectible");
        _playerCharacterMovement.SetActive(false);
        _collectibleUI.SetActive(true);
        dialogue._CollectibleUIShowing = true;
        dialogue.SkipDelay();
        Time.timeScale = 0.0f;
    }

    private void SkipCollectible()
    {
        if (dialogue._skipCollectible == true && dialogue._CollectibleUIShowing == true && _hasBeenCollected == true)
        {
            _collectibleUI.SetActive(false);
            _playerCharacterMovement.SetActive(true);
            dialogue._CollectibleUIShowing = false;
            _hasBeenCollected = false;
            dialogue._skipCollectible = false;
            dialogue._canSkipCollectible = false;
            Time.timeScale = 1.0f;
        }
    }
}

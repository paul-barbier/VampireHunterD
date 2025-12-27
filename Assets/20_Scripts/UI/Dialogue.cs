using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    private PlayerCharacter _playerMovementScript;
    private Collectable _collectible;
    public bool _CollectibleUIShowing = false;


    [SerializeField] private GameObject _playerCharacterMovement;

    [SerializeField] private GameObject _dialogueCanva;
    [SerializeField] private int _dialogueTime;

    [SerializeField] private TMP_Text _speakerText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Image _portraitImage;

    [SerializeField] private string[] _speaker;
    [SerializeField][TextArea] private string[] _dialogueWords;
    [SerializeField] private Sprite[] _portrait;

    public bool _dialogueActivated;
    public bool _skipCollectible = false;
    public bool _canSkipCollectible = false;
    public bool _isCinematique = false;
    private int _step;

    private void Start()
    {
        _collectible = FindAnyObjectByType<Collectable>();
        //_playerMovementScript = FindAnyObjectByType<PlayerCharacter>();
        //if (_playerMovementScript == null)
        //{
        //    Debug.Log("Player null");
        //}

        _dialogueActivated = false;
        _dialogueCanva.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _dialogueActivated = true;
            _dialogueCanva.SetActive(true);
            //SettingVelocity();
            ShowStep();
        }
    }

    private void ShowStep()
    {
        _speakerText.text = _speaker[_step];
        _dialogueText.text = _dialogueWords[_step];
        _portraitImage.sprite = _portrait[_step];
    }

    public void SkipDialogue()
    {
        if (!_canSkipCollectible)
            return;

        _step++;
        if (_step >= _speaker.Length)
        {
            _dialogueCanva.SetActive(false);
            _step = 0;
            _dialogueActivated = false;
        }
        else
        {
            ShowStep();
        }
        if (_canSkipCollectible == true && _CollectibleUIShowing == true)
        {
            Debug.Log("SkipCollectible");
            _skipCollectible = true;
        }
    }

    private void Update()
    {
        SkipDelaywaiting();

        if (_dialogueActivated && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            SkipDialogue();
        }

        if (_CollectibleUIShowing && _canSkipCollectible && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            _skipCollectible = true;
        }
    }

    bool isSkipDelaywaiting = false;
    float timer = 0f;

    public void SkipDelay()
    {
        isSkipDelaywaiting = true;
    }

    private void SkipDelaywaiting()
    {
        if (!isSkipDelaywaiting)
            return;

        timer += Time.unscaledDeltaTime;
        if (timer > _collectible._skipDelay)
        {
            _canSkipCollectible = true;
            timer = 0f;
            isSkipDelaywaiting = false;
        }
    }
}

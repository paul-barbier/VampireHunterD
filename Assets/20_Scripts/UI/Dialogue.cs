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
    private int _step;

    [SerializeField] private bool _Cinematic;

    private void Start()
    {
        if (_Cinematic == false)
        {
            _playerMovementScript = FindAnyObjectByType<PlayerCharacter>();
            if (_playerMovementScript == null)
            {
                Debug.Log("Player null");
            }

            _dialogueActivated = false;
            _dialogueCanva.SetActive(false);
        }
        else if (_Cinematic == true)
        {
            Debug.Log("Cinematique");
            _dialogueActivated = true;
            _dialogueCanva.SetActive(true);
            ShowStep();
        }
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_Cinematic == false)
        {
            if (collision.CompareTag("Player"))
            {
                _dialogueActivated = true;
                _dialogueCanva.SetActive(true);
                SettingVelocity();
                ShowStep();
            }
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
        if (_Cinematic == false && _dialogueActivated)
        {
            _step++;
            if (_step >= _speaker.Length)
            {
                _dialogueCanva.SetActive(false);
                _step = 0;
                _dialogueActivated = false;
                Destroy(this);
                _playerCharacterMovement.SetActive(true);
            }
            else
            {
                ShowStep();
            }
        }

        else if (_Cinematic == true && _dialogueActivated)
        {
            Debug.Log("CinematiqueInput");
            return;
        }
    }

    private void SettingVelocity()
    {
        _playerMovementScript._forceToAdd = Vector2.zero;
        _playerCharacterMovement.SetActive(false);
    }


}

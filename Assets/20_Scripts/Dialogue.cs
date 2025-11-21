using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueCanva;
    [SerializeField] private int _dialogueTime;

    [SerializeField] private TMP_Text _speakerText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Image _portraitImage;

    [SerializeField] private string[] _speaker;
    [SerializeField][TextArea] private string[] _dialogueWords;
    [SerializeField] private Sprite[] _portrait;

    private bool _dialogueActivated;
    private int _step;

    private void Start()
    {
        _dialogueActivated = false;
        _dialogueCanva.SetActive(false);
    }

    private void Update()
    {
        if (_dialogueActivated && Input.GetKeyDown(KeyCode.E))
        {
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _dialogueActivated = true;
            _dialogueCanva.SetActive(true);
            ShowStep();
        }
    }

    private void ShowStep()
    {
        _speakerText.text = _speaker[_step];
        _dialogueText.text = _dialogueWords[_step];
        _portraitImage.sprite = _portrait[_step];
    }
}

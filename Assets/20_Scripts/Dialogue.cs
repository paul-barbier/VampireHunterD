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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_step >= _speaker.Length)
            {
                _dialogueCanva.SetActive(false);
                _step = 0;
            }
            else
            {
            _dialogueActivated = true;
            _dialogueCanva.SetActive(true);
            _speakerText.text = _speaker[_step];
            _dialogueText.text = _dialogueWords[_step];
            _portraitImage.sprite = _portrait[_step];
            StartCoroutine(WaitingTextFinish());
            _step += 1;
            }
        }
    }

    IEnumerator WaitingTextFinish()
    {
        yield return new WaitForSeconds(_dialogueTime);   
    }
}

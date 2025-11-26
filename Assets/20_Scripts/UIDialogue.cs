using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class UIDialogue : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu, _optionsMenu;
    [SerializeField] private GameObject _dialogueUI;


    public void Start()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    public void Pause()
    {
        if (!_pauseMenu.activeInHierarchy && !_optionsMenu.activeInHierarchy)
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (_pauseMenu.activeInHierarchy || _optionsMenu.activeInHierarchy)
        {
            Time.timeScale = 1.0f;
            _pauseMenu.SetActive(false);
            _optionsMenu.SetActive(false);
        }
    }

    public void Reprendre()
    {
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    public void OptionsPauseMenu()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        if (_optionsMenu != null)
        {
            _pauseMenu.SetActive(true);
            _optionsMenu.SetActive(false);
        }
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class UIDialogue : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu, _optionsMenu, _commandesMenu;
    [SerializeField] private GameObject _pauseFirstButton, _optionsFirstButton, _optionsClosedButton, _commandesFirstButton, _commandesClosedButton;

    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private Dialogue _dialogue;

    public void Start()
    {
        Time.timeScale = 1.0f;

        if (_pauseMenu != null) 
            _pauseMenu.SetActive(false);

        if (_optionsMenu != null) 
            _optionsMenu.SetActive(false);

        if (_commandesMenu != null) 
            _commandesMenu.SetActive(false);

        _dialogue = GetComponentInChildren<Dialogue>();
    }

    public void Pause()
    {
        if (!_pauseMenu.activeInHierarchy && !_optionsMenu.activeInHierarchy && !_commandesMenu.activeInHierarchy)
        {
            _pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_pauseFirstButton);
            Time.timeScale = 0.0f;
        }
        else if (_pauseMenu.activeInHierarchy || _optionsMenu.activeInHierarchy || _commandesMenu.activeInHierarchy)
        {
            Time.timeScale = 1.0f;
            _pauseMenu.SetActive(false);
            _optionsMenu.SetActive(false);
            _commandesMenu.SetActive(false);
        }
    }

    public void Reprendre()
    {
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        _optionsMenu.SetActive(true);
        _pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_optionsFirstButton);
    }

    public void CloseOptions()
    {
        _optionsMenu?.SetActive(false);
        _pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_optionsClosedButton);
    }

    public void OpenCommandes()
    {
        _commandesMenu.SetActive(true);
        _pauseMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_commandesFirstButton);
    }

    public void CloseCommandes()
    {
        _commandesMenu?.SetActive(false);
        _pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_commandesClosedButton);
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }

    public void Back()
    {
        if (_optionsMenu != null && _optionsMenu.activeInHierarchy)
        {
            CloseOptions();
        }
        else if (_commandesMenu != null && _commandesMenu.activeInHierarchy)
        {
            CloseCommandes();
        }
        else if (_pauseMenu != null && _pauseMenu.activeInHierarchy)
        {
            Reprendre();
        }
    }
}

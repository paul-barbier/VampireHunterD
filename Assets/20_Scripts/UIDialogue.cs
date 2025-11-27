using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class UIDialogue : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu, _optionsMenu;
    [SerializeField] private GameObject _pauseFirstButton, _optionsFirstButton, _optionsClosedButton;

    [SerializeField] private GameObject _dialogueUI;


    public void Start()
    {
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
    }

    public void Pause()
    {
        if (!_pauseMenu.activeInHierarchy && !_optionsMenu.activeInHierarchy)
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_pauseFirstButton);
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

    public void OpenOptions()
    {
        _optionsMenu.SetActive(true);
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

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }

    public void Back()
    {
        Debug.Log("RondAppuyé");
        if (_pauseMenu.activeInHierarchy && !_optionsMenu.activeInHierarchy)
        {
            Reprendre();
        }
        else if(_optionsMenu.activeInHierarchy && _pauseMenu.activeInHierarchy)
        {
            CloseOptions();
        }
    }
}

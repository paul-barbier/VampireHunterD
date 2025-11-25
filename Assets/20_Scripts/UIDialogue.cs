using UnityEngine;
using UnityEngine.SceneManagement;
public class UIDialogue : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _controlesMenu;
    [SerializeField] private GameObject _dialogueUI;

    public void Start()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _controlesMenu.SetActive(false);
    }

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _controlesMenu.SetActive(false);
    }

    public void Reprendre()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _controlesMenu.SetActive(false);
    }

    public void OptionsPauseMenu()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _controlesMenu.SetActive(false);
    }

    public void ControlesPauseMenu()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _controlesMenu.SetActive(true);
    }

    public void BackToOptions()
    {
        if (_controlesMenu != null)
        {
            _pauseMenu.SetActive(false);
            _optionsMenu.SetActive(true);
            _controlesMenu.SetActive(false);
        }
    }

    public void BackToPauseMenu()
    {
        if (_optionsMenu != null)
        {
            _pauseMenu.SetActive(true);
            _optionsMenu.SetActive(false);
            _controlesMenu.SetActive(false);
        }
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }
}

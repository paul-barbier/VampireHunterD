using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu, _optionsMenu, _commandesMenu;
    [SerializeField] private GameObject _menuFirstButton, _optionsFirstButton, _optionsClosedButton, _commandesFirstButton, _commandesClosedButton;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _commandesMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_menuFirstButton);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_optionsClosedButton);
    }

    public void OpenCommandes()
    {
        _commandesMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_commandesFirstButton);
    }

    public void CloseCommandes()
    {
        _commandesMenu?.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_commandesClosedButton);
    }

    public void Jouer()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void BackMenu()
    {
        Debug.Log("Rond APPUY1");
        if (_optionsMenu.activeInHierarchy && _mainMenu.activeInHierarchy)
        {
            CloseOptions();
        }
        else if (_commandesMenu.activeInHierarchy && _mainMenu.activeInHierarchy)
        {
            CloseCommandes();
        }
    }
}

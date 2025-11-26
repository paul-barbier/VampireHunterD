using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu, _optionsMenu;
    [SerializeField] private GameObject _menuFirstButton, _optionsFirstButton, _optionsClosedButton;

    public void Start()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
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
            Debug.Log("Rond Détecté");
            CloseOptions();
        }
    }
}

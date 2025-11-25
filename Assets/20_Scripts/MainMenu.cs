using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanva;
    [SerializeField] private GameObject _optionsCanva;
    [SerializeField] private GameObject _controlesCanva;

    private void Start()
    {
        _mainMenuCanva.SetActive(true);
        _optionsCanva.SetActive(false);
        _controlesCanva.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Options()
    {
        _mainMenuCanva.SetActive(false);
        _optionsCanva.SetActive(true);
        _controlesCanva.SetActive(false);
    }

    public void QuitOptions()
    {
        _mainMenuCanva.SetActive(true);
        _optionsCanva.SetActive(false);
        _controlesCanva.SetActive(false);
    }

    public void Controles()
    {
        _mainMenuCanva.SetActive(false);
        _optionsCanva.SetActive(false);
        _controlesCanva.SetActive(true);
    }

    public void QuitControles()
    {
        _mainMenuCanva.SetActive(false);
        _optionsCanva.SetActive(true);
        _controlesCanva.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}

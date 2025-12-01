using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string _wantedScene;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter changement de scène ");
        SceneManager.LoadScene(_wantedScene);
    }
}

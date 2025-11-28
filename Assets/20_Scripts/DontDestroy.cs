using UnityEngine;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;

    [SerializeField] private Slider _uiSlider;

    private void Awake()
    {
        // Singleton simple
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }
}

using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;

    public float sliderValue = 100f;

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
}

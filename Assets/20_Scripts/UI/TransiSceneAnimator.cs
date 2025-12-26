using UnityEngine;
using UnityEngine.Events;

public class TransiSceneAnimator : MonoBehaviour
{
    public UnityEvent OnFadeBlackSwitch;
    public UnityEvent DestroyCinematiqueTrigger;
    public static TransiSceneAnimator Instance;

    [SerializeField] private Animator AnimChangementScene;
    [SerializeField] private GameObject GOChangementScene;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        AnimChangementScene.GetComponent<Animator>();
        GOChangementScene.GetComponent<GameObject>();

    }

    private void Start()
    {
        GOChangementScene.SetActive(false);
    }

    public void ChangeScene()
    {
        if (GOChangementScene != null)
        {
            GOChangementScene.SetActive(true);
        }

        if (AnimChangementScene != null)
        {
            AnimChangementScene.SetBool("AnimChangScene", true);
        }
    }

    public void LoadingScene()
    {
        OnFadeBlackSwitch.Invoke();
    }

    public void DestroyTrigger()
    {
        DestroyCinematiqueTrigger.Invoke();
    }

    private void Desactive()
    {
        GOChangementScene.SetActive(false);
        AnimChangementScene.SetBool("AnimChangScene", false);
        AnimChangementScene.Rebind();

    }
}

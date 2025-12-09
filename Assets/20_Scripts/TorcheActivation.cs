using UnityEngine;

public class TorcheActivation : MonoBehaviour
{
    [SerializeField] GameObject Torche;

    private void Start()
    {
        Torche.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Torche.SetActive(true);
    }
}

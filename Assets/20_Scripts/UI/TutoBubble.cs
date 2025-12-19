using UnityEngine;

public class TutoBubble : MonoBehaviour
{
    [SerializeField] GameObject TutoApparaitre;

    private void Awake()
    {
        TutoApparaitre.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TutoApparaitre.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TutoApparaitre.SetActive(false);
    }
}

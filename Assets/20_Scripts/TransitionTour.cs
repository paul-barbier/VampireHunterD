using UnityEngine;

public class TransitionTour : MonoBehaviour
{
    [SerializeField] private GameObject _plafond;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(_plafond);
    }
}

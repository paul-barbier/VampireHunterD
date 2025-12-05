using UnityEngine;

public class TransitionTour : MonoBehaviour
{
    [SerializeField] private GameObject _plafond;
    [SerializeField] private GameObject _murQuiBloque;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(_plafond);
        Destroy(_murQuiBloque);
    }
}

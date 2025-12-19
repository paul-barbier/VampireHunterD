using UnityEngine;

public class TransitionTour : MonoBehaviour
{
    [SerializeField] private GameObject _plafond;
    [SerializeField] private GameObject _murQuiBloque;
    [SerializeField] private GameObject _GradientCatac;
    [SerializeField] private GameObject _GradientBlack;
    [SerializeField] private GameObject _GradientCatacombes;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(_plafond);
        Destroy(_murQuiBloque);
        Destroy(_GradientCatac);
        Destroy(_GradientBlack);
        Destroy(_GradientCatacombes);
        Object.FindFirstObjectByType<CaveDarkness3D>().RestoreLight();
    }
}

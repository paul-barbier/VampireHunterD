using System.Collections;
using UnityEngine;

public class TransitionTour : MonoBehaviour
{
    [SerializeField] private GameObject _plafond;
    [SerializeField] private GameObject _murQuiBloque;
    [SerializeField] private GameObject _GradientCatac;
    [SerializeField] private GameObject _GradientBlack;
    [SerializeField] private GameObject _GradientCatacombes;
    [SerializeField] private Animator _plafondAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _plafondAnim.SetBool("Cassable", true);

        StartCoroutine(PlafondCasse());
        Destroy(_murQuiBloque);
        Destroy(_GradientCatac);
        Destroy(_GradientBlack);
        Destroy(_GradientCatacombes);
        Object.FindFirstObjectByType<CaveDarkness3D>().RestoreLight();
    }

    private IEnumerator PlafondCasse()
    {
        yield return new WaitForSeconds(4);
        Destroy(_plafond);
    }
}

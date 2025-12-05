using System.Collections;
using UnityEngine;

public class OneWay : MonoBehaviour
{
    [Tooltip("Collider2D du cube à désactiver pour laisser passer (le cube au-dessus).")]
    public Collider2D targetCollider;

    [Tooltip("Tag du joueur (ou de l'entité autorisée à traverser).")]
    public string playerTag = "Player";

    [Tooltip("Si vrai, on n'autorise le passage que si l'entrée se fait depuis le dessous.")]
    public bool requireFromBelow = true;

    [Tooltip("Délai (s) avant de réactiver le collider après la sortie.")]
    public float reEnableDelay = 0.15f;

    // Si tu préfères désactiver l'objet entier, active et assigne cet option au lieu de targetCollider.
    public bool disableGameObjectInstead = false;

    Coroutine reenableCoroutine;

    void Reset()
    {
        // Tentative de découverte automatique si non assigné
        if (targetCollider == null)
        {
            var parent = transform.parent;
            if (parent != null)
            {
                foreach (var c in parent.GetComponentsInChildren<Collider2D>())
                {
                    if (c != GetComponent<Collider2D>())
                    {
                        targetCollider = c;
                        break;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (requireFromBelow)
        {
            // Autorise passage uniquement si le centre du joueur est en dessous du centre du trigger
            if (!(other.transform.position.y < transform.position.y)) return;
        }

        SetTargetEnabled(false);
        if (reenableCoroutine != null) StopCoroutine(reenableCoroutine);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (reenableCoroutine != null) StopCoroutine(reenableCoroutine);
        reenableCoroutine = StartCoroutine(ReEnableAfterDelay(reEnableDelay));
    }

    IEnumerator ReEnableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetTargetEnabled(true);
        reenableCoroutine = null;
    }

    void SetTargetEnabled(bool enabled)
    {
        if (targetCollider == null)
        {
            Debug.LogWarning($"OneWay: targetCollider non assigné sur {name}");
            return;
        }

        if (disableGameObjectInstead)
            targetCollider.gameObject.SetActive(enabled);
        else
            targetCollider.enabled = enabled;
    }
}

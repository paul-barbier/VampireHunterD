using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;
    private int layerPremier = -1;
    private int layerSecond = -1;
    private Collider2D[] playerColliders;
    [SerializeField] private UnityEvent PlaySound;

    private void Start()
    {
        layerPremier = LayerMask.NameToLayer("PremierPlan");
        layerSecond = LayerMask.NameToLayer("SecondPlan");

        if (layerPremier == -1 || layerSecond == -1)
        {
            Debug.LogWarning("Layer 'PremierPlan' ou 'SecondPlan' introuvable dans les settings Unity.");
        }

        // Récupère tous les Collider2D présents sur le joueur (inclut les enfants)
        playerColliders = GetComponentsInChildren<Collider2D>();
        if (playerColliders == null || playerColliders.Length == 0)
        {
            Debug.LogWarning("Aucun Collider2D trouvé sur le joueur. Physics2D.IgnoreCollision ne pourra pas être appliqué.");
        }

        // Initialise l'état d'ignorance selon le layer initial du joueur
        if (layerPremier != -1 && layerSecond != -1)
        {
            if (gameObject.layer == layerPremier)
            {
                // Si joueur au PremierPlan, ignorer les collisions entre les deux plans si c'est ce que tu veux
                SetIgnoreBetweenPlans(true);
            }
            else if (gameObject.layer == layerSecond)
            {
                // Si joueur au SecondPlan, ignorer les collisions entre les deux plans
                SetIgnoreBetweenPlans(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }

    // Met à jour la matrice de collision globale entre PremierPlan et SecondPlan.
    private void SetIgnoreBetweenPlans(bool ignore)
    {
        if (layerPremier == -1 || layerSecond == -1)
            return;

        Physics2D.IgnoreLayerCollision(layerPremier, layerSecond, ignore);
    }

    public void UseTP()
    {
        if (currentTeleporter != null)
        {
            transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            PlaySound.Invoke();

            if (gameObject.layer == layerPremier)
            {
                SetLayerRecursively(gameObject, layerSecond);
                SetIgnoreBetweenPlans(true);
            }
            else if (gameObject.layer == layerSecond)
            {
                SetLayerRecursively(gameObject, layerPremier);
                SetIgnoreBetweenPlans(true);
            }
        }
    }
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
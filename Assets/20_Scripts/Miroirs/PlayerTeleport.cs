using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;
    private int layerPremier = -1;
    private int layerSecond = -1;
    private Collider2D[] playerColliders;

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
            //AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.TP].Sounds[0], transform.position, 1f);

            int currentLayer = gameObject.layer;
            if (currentLayer == layerPremier)
            {
                // Passe en SecondPlan : change le layer du joueur et ignore globalement la paire de layers
                gameObject.layer = layerSecond;
                SetIgnoreBetweenPlans(true);
            }
            else if (currentLayer == layerSecond)
            {
                // Revenir en PremierPlan : change le layer du joueur et restaure les collisions entre les plans
                gameObject.layer = layerPremier;
                SetIgnoreBetweenPlans(true);
            }
            else
            {
                Debug.Log("Le joueur n'est ni sur 'PremierPlan' ni sur 'SecondPlan' — aucun basculement effectué.");
            }
        }
    }
}
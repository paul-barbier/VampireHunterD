using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OneWay : MonoBehaviour
{
    [Tooltip("Arc de surface (en degrés) que la plateforme considère comme 'au-dessus'")]
    [SerializeField] private float surfaceArc = 180f;

    [Tooltip("Active le comportement one-way (collision seulement depuis le dessus)")]
    [SerializeField] private bool useOneWay = true;

    private void Awake()
    {
        Setup();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Setup();
    }
#endif

    private void Setup()
    {
        // Récupère ou ajoute un collider 2D
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            // Si aucune collision, on ajoute un BoxCollider2D par défaut
            col = gameObject.AddComponent<BoxCollider2D>();
        }

        // Active "Used By Effector" si possible (nécessaire pour PlatformEffector2D)
        if (col is BoxCollider2D box)
        {
            box.usedByEffector = true;
        }
        else if (col is PolygonCollider2D poly)
        {
            poly.usedByEffector = true;
        }
        else if (col is CapsuleCollider2D cap)
        {
            cap.usedByEffector = true;
        }
        // else : d'autres colliders peuvent exister (TilemapCollider2D nécessite configuration dans l'éditeur)

        // Récupère ou ajoute PlatformEffector2D
        PlatformEffector2D eff = GetComponent<PlatformEffector2D>();
        if (eff == null)
            eff = gameObject.AddComponent<PlatformEffector2D>();

        eff.surfaceArc = surfaceArc;
        eff.useOneWay = useOneWay;
        // Optionnel : empêcher frottement/collision latérale si besoin
        eff.useSideFriction = false;
        eff.useSideBounce = false;
        // Groupe one-way (utile pour groupes de plateformes)
        eff.useOneWayGrouping = true;
    }
}

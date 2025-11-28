using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotativePlat : MonoBehaviour
{
    [Header("Circle (2D - X/Y)")]
    [Tooltip("Si renseigné, le centre suit ce Transform. Sinon centre initial fixe.")]
    public Transform centerTransform;
    [Tooltip("Rayon du cercle (si <= 0, on utilise la distance initiale au centre).")]
    public float radius = 1f;
    [Tooltip("Vitesse angulaire en degrés par seconde.")]
    public float angularSpeedDeg = 90f;
    [Tooltip("Inverse le sens (horaire/anti-horaire).")]
    public bool clockwise = false;
    [Tooltip("Oriente la plateforme suivant la tangente (rotation autour de Z).")]
    public bool alignToTangent = false;

    [Header("Player parenting (2D)")]
    [Tooltip("Tag utilisé pour reconnaître le joueur.")]
    public string playerTag = "Player";
    [Tooltip("Si true, la plateforme fera SetParent(player, this) quand le joueur est dessus.")]
    public bool useParenting = true;

    // interne
    private Vector2 center;
    private float angleRad;
    private float effectiveRadius;
    private float originalZ;

    // stockage des parents originaux pour restauration
    private readonly Dictionary<Transform, Transform> originalParents = new Dictionary<Transform, Transform>();

    private void Start()
    {
        // centre initial (X,Y) et sauvegarde Z pour ne pas le modifier
        Vector3 c3 = centerTransform != null ? centerTransform.position : transform.position;
        center = new Vector2(c3.x, c3.y);
        originalZ = transform.position.z;

        // calcul de l'angle et du rayon initial à partir de la position actuelle
        Vector2 toPos = new Vector2(transform.position.x - center.x, transform.position.y - center.y);
        if (toPos.sqrMagnitude < 1e-6f)
        {
            angleRad = 0f;
            effectiveRadius = radius > 0f ? radius : 1f;
            // positionne sur le cercle pour éviter téléport brutal si radius > 0
            transform.position = new Vector3(center.x + effectiveRadius, center.y, originalZ);
        }
        else
        {
            angleRad = Mathf.Atan2(toPos.y, toPos.x);
            effectiveRadius = radius > 0f ? radius : toPos.magnitude;
            if (radius > 0f)
            {
                float c = Mathf.Cos(angleRad);
                float s = Mathf.Sin(angleRad);
                transform.position = new Vector3(center.x + c * effectiveRadius, center.y + s * effectiveRadius, originalZ);
            }
        }
    }

    private void Update()
    {
        // si le centre doit suivre un Transform, on met à jour center chaque frame (utile si center bouge)
        if (centerTransform != null)
        {
            Vector3 c3 = centerTransform.position;
            center = new Vector2(c3.x, c3.y);
        }

        float dir = clockwise ? -1f : 1f;
        angleRad += dir * Mathf.Deg2Rad * angularSpeedDeg * Time.deltaTime;
        angleRad = Mathf.Repeat(angleRad, Mathf.PI * 2f);

        float c = Mathf.Cos(angleRad);
        float s = Mathf.Sin(angleRad);
        Vector3 newPos = new Vector3(center.x + c * effectiveRadius, center.y + s * effectiveRadius, originalZ);
        transform.position = newPos;

        if (alignToTangent)
        {
            // tangente 2D = (-sin, cos) ; applique le sens du mouvement
            Vector2 tangent = new Vector2(-s, c) * dir;
            if (tangent.sqrMagnitude > 1e-6f)
            {
                float angleDeg = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
            }
        }
    }

    // ---------- Parenting / tag handling ----------

    private void TryParent(Transform other)
    {
        if (!useParenting) return;
        Transform player = FindTaggedAncestor(other);
        if (player == null) return;
        if (originalParents.ContainsKey(player)) return; // déjà parenté

        // sauvegarde parent original puis affecte la plateforme comme parent
        originalParents[player] = player.parent;
        player.SetParent(transform, true); // conserve la position mondiale
    }

    private void TryUnparent(Transform other)
    {
        if (!useParenting) return;
        Transform player = FindTaggedAncestor(other);
        if (player == null) return;
        if (!originalParents.ContainsKey(player)) return;

        // restaure le parent original (ou null)
        Transform orig = originalParents[player];
        player.SetParent(orig, true);
        originalParents.Remove(player);
    }

    // remonte l'arbre pour trouver un transform avec le bon tag
    private Transform FindTaggedAncestor(Transform t)
    {
        if (t == null) return null;
        if (string.IsNullOrEmpty(playerTag)) return t;

        Transform cur = t;
        while (cur != null)
        {
            if (cur.CompareTag(playerTag)) return cur;
            cur = cur.parent;
        }
        return null;
    }

    // 2D collisions / triggers
    private void OnCollisionEnter2D(Collision2D collision) => TryParent(collision.transform);
    private void OnCollisionExit2D(Collision2D collision) => TryUnparent(collision.transform);
    private void OnTriggerEnter2D(Collider2D other) => TryParent(other.transform);
    private void OnTriggerExit2D(Collider2D other) => TryUnparent(other.transform);
}

using System.Collections;
using UnityEngine;

public class PlatformeOneWay : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    private Collider2D currentPlatformCollider;
    private Coroutine ignoreCoroutine;

    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private float ignoreDuration = 0.6f; // durée d'ignorance des collisions
    [SerializeField] private bool autoIgnoreOnJump = true; // allow passing up when jumping from below
    [SerializeField] private bool enableDebug = true;

    private Rigidbody2D playerRigidbody;

    // Small search sizes
    private const float aboveSearchHeight = 15f;
    private const float belowSearchHeight = 0.25f;
    private const float horizontalPadding = 0.95f;

    private void Awake()
    {
        if (playerCollider != null)
            playerRigidbody = playerCollider.attachedRigidbody;
    }

    private void Update()
    {
        // traverser vers le bas
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownOneWay();
        }

        // Détection automatique : si on saute vers le haut depuis en-dessous, autoriser le passage
        if (autoIgnoreOnJump && playerRigidbody != null)
        {
            if (playerRigidbody.linearVelocity.y > 0.1f)
            {
                Collider2D platformAbove = FindPlatformAbove();
                if (platformAbove != null)
                {
                    if (enableDebug) Debug.Log("[PlatformeOneWay] Auto ignore collision (jump up) -> " + platformAbove.name);
                    StartIgnoreCoroutine(platformAbove, ignoreDuration);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
            currentPlatformCollider = collision.collider;
            if (enableDebug) Debug.Log("[PlatformeOneWay] Enter OneWay platform: " + currentOneWayPlatform.name);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            if (collision.gameObject == currentOneWayPlatform)
            {
                if (enableDebug) Debug.Log("[PlatformeOneWay] Exit OneWay platform: " + currentOneWayPlatform.name);
                currentOneWayPlatform = null;
                currentPlatformCollider = null;
            }
        }
    }

    // coroutine qui capture le collider ciblé localement pour pouvoir rétablir la collision même si currentPlatformCollider change
    private IEnumerator DisableCollisionCoroutine(Collider2D platformCollider, float duration)
    {
        if (platformCollider == null || playerCollider == null)
            yield break;

        Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        if (enableDebug) Debug.Log($"[PlatformeOneWay] Ignoring collision with {platformCollider.name} for {duration}s");
        yield return new WaitForSeconds(duration);

        // tenter de restaurer la collision, même si currentPlatformCollider a été nulled
        if (playerCollider != null && platformCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
            if (enableDebug) Debug.Log($"[PlatformeOneWay] Restored collision with {platformCollider.name}");
        }
        ignoreCoroutine = null;
    }

    // Wrapper qui accepte un collider cible
    private void StartIgnoreCoroutine(Collider2D platformCollider, float duration)
    {
        if (platformCollider == null)
            return;

        if (ignoreCoroutine != null)
            return;

        ignoreCoroutine = StartCoroutine(DisableCollisionCoroutine(platformCollider, duration));
    }

    public void DownOneWay()
    {
        Collider2D platformBelow = FindPlatformBelow();
        if (platformBelow == null)
        {
            if (enableDebug) Debug.Log("[PlatformeOneWay] Aucune plateforme OneWay détectée sous le joueur");
            return;
        }

        // s'assurer que le joueur est bien au-dessus de la plateforme avant de le faire traverser
        float playerBottom = playerCollider.bounds.min.y;
        float platformTop = platformBelow.bounds.max.y;
        if (playerBottom <= platformTop + 0.02f)
        {
            if (enableDebug) Debug.Log("[PlatformeOneWay] DownOneWay : start ignore for " + platformBelow.name);
            StartIgnoreCoroutine(platformBelow, ignoreDuration);
        }
        else
        {
            if (enableDebug) Debug.Log("[PlatformeOneWay] Le joueur n'est pas au-dessus de la plateforme — ignoré");
        }
    }

    // Cherche une plateforme juste au-dessus du joueur (détecte même si pas encore en collision)
    private Collider2D FindPlatformAbove()
    {
        if (playerCollider == null) return null;

        Vector2 playerBoundsCenter = playerCollider.bounds.center;
        float halfWidth = (playerCollider.bounds.size.x * horizontalPadding) / 2f;
        Vector2 boxCenter = new Vector2(playerBoundsCenter.x, playerCollider.bounds.max.y + aboveSearchHeight / 2f);
        Vector2 boxSize = new Vector2(halfWidth * 2f, aboveSearchHeight);

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);
        foreach (var c in hits)
        {
            if (c != null && c.CompareTag("OneWayPlatform"))
            {
                // ensure platform is actually above player's top
                if (c.bounds.min.y >= playerCollider.bounds.max.y - 0.01f)
                    return c;
            }
        }
        return null;
    }

    // Cherche une plateforme directement sous les pieds du joueur (pour drop)
    private Collider2D FindPlatformBelow()
    {
        if (playerCollider == null) return null;

        Vector2 playerBoundsCenter = playerCollider.bounds.center;
        float halfWidth = (playerCollider.bounds.size.x * horizontalPadding) / 2f;
        Vector2 boxCenter = new Vector2(playerBoundsCenter.x, playerCollider.bounds.min.y - belowSearchHeight / 2f);
        Vector2 boxSize = new Vector2(halfWidth * 2f, belowSearchHeight);

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f);
        foreach (var c in hits)
        {
            if (c != null && c.CompareTag("OneWayPlatform"))
            {
                // ensure platform's top is close beneath player's bottom
                if (c.bounds.max.y <= playerCollider.bounds.min.y + 0.05f)
                    return c;
            }
        }

        // fallback : use currentPlatformCollider if still colliding
        if (currentPlatformCollider != null)
            return currentPlatformCollider;

        return null;
    }

    // Visual debug dans la scène pour voir la plateforme courante et les zones de recherche
    private void OnDrawGizmos()
    {
        if (!enableDebug || playerCollider == null) return;

        Gizmos.color = Color.yellow;
        Bounds pb = playerCollider.bounds;
        Gizmos.DrawWireCube(pb.center, pb.size);

        // above box
        Vector2 playerBoundsCenter = playerCollider.bounds.center;
        Vector2 aboveCenter = new Vector2(playerBoundsCenter.x, playerCollider.bounds.max.y + aboveSearchHeight / 2f);
        Vector2 aboveSize = new Vector2(playerCollider.bounds.size.x * horizontalPadding, aboveSearchHeight);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(aboveCenter, aboveSize);

        // below box
        Vector2 belowCenter = new Vector2(playerBoundsCenter.x, playerCollider.bounds.min.y - belowSearchHeight / 2f);
        Vector2 belowSize = new Vector2(playerCollider.bounds.size.x * horizontalPadding, belowSearchHeight);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(belowCenter, belowSize);
    }
}


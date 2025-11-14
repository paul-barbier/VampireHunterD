using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeOneWay : MonoBehaviour
{
    private GameObject currentOnewayPlatform;

    [SerializeField] private BoxCollider2D playerCollider;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentOnewayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOnewayPlatform = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            if (collision.gameObject == currentOnewayPlatform)
            {
                currentOnewayPlatform = null;
            }
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOnewayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}


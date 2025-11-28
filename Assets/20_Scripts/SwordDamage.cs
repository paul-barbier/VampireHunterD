using System.Collections;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    private float respawnTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MouvementScript enemy = collision.GetComponentInParent<MouvementScript>();
        if (enemy != null)
        {
            Debug.Log("Ennemi touché et détruit !");
            Destroy(enemy.gameObject);
        }
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnTime);
    }
}

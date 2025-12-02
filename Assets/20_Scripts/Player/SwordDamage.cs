using System.Collections;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] private float respawnTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MouvementScript enemy = collision.GetComponentInParent<MouvementScript>();
        if (enemy != null && collision.CompareTag("Dash") && CompareTag("Sword"))
        {
            SoundManager.PlaySound(SoundType.VampireDeath, 10f);
            enemy.gameObject.SetActive(false);
            StartCoroutine(RespawnEnemy());
        }
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnTime);
    }
}

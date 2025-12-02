using System.Collections;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] private float respawnTime;
    [SerializeField] private PlayerCharacter _character;

    private void Start()
    {
        _character = GetComponent<PlayerCharacter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MouvementScript enemy = collision.GetComponentInParent<MouvementScript>();
        if (enemy != null && collision.CompareTag("Dash") && CompareTag("Sword"))
        {
<<<<<<< Updated upstream
=======
            SoundManager.PlaySound(SoundType.VampireDeath, 10f);
>>>>>>> Stashed changes
            enemy.gameObject.SetActive(false);
            StartCoroutine(RespawnEnemy());
        }
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnTime);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] private float respawnTime;
    [SerializeField] private PlayerCharacter _character;

    [SerializeField] private UnityEvent PlaySound;

    private void Start()
    {
        _character = GetComponent<PlayerCharacter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MouvementScript enemy = collision.GetComponentInParent<MouvementScript>();
        if (enemy != null && collision.CompareTag("Dash") && CompareTag("Sword"))
        {
            PlaySound.Invoke();
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

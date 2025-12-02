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
        if (enemy != null && collision.CompareTag("Dash") && CompareTag("Sword") && CompareTag("VampireEnnemy"))
        {
            enemy.gameObject.SetActive(false);
           // AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.VampireDeath].Sounds[0], transform.position, 1f);
            StartCoroutine(RespawnEnemy());
        }
        else if (enemy != null && collision.CompareTag("Attack") && CompareTag("Sword") && CompareTag("ChauveSouris"))
        {
            enemy.gameObject.SetActive(false);
            SoundManager.PlaySound(SoundType.BatExplosion, 1f);
            //AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.BatExplosion].Sounds[0], transform.position, 1f);
            StartCoroutine(RespawnEnemy());
        }
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(respawnTime);
    }
}

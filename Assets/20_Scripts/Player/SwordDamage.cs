using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwordDamage : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private float respawnTime;
=======
>>>>>>> 447e2ef66087783545e77ec72829650d69551556
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
            enemy.gameObject.SetActive(false);
<<<<<<< HEAD
            PlaySound.Invoke();
            StartCoroutine(RespawnEnemy());
=======
>>>>>>> 447e2ef66087783545e77ec72829650d69551556
        }
    }
}

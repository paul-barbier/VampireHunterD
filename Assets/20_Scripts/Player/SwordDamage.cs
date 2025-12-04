using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _character;

    [SerializeField] private UnityEvent PlaySound;

    private void Start()
    {
        _character = GetComponent<PlayerCharacter>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Dash") && CompareTag("Sword"))
        {
            PlaySound.Invoke();
            //SoundManager.PlaySound(SoundType.VampireDeath, 10f);
            _character.KillingEnemy(collision);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SwordDamage : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _character;

    [SerializeField] private UnityEvent PlaySound;

    private void Start()
    {
        _character = GetComponentInParent<PlayerCharacter>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Dash") && CompareTag("Sword"))
        {
            _character.KillingEnemy(collision);
            Debug.Log("Killing :" + collision.transform.parent.name);
            PlaySound.Invoke();
            //SoundManager.PlaySound(SoundType.VampireDeath, 10f);
        }
    }
}

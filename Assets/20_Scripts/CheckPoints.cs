using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] private Health Health;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();

            playerHealth.GetHeal(playerHealth.GetMaxHealth());

            PlayerCharacter character = collision.GetComponent<PlayerCharacter>();

            character.checkpoint = this;
        }
    }
}

using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private Health Health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter controller = collision.GetComponent<PlayerCharacter>();
        if (controller != null)
        {
            Health._currentHealth = Health._maxHealth;
            controller.checkpoint = this;
        }
    }
}

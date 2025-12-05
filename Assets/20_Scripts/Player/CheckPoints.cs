using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();

            playerHealth.GetHeal(playerHealth.GetMaxHealth());

            playerHealth.checkpoint = this;
        }
    }
}

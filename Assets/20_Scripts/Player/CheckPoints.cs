using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            animator.SetBool("PlayAnime", true);
            Health playerHealth = collision.GetComponent<Health>();

            playerHealth.GetHeal(playerHealth.GetMaxHealth());

            playerHealth.checkpoint = this;
        }
    }
}

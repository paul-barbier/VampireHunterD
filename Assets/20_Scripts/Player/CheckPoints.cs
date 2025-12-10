using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Transform respawnPoint;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("PlayAnime", true);

            Health playerHealth = collision.GetComponent<Health>();

            playerHealth.GetHeal(playerHealth.GetMaxHealth());

            playerHealth.checkpoint = respawnPoint.transform.position;
        }
    }
}

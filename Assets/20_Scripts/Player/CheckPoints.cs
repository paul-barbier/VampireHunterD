using UnityEngine;
using UnityEngine.Events;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Transform respawnPoint;
    [SerializeField] GameObject checkpointEffect;
    [SerializeField] Transform effectSpawnPoint;
    //[SerializeField] UnityEvent onCheckpointActivated;
    private bool alreadyplayed = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(checkpointEffect, effectSpawnPoint.position, Quaternion.identity);
            if (!alreadyplayed)
            {
                AudioSource.PlayClipAtPoint(SoundManager.instance.soundList[(int)SoundType.Checkpoints].Sounds[0], transform.position, 1f);
                alreadyplayed = true;
            }
            animator.SetBool("PlayAnime", true);

            Health playerHealth = collision.GetComponent<Health>();

            playerHealth.GetHeal(playerHealth.GetMaxHealth());

            playerHealth.checkpoint = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
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

using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ennemy"))
        {
            Debug.Log("EnnemyDétruit");
            collision.gameObject.SetActive(false);
        }
    }
}

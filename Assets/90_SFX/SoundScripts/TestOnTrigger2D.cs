using UnityEngine;

public class TestOnTrigger2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Trigger fonctionne");
        }
    }
}

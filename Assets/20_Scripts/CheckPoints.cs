using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter controller = collision.GetComponent<PlayerCharacter>();
        if (controller != null)
        {
            controller.checkpoint = this;
        }
    }
}

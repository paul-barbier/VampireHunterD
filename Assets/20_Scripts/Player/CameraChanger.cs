using UnityEngine;
using Unity.Cinemachine;

public class CameraChanger : MonoBehaviour
{
    PlayerCharacter _character;
    [SerializeField] private float Weight;
    [SerializeField] private float Radius;
    [SerializeField] private int direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacter>() != null)
        {
            collision.GetComponent<CameraFollow>().ChangeLook(direction);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacter>() != null)
        {
            collision.GetComponent<CameraFollow>().ChangeLook(0);
        }
    }
}

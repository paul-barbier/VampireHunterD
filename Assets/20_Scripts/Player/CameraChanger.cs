using UnityEngine;
using Unity.Cinemachine;

public class CameraChanger : MonoBehaviour
{
    PlayerCharacter _character;
    [SerializeField] private float Weight;
    [SerializeField] private float Radius;
    [SerializeField] private Transform Target;

    [SerializeField] private int direction;

    [Header("Cam")]
    public CinemachineCamera _cineCamera;

    [Header("Targets")]
    public CinemachineTargetGroup defaultTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacter>() != null)
        {
            //if (_cineCamera != null && defaultTarget != null)
            //{
            //    defaultTarget.AddMember(Target, Weight, Radius);
            //}
            collision.GetComponent<CameraFollow>().ChangeLook(direction);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacter>() != null)
        {
            //if (_cineCamera != null && defaultTarget != null)
            //{
            //    defaultTarget.RemoveMember(Target);
            //}

            collision.GetComponent<CameraFollow>().ChangeLook(0);
        }
    }
}

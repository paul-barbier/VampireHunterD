using UnityEngine;
using Unity.Cinemachine;
using System;
 
public enum CameraDirection {LEGACY = 0, DEFAULT, HAUT, GAUCHE, BAS, DROITE}

public class CameraChanger : MonoBehaviour
{
    PlayerCharacter _character;
    [SerializeField] private float Weight;
    [SerializeField] private float Radius;
    [HideInInspector][SerializeField] private int direction;
    [SerializeField] private CameraDirection cameraDirection = CameraDirection.LEGACY;

    private void Awake()
    {
        if (cameraDirection == CameraDirection.LEGACY)
        {
            if(direction == 1)
            {
                cameraDirection = CameraDirection.HAUT;
            }
            else if (direction == -1)
            {
                cameraDirection = CameraDirection.BAS;
            }
            else
            {
                cameraDirection = CameraDirection.DEFAULT;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacter>() != null)
        {
            collision.GetComponent<CameraFollow>().ChangeLook(cameraDirection);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacter>() != null)
        {
            collision.GetComponent<CameraFollow>().ChangeLook(CameraDirection.DEFAULT);
        }
    }
}

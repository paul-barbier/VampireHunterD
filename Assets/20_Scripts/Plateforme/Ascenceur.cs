using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascenceur : MonoBehaviour
{

    PlayerCharacter character;

    public Transform posA, posB;
    public float speed;
    private Vector3 targetPos;
    private bool disableMovement = false;
    [SerializeField] private float WaitingExitTime;

    private void Start()
    {
        transform.position = posA.position;
    }

    private void Update()
    {
        if (disableMovement)
        {
            character.DisableAllMovement();
        }
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("PlayerSurAscenceur");
        collision.transform.SetParent(transform);
        targetPos = posB.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        disableMovement = true;
    }
}

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
    [SerializeField] private Collider2D ascenceurCollider;
    [SerializeField] private GameObject ColliderMur;
    private bool onPlateforme = false;

    private void Start()
    {
        character = FindAnyObjectByType<PlayerCharacter>();
        transform.position = posA.position;
        ColliderMur.SetActive(false);
    }

    private void Update()
    {
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        collision.GetComponent<PlayerCharacter>();
        Debug.Log("PlayerSurAscenceur");
        collision.transform.SetParent(transform);
        targetPos = posB.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        ColliderMur.SetActive(true);
        if (Vector2.Distance(transform.position, targetPos) < 1f)
        {
            Debug.Log("Ascenceur arrivé");
            ascenceurCollider.enabled = false;
            ColliderMur.SetActive(false);
        }
    }
}

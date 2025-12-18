using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascenceur : MonoBehaviour
{

    [SerializeField] private PlayerCharacter character;

    public Transform posA, posB;
    public float speed;
    private Vector3 targetPos;

    [SerializeField] private float WaitingExitTime;
    [SerializeField] private Collider2D ascenceurCollider;
    [SerializeField] private GameObject ColliderMur;
    [SerializeField] private Animator AnimAscenceur;

    private void Start()
    {
        AnimAscenceur = GetComponent<Animator>();
        character = FindAnyObjectByType<PlayerCharacter>();
        transform.position = posA.position;
        ColliderMur.SetActive(false);
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        character = collision.GetComponent<PlayerCharacter>();
        character._movementDisabled = true;
        Debug.Log("PlayerSurAscenceur");
        collision.transform.SetParent(transform);
        targetPos = posB.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        ColliderMur.SetActive(true);
        AnimAscenceur.SetBool("OuvreAscenceur", false);
        if (Vector2.Distance(transform.position, targetPos) < 1f)
        {
            AnimAscenceur.SetBool("OuvreAscenceur", true);
            Debug.Log("Ascenceur arrivé");
            character._movementDisabled = false;
            ascenceurCollider.enabled = false;
            ColliderMur.SetActive(false);
        }
    }
}

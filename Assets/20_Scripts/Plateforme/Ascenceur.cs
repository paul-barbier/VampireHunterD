using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascenceur : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    private Vector3 targetPos;
    [SerializeField] private float WaitingExitTime;

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("PlayerSurAscenceur");
        collision.transform.SetParent(transform);
        if (Vector2.Distance(transform.position, posA.position) < 0.1f)
        {
            targetPos = posB.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("PlayerPasSurAscenceur");
        StartCoroutine(ElevatorWaiting());
        targetPos = posA.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private IEnumerator ElevatorWaiting()
    {
        yield return new WaitForSeconds(WaitingExitTime);
    }
}

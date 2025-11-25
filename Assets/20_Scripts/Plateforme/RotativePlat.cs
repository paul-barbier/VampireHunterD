using UnityEngine;

public class RotativePlat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform posA, posB;
    public float speed;
    private Vector3 Rota;

    private void Start()
    {
        Rota = posB.position;
    }

    private void Update()
    {
        
    }
}

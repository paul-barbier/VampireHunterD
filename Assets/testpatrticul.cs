using UnityEngine;

public class testpatrticul : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] ParticleSystem _particul;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Prtipaly();
        }
    }

    public void Prtipaly()
    {
        _particul.Play();
    }
}

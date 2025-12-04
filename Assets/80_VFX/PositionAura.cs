using UnityEngine;

public class PositionAura : MonoBehaviour
{
    [SerializeField] private Transform chauveSouris; // Assigne l'objet ChauveSouris dans l'inspecteur
    [SerializeField] private float oscillationampl;
    [SerializeField] private float frequency;
    void Start()
    {
        if (chauveSouris != null)
        {
            transform.position = chauveSouris.position;
        }
    }

    void Update()
    {
        if (chauveSouris != null)
        {
            transform.position = chauveSouris.position;
            // Ajoute une oscillation verticale
            float newY = chauveSouris.position.y + Mathf.Sin(Time.time * frequency) * oscillationampl;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        }
    }
}
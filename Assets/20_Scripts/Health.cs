using UnityEngine;

public class Health : MonoBehaviour
{
    public int _maxHealth;
    public int _currentHealth;
    [SerializeField] private int _damages;


    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage()
    {
        _currentHealth -= _damages;
    }
}

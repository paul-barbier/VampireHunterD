using UnityEngine;

public class Health : MonoBehaviour
{
    public int _maxHealth;
    public int _currentHealth;
    [SerializeField] private int _damages;

    //Visuel
    [SerializeField] private Animator HpAnime;


    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateBar();
    }

    public void TakeDamage()
    {
        _currentHealth -= _damages;
    }

    void UpdateBar()
    {
        float ratio = (float)_currentHealth / _maxHealth;

        int state = 0;
        if (ratio > 0.75f) state = 0;
        else if (ratio > 0.50f) state = 1;
        else if (ratio > 0.25f) state = 2;
        else state = 3;

        HpAnime.SetInteger("HealthState", state);
    }
}

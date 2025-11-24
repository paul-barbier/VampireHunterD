using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    PlayerCharacter _character;

    //Visuel
    [SerializeField] private Animator HpAnime;


    private void Start()
    {
        _currentHealth = _maxHealth;
        _character = GetComponent<PlayerCharacter>();
    }

    public void TakeDamage(int damages)
    {
        _currentHealth -= damages;
        if (_currentHealth <= 0)
            _character.Die();
        UpdateBar();
    }

    public void UpdateBar()
    {
        float ratio = (float)_currentHealth / _maxHealth;

        int state = 0;
        if (ratio > 0.75f) state = 0;
        else if (ratio > 0.50f) state = 1;
        else if (ratio > 0.25f) state = 2;
        else state = 3;

        HpAnime.SetInteger("HealthState", state);
    }

    public IEnumerator CdDmg()
    {
        yield return new WaitForSeconds(5.0f);
    }

    public void GetHeal(int heal)
    {
        _currentHealth += heal;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        UpdateBar();
    }

    public int GetHealth()
    {
        return _currentHealth;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }
}

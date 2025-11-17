using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerCharacter characterScript;
    [SerializeField] private Collider2D _characterCollider;
    [SerializeField] private Rigidbody2D _characterRigidbody;

    [Serializable]
    private struct AttackValues
    {
        public float _damage;
        public Vector3 _knockbackDirection;
        public float _knockbackForce;
    }

    [SerializeField] private AttackValues _playerValues = new AttackValues();

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == _characterCollider)
        {
            CalculateKnockBackDirection();
        }
    }

    private void CalculateKnockBackDirection()
    {
        _playerValues._knockbackDirection.x = (transform.position.x + _characterCollider.transform.position.x);
        Vector3 targetKnockBack = (_characterCollider.transform.position + _playerValues._knockbackDirection).normalized;
        _characterRigidbody.AddForce(targetKnockBack * _playerValues._knockbackForce, ForceMode2D.Impulse);
        Debug.Log(targetKnockBack.x);
    }
}

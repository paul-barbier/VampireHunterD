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

    private Vector3 targetKnockback = Vector3.zero;

    private void Awake()
    {

    }

    private void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other == _characterCollider)
    //    {
    //        CalculateKnockBackDirection();
    //    }
    //}

    //private void CalculateKnockBackDirection()
    //{
    //    _playerValues._knockbackDirection.x = (_characterCollider.transform.position.x - transform.position.x);
    //    targetKnockback = new Vector3((_playerValues._knockbackDirection.x), 0, 0).normalized;
    //    _useKnockback = true;
    //    _characterRigidbody.AddForce(targetKnockback * _playerValues._knockbackForce, ForceMode2D.Impulse);
    //}
}

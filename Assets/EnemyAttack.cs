using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
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
        Debug.Log(_playerValues._knockbackDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Pas fini
        Debug.Log("PlayerEnter");
        _playerValues._knockbackDirection.x = (transform.position.x - other.transform.position.x);
        Vector3 targetKnockBack = other.transform.position += _playerValues._knockbackDirection;
        other.transform.position = targetKnockBack;
    }
}

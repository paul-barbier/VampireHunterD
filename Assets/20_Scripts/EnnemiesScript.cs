using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

[RequireComponent(typeof(Rigidbody2D))]

public class EnnemiesScript : MonoBehaviour
{
    [Serializable]
    private struct EnemyMovementValues
    {
        public int NextPoint;
        public Transform[] Points;
        public float Speed;
        public float stunTime;
        public float knockForce;
    }

    [Header("Gameplay")]
    [SerializeField] private EnemyMovementValues _airMovement = new EnemyMovementValues();

    [Header("Setup")]
    [SerializeField] private Transform _mesh = null;
    [SerializeField] private float _meshRotationSpeed = 10.0f;

    private Rigidbody2D _rigidbody = null;
    private Vector3 _currentMeshRotation = Vector3.zero;

    private bool _isDowned;

    private Vector2 _sourcePosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _currentMeshRotation = _mesh.eulerAngles;
        _airMovement.NextPoint = 0;
    }

    private void Start()
    {
        _sourcePosition = GameObject.FindGameObjectWithTag("Bomb").transform.position;
    }

    private void FixedUpdate()
    {
        if (!_isDowned)
            EnemyMove();
    }

    private void NextPointTarget()
    {
        _airMovement.NextPoint++;
        if (_airMovement.NextPoint >= _airMovement.Points.Length)
            _airMovement.NextPoint = 0;
    }

    private void EnemyMove()
    {
        Vector2 currentPos = _rigidbody.position;
        Vector2 nextPos = _airMovement.Points[_airMovement.NextPoint].position;

        Vector2 direction = (nextPos - currentPos).normalized;

        _rigidbody.linearVelocity = direction * _airMovement.Speed;

        if (Vector2.Distance(currentPos, nextPos) <= 0.1f)
        {
            NextPointTarget();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bomb")
        {
            _isDowned = true;
            Vector2 bombPos = collision.transform.position;
            Vector2 enemyPos = _rigidbody.position;

            Vector2 directionBomb = (enemyPos - bombPos).normalized;
            _rigidbody.linearVelocity = directionBomb * _airMovement.knockForce;

            StartCoroutine(StunnedEnnemy());
        }
    }

    private IEnumerator StunnedEnnemy()
    {
        yield return new WaitForSeconds(_airMovement.stunTime);
        _isDowned = false;
    }
}
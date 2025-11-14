using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouvementScript : MonoBehaviour
{
    [Serializable]
    private struct MovementValues
    {
        public Transform[] _patrolPoints;
        public int _targetPoints;
        public float _speed;
    }

    [SerializeField] private MovementValues _pointsMovement = new MovementValues();

    private void Start()
    {
        _pointsMovement._targetPoints = 0;
    }

    private void FixedUpdate()
    {
        Debug.Log("Position Ennemi" + transform.position);
        Debug.Log("Position TargetPoint" + _pointsMovement._patrolPoints[_pointsMovement._targetPoints].position);
        if (transform.position.x == _pointsMovement._patrolPoints[_pointsMovement._targetPoints].position.x)
        {
            Debug.Log("TargetIncrease");
            IncreaseTargetInt();
        }
        transform.position = Vector3.MoveTowards(transform.position, _pointsMovement._patrolPoints[_pointsMovement._targetPoints].position, _pointsMovement._speed * Time.fixedDeltaTime);
    }

    void IncreaseTargetInt()
    {
        _pointsMovement._targetPoints++;
        if (_pointsMovement._targetPoints >= _pointsMovement._patrolPoints.Length)
        {
            _pointsMovement._targetPoints = 0;
        }
    }
}
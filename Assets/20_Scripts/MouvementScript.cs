using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouvementScript : MonoBehaviour
{
    public SoundType MobFootsteps;
    [Serializable]
    private struct MovementValues
    {
        public Transform[] _patrolPoints;
        public int _targetPoints;
        public float _speed;
        public float _waitingTime;
    }

    [SerializeField] private SpriteRenderer _ennemySprite;

    [SerializeField] private MovementValues _pointsMovement = new MovementValues();

    private float _reachThreshold = 0.5f;
    private bool _isWaiting = false;

    [SerializeField] private bool _isMoving = false;

    [SerializeField] private Animator _MobAnimation;


    private void Start()
    {
        _pointsMovement._targetPoints = 0;
    }


    private void FixedUpdate()
    {
        if (_isMoving == true)
        {
            if (_isWaiting) return; // Blocage du mouvement pendant l'attente

            _MobAnimation.SetBool("IsWalking", true);
            Transform target = _pointsMovement._patrolPoints[_pointsMovement._targetPoints];


            // Si on est suffisamment proche du point
            if (Vector3.Distance(transform.position, target.position) <= _reachThreshold)
            {
                StartCoroutine(WaitingTime());
            }

            // Avancer vers la cible
            transform.position = Vector3.MoveTowards(transform.position, target.position, _pointsMovement._speed * Time.fixedDeltaTime);

            RotateMesh();
        }
    }

    void IncreaseTargetInt()
    {
        _pointsMovement._targetPoints++;

        if (_pointsMovement._targetPoints >= _pointsMovement._patrolPoints.Length)
        {
            _pointsMovement._targetPoints = 0;
        }
    }

    IEnumerator WaitingTime()
    {
        _MobAnimation.SetBool("IsWalking", false);
        _isWaiting = true;
        yield return new WaitForSeconds(_pointsMovement._waitingTime);
        IncreaseTargetInt();
        _isWaiting = false;
    }

    private void RotateMesh()
    {
        float target = _pointsMovement._patrolPoints[_pointsMovement._targetPoints].transform.position.x;
        float direction = transform.position.x - target;
        if (direction >= 0)
        {
            _ennemySprite.flipX = false;
        }
        else if (direction <= 0)
        {
            _ennemySprite.flipX = true;
        }
    }

}

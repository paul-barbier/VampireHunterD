using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform attackPivot;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float lowAngle;
    [SerializeField] private float highAngle;

    public Coroutine AttackCoroutine;
    public bool isAttacking = false;
    public bool canAttack = true;

    [SerializeField] private Animator _DAnimation;
    private PlayerCharacter _playerCharacter;


    void Awake()
    {
        _playerCharacter = GetComponent<PlayerCharacter>();
        attackPivot.gameObject.SetActive(false);
        canAttack = true;
        _playerCharacter._rotatePlayer = true;
    }

    void Update()
    {
        if (!isAttacking || _playerCharacter._isDashing)
            return;

        _playerCharacter._movementDisabled = true;

        _playerCharacter._rigidbody.linearVelocity = new Vector2(0, _playerCharacter._rigidbody.linearVelocity.y);
        _playerCharacter._currentHorizontalVelocity = Vector2.zero;
        _playerCharacter._forceToAdd = Vector2.zero;

        _playerCharacter._DAnimation.SetBool("IsRunning", false);
        _DAnimation.SetBool("IsAttacking", true);

        highAngle = Mathf.MoveTowards(highAngle, lowAngle, attackSpeed * Time.deltaTime);
        attackPivot.localRotation = Quaternion.Euler(0, 0, highAngle);

        if (Mathf.Approximately(highAngle, lowAngle))
        {
            isAttacking = false;
            attackPivot.gameObject.SetActive(false);
            highAngle = 50f;
            attackPivot.localRotation = Quaternion.identity;
            _DAnimation.SetBool("IsAttacking", false);
        }
    }

    public void AttackZone()
    {
        if (!canAttack || _playerCharacter._isDashing)
            return;

        isAttacking = true;
        canAttack = false;
        attackPivot.gameObject.SetActive(true);

        SoundManager.PlaySound(SoundType.Attack);

        StartCoroutine(AttackTime());
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackTime()
    {
        _playerCharacter._movementDisabled = true;
        _playerCharacter._rotatePlayer = false;
        yield return new WaitForSeconds(0.5f);
        _playerCharacter._rotatePlayer = true;
        _playerCharacter._movementDisabled = false;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        canAttack = true;
    }
}


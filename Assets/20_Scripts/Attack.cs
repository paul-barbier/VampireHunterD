using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform attackPivot;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float lowAngle;
    [SerializeField] private float highAngle;
    public bool isAttacking = false;
    [SerializeField] private Animator _DAnimation;
    private PlayerCharacter _playerCharacter;


    void Awake()
    {
        _playerCharacter = GetComponent<PlayerCharacter>();
        attackPivot.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isAttacking)
        {
            highAngle = Mathf.MoveTowards(highAngle, lowAngle, attackSpeed * Time.deltaTime);
            attackPivot.localRotation = Quaternion.Euler(0, 0, highAngle);
            _DAnimation.SetBool("IsAttacking", true);

            if (Mathf.Approximately(highAngle, lowAngle))
            {
                isAttacking = false;
                attackPivot.gameObject.SetActive(false);
                highAngle = 50f;
                attackPivot.localRotation = Quaternion.identity;
                _DAnimation.SetBool("IsAttacking", false);

            }
        }
    }

    public void AttackZone()
    {
        attackPivot.gameObject.SetActive(true);
        isAttacking = true;
        SoundManager.PlaySound(SoundType.Attack);
    }
}


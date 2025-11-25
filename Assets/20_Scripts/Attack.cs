using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private Transform attackPivot;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float lowAngle;
    [SerializeField] private float highAngle;
    private bool isAttacking = false;



    void Awake()
    {
        attackPivot.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isAttacking)
        {
            highAngle = Mathf.MoveTowards(highAngle, lowAngle, attackSpeed * Time.deltaTime);
            attackPivot.localRotation = Quaternion.Euler(0, 0, highAngle);

            if (Mathf.Approximately(highAngle, lowAngle))
            {
                isAttacking = false;
                attackPivot.gameObject.SetActive(false);
                highAngle = 50f;
                attackPivot.localRotation = Quaternion.identity;
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


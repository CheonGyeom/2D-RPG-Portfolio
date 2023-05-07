using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]
    public float health; // ���� ü��
    public float maxHealth; // �ִ� ü��

    public int attackDamage; // ���ݷ�
    public float moveSpeed; // �̵� �ӵ�

    public Transform[] attackTransform; // ���� ��ġ

    public float[] attackRadius; // ���� ����
    public float attackCoolDown; // ���� �� ���� �ð�
    public float nextAttackTime; // �ٽ� ������ ������ �ð�

    void Awake()
    {
        health = maxHealth; // ü�� �ʱ�ȭ
    }

    // ������� �޴� �Լ�
    public virtual void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        // ġ��Ÿ���
        if (isCritical)
        {
            health -= Mathf.RoundToInt(damage * (GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue)); // ũ��Ƽ�� �����
        }
        else
        {
            health -= damage;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    // �״� �Լ�
    public virtual void Die() { }

    // ����� ǥ��
    public virtual void OnDrawGizmosSelected()
    {
        // ���� ���� �����
        if (attackTransform.Length > 0)
        {
            Color gizmosColor = Gizmos.color;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackTransform[0].position, attackRadius[0]);
            Gizmos.color = gizmosColor;
        }
    }
}

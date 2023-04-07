using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField]
    public float health { get; private set; } // ���� ü��
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
    public virtual void TakeDamage(int damage, Transform Pos)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    // �״� �Լ�
    public virtual void Die() { }
}

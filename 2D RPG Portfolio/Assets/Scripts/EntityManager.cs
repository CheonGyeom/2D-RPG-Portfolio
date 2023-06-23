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

    public SpriteRenderer sr; // ��������Ʈ ������

    public float hitAnimFadeTime; // �ǰ� �ִϸ��̼� ���̵� �Ǵ� �ð�
    public bool isHitMonster; // ���� �ǰ� �������� üũ

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ �Ҵ�
        health = maxHealth; // ü�� �ʱ�ȭ
    }

    // ������� �޴� �Լ�
    public virtual void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        // ����� ���Ͷ��
        if (gameObject.tag == "Monster")
        {
            isHitMonster = true; // ���� �ǰ� ���� üũ

            SoundManager.instance.PlaySound("EnemyHit"); // ���� ���

            StartCoroutine("HitAnimation_Red"); // ���� �ǰ� �ִϸ��̼�
        }
        // ����� �÷��̾���
        else if (gameObject.tag == "Player")
        {
            StartCoroutine("HitAnimation_Alpha"); // �÷��̾� �ǰ� �ִϸ��̼�
        }    

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

    // ���� �ǰ� �ִϸ��̼� �ڷ�ƾ
    IEnumerator HitAnimation_Red()
    {
        // ��ƼƼ ���� ���������� ����
        sr.material.color = new Color(1f, 0.15f, 0.15f, 1f);

        // ���̵� �Ǵ� �ð�
        float time = 0;

        Color alpha = sr.material.color;

        while (alpha.g < 1f)
        {
            time += Time.deltaTime / hitAnimFadeTime;

            // ���̵� ��
            alpha.g = Mathf.Lerp(0.15f, 1f, time);
            alpha.b = Mathf.Lerp(0.15f, 1f, time);

            // ���� ����
            sr.material.color = alpha;

            yield return null;
        }

        isHitMonster = false; // ���� �ǰ� ���� ����

        yield return null;
    }

    // �÷��̾� �ǰ� �ִϸ��̼� �ڷ�ƾ
    IEnumerator HitAnimation_Alpha()
    {
        // ��ƼƼ ���� ����������� ����
        sr.material.color = new Color(1f, 1f, 1f, 0.45f);

        // ���̵� �Ǵ� �ð�
        float time = 0;

        Color alpha = sr.material.color;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / hitAnimFadeTime;

            // ���̵� ��
            alpha.a = Mathf.Lerp(0.45f, 1f, time);

            // ���� ����
            sr.material.color = alpha;

            yield return null;
        }
    }
}

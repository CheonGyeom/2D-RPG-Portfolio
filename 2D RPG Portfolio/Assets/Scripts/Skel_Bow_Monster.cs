using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skel_Bow_Monster : SkelManager
{
    Arrow ar;

    Rigidbody2D arrowRb; // ȭ�� ������ٵ�

    public Animator arrowHitFXAnim; // ȭ�� �浹 ����Ʈ �ִϸ�����

    public Animator secondHandsAnim; // �� ��° �� �ִϸ�����
    public Transform bulletPos; // ȭ���� �߻��� ��ġ

    public float arrowSpeed; // ȭ�� �ӵ�
    public float arrowDistance; // ȭ�� ��Ÿ�

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // �÷��̾� ��ġ �Ҵ�
        targetTransform = GameObject.FindWithTag("Player").transform;

        hp_fill = hp_fill_GameObject.GetComponent<Image>(); // HP �� fill ���� �̹��� �Ҵ�
        hp_fill_Lerp = hp_fill_Lerp_GameObject.GetComponent<Image>(); // HP �� �ε巯�� fill ���� �̹��� �Ҵ�

        hp_Bar.SetActive(false); // HP �� ������
    }

    private void FixedUpdate()
    {
        // �������� HP ��
        hp_fill_Lerp.fillAmount = Mathf.Lerp(hp_fill_Lerp.fillAmount, hp_fill.fillAmount, Time.deltaTime * 7.5f);

        // ���Ͱ� �׾��ٸ� ����
        if (isDie)
        {
            return;
        }

        MonsterAI();
        PlatformCheck();

        // ���� �����̰�, ���� �ִϸ��̼��� ����� ����Ǿ��ٸ�
        if (isAttack && (WeaponAnim.GetCurrentAnimatorStateInfo(0).IsName("Skel_OakBow_Attack") && WeaponAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f))
        {
            // ����
            Attack();
            isAttack = false;
        }
    }

    public override void Move()
    {
        base.Move();
    }

    public override void Attack()
    {
        // ������Ʈ Ǯ���� ȭ�� ������ �뿩
        GameObject arrow = ObjectPoolingManager.instance.GetObject("Bullet_Arrow");
        arrowRb = arrow.GetComponent<Rigidbody2D>();

        // ���� ���
        SoundManager.instance.PlaySound("SkelBow_Shot");

        ar = arrow.GetComponent<Arrow>();
        ar.skel = GetComponent<Skel_Bow_Monster>();

        // ȭ���� ��ġ ����
        arrow.transform.position = bulletPos.position;

        // ȭ���� ���� ����
        if (transform.localScale.x > 0)
        {
            arrow.transform.rotation = Quaternion.Euler(0, 0, handsPosObject.transform.rotation.eulerAngles.z - 90);

            arrowRb.velocity = handsPosObject.transform.right * arrowSpeed; // ȭ�� �̵�
        }
        else if (transform.localScale.x < 0)
        {
            arrow.transform.rotation = Quaternion.Euler(0, 0, handsPosObject.transform.rotation.eulerAngles.z + 90);

            arrowRb.velocity = handsPosObject.transform.right * -arrowSpeed; // ȭ�� �̵�
        }
    }

    public override void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        base.TakeDamage(damage, Pos, isCritical);

        secondHandsAnim.SetTrigger("Hit"); // �� ��° �� �´� �ִϸ��̼�
    }

    public override void Die()
    {
        base.Die();
    }

    public override void MonsterAI()
    {
        if (Vector2.Distance(transform.position, targetTransform.position) < attackDistance)  // �÷��̾ ���� ���� �ȿ� �ִ�.
        {
            // �̵� �ִϸ��̼� ����
            skelAnim.SetBool("isMove", false);

            // �÷��̾� ����
            AimPlayer();

            // ���� �ִϸ��̼��� ���� ������ ���� ��
            if (!WeaponAnim.GetCurrentAnimatorStateInfo(0).IsName("Skel_ShortSword_Attack"))
            {
                // ���� ���� ����
                if (targetTransform.position.x < transform.position.x)
                {
                    // ����
                    Vector3 scale = transform.localScale; // ������ ������ ��
                    scale.x = -Mathf.Abs(scale.x); // �������� x���� -����
                    transform.localScale = scale; // ������ ������ ���� ������

                    Vector3 hpScale = hp_Bar.transform.localScale;
                    hpScale.x = -Mathf.Abs(hpScale.x);
                    hp_Bar.transform.localScale = hpScale; // HP �ٴ� �׻� �������� ��
                }

                if (targetTransform.position.x > transform.position.x)
                {
                    // ������
                    Vector3 scale = transform.localScale; // ������ ������ ��
                    scale.x = Mathf.Abs(scale.x); // �������� x���� ����
                    transform.localScale = scale; // ������ ������ ���� ������

                    Vector3 hpScale = hp_Bar.transform.localScale;
                    hpScale.x = Mathf.Abs(hpScale.x);
                    hp_Bar.transform.localScale = hpScale; // HP �ٴ� �׻� �������� ��
                }
            }

            if (Time.time >= nextAttackTime)
            {
                // ���� �ִϸ��̼�
                handsAnim.SetTrigger("Attack");
                WeaponAnim.SetTrigger("Attack");

                isAttack = true; // ���� ���¸� true�� ����

                nextAttackTime = Time.time + attackCoolDown;
            }
        }
        // �÷��̾ ���� ���� �ۿ� �ִ�.
        else
        {
            // �´� �ִϸ��̼��� ���� ���� ��
            if (skelAnim.GetCurrentAnimatorStateInfo(0).IsName("Skel_Hit"))
            {
                return;
            }

            // �̵�
            Move();

            // ���� ����
            NotAim();

            // ���� X
            isAttack = false;

            if (Vector2.Distance(transform.position, targetTransform.position) <= followDistance) // �÷��̾ �ν� ���� �ȿ� �ִ�.
            {
                // �÷��̾� ��ġ ���� ��
                Vector3 playerPos = targetTransform.position;

                // �÷��̾��� x��ġ�� ������ x��ġ���� ������
                if (playerPos.x < transform.position.x)
                {
                    // �������� �̵�
                    nextMove = -1;
                }

                // �÷��̾��� x��ġ�� ������ ���� ������ ���ݺ��� ������
                else if ((playerPos.x > transform.position.x - attackDistance / 2) && (playerPos.x < transform.position.x + attackDistance / 2))
                {
                    // �̵� �� ��
                    nextMove = 0;
                }

                // �÷��̾��� x��ġ�� ������ x��ġ���� ũ��
                else if (playerPos.x > transform.position.x)
                {
                    // ���������� �̵�
                    nextMove = 1;
                }
            }

            // �÷��̾ �ν� ���� �ۿ� �ִ�.
            else
            {
                thinkTime += Time.deltaTime;

                // ���� �ൿ ���� ����
                if (thinkTime >= nextThinkTime)
                {
                    thinkTime = 0;

                    nextMove = Random.Range(-1, 2); // ���� -1 ~ 1���� ���� �ޱ�

                    nextThinkTime = Random.Range(2, 4); // ���� �ൿ�� ���ϱ���� �ɸ��� �ð� �ޱ�
                }
            }
        }
    }

    public override void PlatformCheck()
    {
        base.PlatformCheck();
    }

    public override void DropItem()
    {
        base.DropItem();
    }

    // �÷��̾� ���� �Լ�
    public void AimPlayer()
    {
        Vector3 dir = targetTransform.position - transform.position;

        if (transform.localScale.x > 0)
        {
            handsPosObject.transform.right = dir.normalized;
        }
        else if (transform.localScale.x < 0)
        {
            handsPosObject.transform.right = -dir.normalized;
        }
    }
    
    // ���� ���� �Լ�
    public void NotAim()
    {
        handsPosObject.transform.rotation = Quaternion.identity;
    }

    public override void Hide_HpBar()
    {
        base.Hide_HpBar();
    }

    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // ���Ÿ� �� �����
        Color gizmosColor = Gizmos.color;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = gizmosColor;
    }
}

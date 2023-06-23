using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skel_GraveSword_Monster : SkelManager
{
    public Animator secondHandsAnim; // �� ��° �� �ִϸ�����
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
        // �������� HP, EXP ��
        hp_fill_Lerp.fillAmount = Mathf.Lerp(hp_fill_Lerp.fillAmount, hp_fill.fillAmount, Time.deltaTime * 7.5f);

        // ���Ͱ� �׾��ٸ� ����
        if (isDie)
        {
            return;
        }

        MonsterAI();
        PlatformCheck();

        // ���� �����̰�, ���� �ִϸ��̼��� ����� ����Ǿ��ٸ�
        if (isAttack && (WeaponAnim.GetCurrentAnimatorStateInfo(0).IsName("Skel_GraveSword_Attack") && WeaponAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f))
        {
            // ����
            Attack();
        }
    }

    public override void Move()
    {
        base.Move();
    }

    public override void Attack()
    {
        base.Attack();

        // ���� ���� ���
        SoundManager.instance.PlaySound("GraveSwordSkel_Attack");

        Collider2D[] players = Physics2D.OverlapCircleAll(attackTransform[0].position, attackRadius[0], LayerMask.GetMask("Player"));

        if (players != null)
        {
            foreach (Collider2D player in players)
            {
                GameManager.instance.failCause = "�׷��̺�ҵ� ���̷��濡�� �й�"; // ��� ����

                attackPos = transform;
                player.GetComponent<PlayerManager>().TakeDamage(attackDamage, attackPos, false); // ������ �μ� = ���ʹ� ũ��Ƽ�� ���� ����
            }

            nextAttackTime = Time.time + attackCoolDown;

            isAttack = false; // ���� ���¸� false�� ����
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
        base.MonsterAI();

        if (Vector2.Distance(transform.position, targetTransform.position) < attackDistance)  // �÷��̾ ���� ���� �ȿ� �ִ�.
        {
            // �̵� �ִϸ��̼� ����
            skelAnim.SetBool("isMove", false);

            // ���� �ִϸ��̼��� ���� ������ ���� ��
            if (!WeaponAnim.GetCurrentAnimatorStateInfo(0).IsName("Skel_GraveSword_Attack"))
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
                secondHandsAnim.SetTrigger("Attack");
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

    public override void Hide_HpBar()
    {
        base.Hide_HpBar();
    }
}
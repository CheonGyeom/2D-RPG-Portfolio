using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skull_Monster : MonsterManager
{
    Rigidbody2D rb;
    Animator anim;
    Transform targetTransform; // �÷��̾��� ��ġ

    Transform attackPos; // ���ݹ޾��� �� ��밡 ������ ��ġ

    // ��� ������
    public GameObject coinPrefab; // ����
    public GameObject expPrefab; // ����ġ

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // �÷��̾� ��ġ �Ҵ�
        targetTransform = GameObject.FindWithTag("Player").transform;

        // HP �� �Ҵ�
        hp_Bar = GameObject.Find("Hp_Bar");
        hp_fill_GameObject = GameObject.Find("Hp_fill");
        hp_fill_Lerp_GameObject = GameObject.Find("Hp_fill_Lerp");

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
        if (isAttack && (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f))
        {
            // ����
            Attack();
        }
    }
    // �̵� �Լ�
    void Move()
    {
        // ���Ͱ� �׾��ų� ���� ���̶�� �̵� ����
        if (anim.GetBool("isDeath") || (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)))
        {
            return;
        }

        // �̵�
        rb.velocity = new Vector2(nextMove * moveSpeed, rb.velocity.y);

        // �̵� �ִϸ��̼�
        anim.SetBool("isMove", true);

        // ���� ���� ����
        if (nextMove < 0)
        {
            // ����
            Vector3 scale = transform.localScale; // ������ ������ ��
            scale.x = -Mathf.Abs(scale.x); // �������� x���� -����
            transform.localScale = scale; // ������ ������ ���� ������

            Vector3 hpScale = hp_Bar.transform.localScale;
            hpScale.x = -Mathf.Abs(hpScale.x);
            hp_Bar.transform.localScale = hpScale; // HP �ٴ� �׻� �������� ��
        }
        else if (nextMove > 0)
        {
            // ������
            Vector3 scale = transform.localScale; // ������ ������ ��
            scale.x = Mathf.Abs(scale.x); // �������� x���� ����
            transform.localScale = scale; // ������ ������ ���� ������

            Vector3 hpScale = hp_Bar.transform.localScale;
            hpScale.x = Mathf.Abs(hpScale.x);
            hp_Bar.transform.localScale = hpScale; // HP �ٴ� �׻� �������� ��
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }

    // ���� �Լ�
    void Attack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(attackTransform[0].position, attackRadius[0], LayerMask.GetMask("Player"));

        if (players != null)
        {
            foreach (Collider2D player in players)
            {
                attackPos = transform;
                player.GetComponent<PlayerManager>().TakeDamage(attackDamage, attackPos);
            }

            nextAttackTime = Time.time + attackCoolDown;

            isAttack = false; // ���� ���¸� false�� ����
        }
    }

    // ���Ͱ� ������� �޴� �Լ�
    public override void TakeDamage(int damage, Transform Pos)
    {
        base.TakeDamage(damage, Pos); // Entity Manager�� TakeDamage�Լ� ����
        anim.SetTrigger("Hit"); // �´� �ִϸ��̼�

        // ���� �ʾ��� ����
        if (!anim.GetBool("isDeath"))
        {
            // �˹�
            float x = transform.position.x - Pos.position.x; // �з��� ����
            if (x > 0)
            {
                rb.velocity = new Vector2(3f, rb.velocity.y); // ���������� 3��ŭ �˹� 
            }
            else if (x < 0)
            {
                rb.velocity = new Vector2(-3f, rb.velocity.y); // �������� 3��ŭ �˹� 
            }

            hp_Bar.SetActive(true); // HP �� Ȱ��ȭ

            CancelInvoke("Hide_HpBar"); // �ʱ�ȭ
            Invoke("Hide_HpBar", 2f); // 2�� �� HP �� ��Ȱ��ȭ
        }

        // ���� ����
        nextAttackTime = Time.time + attackCoolDown;

        hp_fill.fillAmount = health / maxHealth; // ü�¹� ����
    }

    // ���Ͱ� �״� �Լ�
    public override void Die()
    {
        base.Die();

        anim.SetBool("isDeath", true); // �״� �ִϸ��̼�

        GetComponent<Collider2D>().enabled = false; // �ݶ��̴� ����
        rb.isKinematic = true; // ��ġ ����
        rb.velocity = new Vector2(0, 0); // ��ġ ����

        hp_Bar.gameObject.SetActive(false); // HP �� ����
    }

    // ���� �ΰ����� �Լ�
    public override void MonsterAI()
    {
        if (Vector2.Distance(transform.position, targetTransform.position) < attackDistance)  // �÷��̾ ���� ���� �ȿ� �ִ�.
        {
            // ���� �ִϸ��̼��� ���� ������ ���� ��
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
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
                
                anim.SetTrigger("Attack");

                isAttack = true; // ���� ���¸� true�� ����

                nextAttackTime = Time.time + attackCoolDown;
            }
        }
        // �÷��̾ ���� ���� �ۿ� �ִ�.
        else
        {
            // �´� �ִϸ��̼��� ���� ���� ��
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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

                    nextThinkTime = Random.Range(2, 6); // ���� �ൿ�� ���ϱ���� �ɸ��� �ð� �ޱ�
                }
            }
        }
    }

    // AI�� ���������� ���� ���� ���� �Լ�
    public void PlatformCheck()
    {
        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            nextMove = 0; // ����
        }
    }

    void Hide_HpBar()
    {
        hp_Bar.SetActive(false);
    }
}

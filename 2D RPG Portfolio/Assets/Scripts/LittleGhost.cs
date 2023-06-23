using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGhost : MonsterManager
{
    public Rigidbody2D rb; // �������� ������ٵ�
    public Animator anim; // �������� �ִϸ�����

    public Animator dieAnim; // ��� �� ����Ʈ �ִϸ�����
    public Transform attackPos; // ���ݹ޾��� �� ��밡 ������ ��ġ

    Vector2 derection; // �÷��̾ �ִ� ����
    public float attackMoveSpeed; // ���� �� �̵� �ӵ�
    public bool isDash; // ���� �뽬���� ������ üũ

    private void Start()
    {
        // �÷��̾� ��ġ �Ҵ�
        targetTransform = GameObject.FindWithTag("Player").transform;

        hp_Bar.SetActive(false); // HP �� ������
    }

    private void Update()
    {
        // ���� ���¶��
        if (isAttack)
        {
            Collider2D[] players = Physics2D.OverlapCircleAll(attackTransform[0].position, attackRadius[0], LayerMask.GetMask("Player"));

            // �÷��̾ �ǰ� ���� �ȿ� ���Դٸ� ����� ����
            if (players != null)
            {
                foreach (Collider2D player in players)
                {
                    GameManager.instance.failCause = "�������ɿ��� �й�"; // ��� ����

                    attackPos = transform;
                    player.GetComponent<PlayerManager>().TakeDamage(attackDamage, attackPos, false); // ������ �μ� = ���ʹ� ũ��Ƽ�� ���� ����

                    isAttack = false; // ���� ���¸� false�� ����
                }

                nextAttackTime = Time.time + attackCoolDown;
            }
        }
    }

    private void FixedUpdate()
    {
        // �������� HP ��
        hp_fill_Lerp.fillAmount = Mathf.Lerp(hp_fill_Lerp.fillAmount, hp_fill.fillAmount, Time.deltaTime * 7.5f);

        // �÷��̾ �ִ� ���� ���
        derection = new Vector2(targetTransform.position.x - transform.position.x, targetTransform.position.y - transform.position.y);

        // ���Ͱ� �׾��ٸ� ����
        if (isDie)
        {
            return;
        }

        MonsterAI();
    }

    // �̵� �Լ�
    public virtual void Move()
    {
        // ���Ͱ� �׾��ų� �뽬���� ���̶�� �̵� ����
        if (isDie || isDash)
        {
            return;
        }

        // �Ÿ��� �ʹ� ������ ���� ���� �̵�
        if (Vector2.Distance(transform.position, targetTransform.position) > 0.5f)
        {
            rb.velocity = derection.normalized * moveSpeed; // �̵�
        }
        else
        {
            rb.velocity = Vector2.zero; // �ʹ� ������ �̵����� ����
        }

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
    }

    // �������� �뽬 ���� �Լ�
    public void Attack()
    {
        isAttack = true; // ���� ���� ����
        isDash = true; // ���� �뽬���� ����
        anim.SetBool("isAttack", isDash); // ���� �ִϸ��̼� 

        rb.velocity = derection.normalized * attackMoveSpeed; // �뽬 ����

        Invoke("EndAttack", 1.5f); // 1.5�� �� isAttack = false�� ����
    }

    // ���� �ൿ AI �Լ�
    public override void MonsterAI()
    {
        // �ǰ��� ���� ���̰ų� �뽬���� ���� ��
        if (isHitMonster || isDash)
        {
            return;
        }

        // �÷��̾� ��ġ ���� ��
        Vector3 playerPos = targetTransform.position;

        // �÷��̾��� x��ġ�� ������ x��ġ���� ������
        if (playerPos.x < transform.position.x - 0.5f)
        {
            // �������� �̵�
            nextMove = -1;
        }
        // �÷��̾��� x��ġ�� ������ x��ġ���� ũ��
        else if (playerPos.x > transform.position.x + 0.5f)
        {
            // ���������� �̵�
            nextMove = 1;
        }
        else
        {
            // ������
            nextMove = 0;
        }

        // �뽬���� ������ ���� ��
        if (!isDash)
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

        // �÷��̾ ���� ���� �ȿ� �ִ�.
        if (Vector2.Distance(transform.position, targetTransform.position) < attackDistance)
        {
            // ���� ��Ÿ���� �ƴ� ��
            if (Time.time >= nextAttackTime)
            {
                Attack(); // ����

                nextAttackTime = Time.time + attackCoolDown; // ��Ÿ��
            }
        }

        // �̵�
        Move();
    }

    // ����� ���� �Լ�
    public override void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        // �׾��ٸ� ����
        if (isDie)
        {
            return;
        }

        hp_Bar.SetActive(true); // HP �� Ȱ��ȭ

        CancelInvoke("Hide_HpBar"); // �ʱ�ȭ
        Invoke("Hide_HpBar", 2f); // 2�� �� HP �� ��Ȱ��ȭ

        EndAttack(); // �뽬���� ����

        base.TakeDamage(damage, Pos, isCritical); // ����� ����

        // ũ��Ƽ���̶��
        if (isCritical)
        {
            // ũ��Ƽ�� ����� ��� �ؽ�Ʈ ����
            GameObject damage_HudText = ObjectPoolingManager.instance.GetObject("HudText_CriticalDam");
            damage_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            damage_HudText.transform.GetChild(0).GetComponent<Damage_HudText>().ShowDamageText(Mathf.RoundToInt(damage * (GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue))); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�׸���)
            damage_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<DamageHudText_Shadow>().ShowDamageText(Mathf.RoundToInt(damage * (GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue))); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�ؽ�Ʈ)

            // �˹�
            float x = transform.position.x - Pos.position.x; // �з��� ����
            if (x > 0)
            {
                rb.velocity = new Vector2(5f, rb.velocity.y); // ���������� 5��ŭ �˹� 
            }
            else if (x < 0)
            {
                rb.velocity = new Vector2(-5f, rb.velocity.y); // �������� 5��ŭ �˹� 
            }
        }
        else
        {
            // ����� ��� �ؽ�Ʈ ����
            GameObject damage_HudText = ObjectPoolingManager.instance.GetObject("HudText_Damage");
            damage_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            damage_HudText.transform.GetChild(0).GetComponent<Damage_HudText>().ShowDamageText(damage); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�׸���)
            damage_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<DamageHudText_Shadow>().ShowDamageText(damage); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�ؽ�Ʈ)

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
        }

        hp_fill.fillAmount = health / maxHealth; // ü�¹� ����
    }

    // ��� �Լ�
    public override void Die()
    {
        base.Die();

        anim.SetTrigger("Die"); // �״� �ִϸ��̼�
        hp_Bar.gameObject.SetActive(false); // HP �� ��Ȱ��ȭ
    }

    // ������ ���
    public override void DropItem()
    {
        base.DropItem();

        // ����� ������ ���� ���� ��
        int rand_dropAmount = Random.Range(minItemDrop, maxItemDrop);

        // ����� ������ ������ŭ for�� ����
        for (int i = 0; i < rand_dropAmount; i++)
        {
            float rand_dropItem = Random.Range(0f, 1f);

            if (rand_dropItem < 0.55f) // ����ġ (55%) 
            {
                Debug.Log("����ġ ���");
                GameObject exp = ObjectPoolingManager.instance.GetObject("Item_Exp"); // ������Ʈ Ǯ���� ����ġ �뿩
                exp.transform.position = this.transform.position; // ��ġ �ʱ�ȭ
            }
            else if (rand_dropItem < 0.95f) // ���� (40%) 
            {
                Debug.Log("���� ���");
                GameObject coin = ObjectPoolingManager.instance.GetObject("Item_Coin"); // ������Ʈ Ǯ���� ���� �뿩
                coin.transform.position = this.transform.position; // ��ġ �ʱ�ȭ
            }
            else if (rand_dropItem < 1.0f) // �ݱ� (5%) 
            {
                Debug.Log("�ݱ� ���");
                GameObject bullion = ObjectPoolingManager.instance.GetObject("Item_Bullion"); // ������Ʈ Ǯ���� �ݱ� �뿩
                bullion.transform.position = this.transform.position; // ��ġ �ʱ�ȭ
            }
        }
    }

    // HP�ٸ� ����� �Լ�
    public override void Hide_HpBar()
    {
        base.Hide_HpBar();
    }

    // �뽬 ���� üũ ���� �����ϴ� �Լ�
    public void EndAttack()
    {
        isAttack = false; // ���� ���� ���� �ƴ�
        isDash = false; // ���� �뽬���� ���� �ƴ�
        anim.SetBool("isAttack", isDash); // ���� �ִϸ��̼�
    }
}

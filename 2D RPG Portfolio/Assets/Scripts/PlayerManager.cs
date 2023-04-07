using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : EntityManager
{
    Rigidbody2D rb;
    Animator anim;

    public float jumpPower; // ������
    public int jumpCount; // ���߿��� ������ Ƚ��
    public int maxJump; // �ִ� ���� Ƚ��
    private bool isGround; // �ٴڿ� ����ִ��� üũ
    public bool isWall; // ���� ����ִ��� üũ
    private bool isWallJump; // �� ������ �ϴ� ������ üũ
    public float wallSlideSpeed; // ���� �پ����� �� �̲����� �������� �ӵ�
    public int attackCombo; // �� ��° �������� ����

    public float dashDistance; // �뽬 �Ÿ�
    public float dashCooldown; // �뽬 ��Ÿ��
    public int maxDashChargeCount; // �ִ� �뽬 ������
    public int dashChargeCount; // �뽬 ������
    private float timer_Dash; // �뽬 Ÿ�̸�

    // UI ����
    public GameObject hp_Bar; // HP �� ���ӿ�����Ʈ
    public GameObject hp_fill_GameObject; // HP �� fill ���� ���ӿ�����Ʈ
    public GameObject hp_fill_Lerp_GameObject; // HP �� �ε巯�� fill ���� ���ӿ�����Ʈ
    public Image hp_fill; // HP �� fill
    public Image hp_fill_Lerp; // HP �� �ε巯�� fill

    public GameObject exp_Bar; // EXP �� ���ӿ�����Ʈ
    public GameObject exp_fill_GameObject; // EXP �� fill ���� ���ӿ�����Ʈ
    public GameObject exp_fill_Lerp_GameObject; // EXP �� �ε巯�� fill ���� ���ӿ�����Ʈ
    public Image exp_fill; // EXP �� fill
    public Image exp_fill_Lerp; // EXP �� �ε巯��fill

    public GameObject dashGauge1; // �뽬 ������ ù ��° ĭ
    public GameObject dashGauge2; // �뽬 ������ �� ��° ĭ


    Transform attackPos; // ���ݹ޾��� �� ��밡 ������ ��ġ

    // Ground üũ �ڽ�ĳ��Ʈ
    private Vector2 boxCastSize = new Vector2(0.6f, 0.8f);
    private float boxCastMaxDistance = 0.5f;

    // Wall üũ ����ĳ��Ʈ
    private float wallCastMaxDistance = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        dashChargeCount = maxDashChargeCount; // �뽬 Ǯ����
        timer_Dash = dashCooldown; // �뽬 Ÿ�̸� �ʱ�ȭ

        // HP �� �Ҵ�
        hp_Bar = GameObject.Find("Player_HP_Bar");
        hp_fill_GameObject = GameObject.Find("Player_HP_fill");
        hp_fill_Lerp_GameObject = GameObject.Find("Player_HP_fill_Lerp");

        hp_fill = hp_fill_GameObject.GetComponent<Image>(); // HP �� fill ���� �̹��� �Ҵ�
        hp_fill_Lerp = hp_fill_Lerp_GameObject.GetComponent<Image>(); // HP �� �ε巯�� fill ���� �̹��� �Ҵ�

        // EXP �� �Ҵ�
        exp_Bar = GameObject.Find("Player_EXP_Bar");
        exp_fill_GameObject = GameObject.Find("Player_EXP_fill");
        exp_fill_Lerp_GameObject = GameObject.Find("Player_EXP_fill_Lerp");

        exp_fill = exp_fill_GameObject.GetComponent<Image>(); // EXP �� fill ���� �̹��� �Ҵ�
        exp_fill_Lerp = exp_fill_Lerp_GameObject.GetComponent<Image>(); // EXP �� �ε巯�� fill ���� �̹��� �Ҵ�

        // �뽬 ������ �Ҵ�
        dashGauge1 = GameObject.Find("Gauge 1");
        dashGauge2 = GameObject.Find("Gauge 2");
    }

    void Update()
    {
        // �������� HP, EXP ��
        hp_fill_Lerp.fillAmount = Mathf.Lerp(hp_fill_Lerp.fillAmount, hp_fill.fillAmount, Time.deltaTime * 7.5f);
        exp_fill_Lerp.fillAmount = Mathf.Lerp(exp_fill_Lerp.fillAmount, exp_fill.fillAmount, Time.deltaTime * 7.5f);

        hp_fill.fillAmount = health / maxHealth; // HP �� ������Ʈ
        exp_fill.fillAmount = 0; // (�ӽ� exp �� ������Ʈ) ���� ����ġ / ���� ���������� ����ġ

        // �׾��ٸ� Return
        if (GameManager.instance.isDie)
        {
            return;
        }

        // ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // SŰ�� ������ �ְų� �뽬 ���̰ų� �� ���� ���̶�� �������� �ʵ��� ��
            if (Input.GetKey(KeyCode.S) 
                || isWallJump
                || (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
                || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)))
            {
                return;
            }

            // �� Ÿ�� ���̶��
            if (isWall && !isGround && !isWallJump)
            {
                isWallJump = true;
                //rb.velocity = new Vector2(transform.localScale.x * -5f, jumpPower * 0.95f);

                // �� ����
                rb.velocity = new Vector2(0, 0); // �ӵ� �ʱ�ȭ
                rb.AddForce(new Vector2(transform.localScale.x * -4.5f, jumpPower), ForceMode2D.Impulse);

                // ���� ��ȯ
                if (transform.localScale.x > 0) // �������� ���� �ִ� ���
                {
                    // �������� ���� ��ȯ
                    Vector3 scale = transform.localScale; // �÷��̾��� ������ ��
                    scale.x = -Mathf.Abs(scale.x); // �������� x���� -����
                    transform.localScale = scale; // �÷��̾��� ������ ���� 
                }
                else if (transform.localScale.x < 0) // ������ ���� �ִ� ���
                {
                    // ���������� ���� ��ȯ
                    Vector3 scale = transform.localScale; // �÷��̾��� ������ ��
                    scale.x = Mathf.Abs(scale.x); // �������� x���� -����
                    transform.localScale = scale; // �÷��̾��� ������ ���� 
                }
            }
            else
            {
                if (jumpCount < maxJump)
                {
                    // ����
                    Jump();
                    jumpCount++; // ���� Ƚ�� ����
                }
            }
        }

        // ���� �ִϸ��̼� �Ű�����
        anim.SetFloat("y_Velocity", rb.velocity.y);

        // ����
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // �뽬 ���̰ų� �� Ÿ�� ���̶�� ���� X
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Wall_Slide") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))))
                {
                    return;
                }
                else
                {
                    Attack();
                }
            }
        }

        // �޺��� 0�� �ƴ� ��
        if (attackCombo != 0)
        {
            // ���� �غ�ð� + 0.5f��ŭ�� �ð��� �����ٸ�
            if (Time.time >= nextAttackTime + 0.5f)
            {
                // �޺��� 0���� �ʱ�ȭ
                attackCombo = 0;
            }
        }

        // �뽬
        if (Input.GetMouseButtonDown(1) && dashChargeCount > 0)
        {
            // ���� ���̰ų� ���� �Ŵ޷��ִٸ� �뽬 ����
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                || anim.GetCurrentAnimatorStateInfo(0).IsName("Wall_Slide") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
            {
                return;
            }
            else
            {
                Dash();
            }
        }

        // �뽬 Ÿ�̸�
        if ((timer_Dash > 0) && !(dashChargeCount == maxDashChargeCount)) // Ÿ�̸Ӱ� �Ϸ���� �ʾҰ�, �뽬�� �ִ�� �������� �ʾҴٸ�
        {
            // Ÿ�̸� ����
            timer_Dash -= Time.deltaTime;
        }
        // Ÿ�̸Ӱ� 0�̸�
        else if (timer_Dash <= 0)
        {
            // �뽬 �������� �ִ� ���������� ���ٸ�
            if (dashChargeCount < maxDashChargeCount)
            {
                dashChargeCount++; // �뽬 1����
                timer_Dash = dashCooldown; // Ÿ�̸� �ʱ�ȭ
            }
        }

        // �뽬 ������
        dashGauge1.SetActive(false);
        dashGauge2.SetActive(false);

        if (dashChargeCount == 2) // �뽬�� 2�� �������ִٸ�
        {
            dashGauge1.SetActive(true);
            dashGauge2.SetActive(true);
        }
        else if (dashChargeCount == 1) // �뽬�� 1�� �������ִٸ�
        {
            dashGauge1.SetActive(true);
        }

        // �� Ÿ��
        if (isWall && !isGround && rb.velocity.y <= 0)
        {
            isWallJump = false;
            // y�ӵ��� �̲����� �������� �ӵ��� ���������� ������ �귯�������� ��
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallSlideSpeed);
        }
    }
    void FixedUpdate()
    {
        // �׾��ٸ� Return
        if (GameManager.instance.isDie)
        {
            return;
        }

        GroundCheck(); // ���� �ִ��� üũ
        WallCheck(); // ���� �پ��ִ��� üũ
        Move(); // �÷��̾� �̵�
    }

    // �̵� �Լ�
    void Move()
    {
        // ���� ���̰ų� �´� ���̰ų� �뽬 ���̰ų� �� ���� ���̶�� �������� �ʱ�
        if (isWallJump
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))))))
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(h, rb.velocity.y);

        // �÷��̾� ���� ����
        if (h < 0)
        {
            // ����
            Vector3 scale = transform.localScale; // �÷��̾��� ������ ��
            scale.x = -Mathf.Abs(scale.x); // �������� x���� -����
            transform.localScale = scale; // �÷��̾��� ������ ���� 
        }
        else if (h > 0)
        {
            // ������
            Vector3 scale = transform.localScale; // �÷��̾��� ������ ��
            scale.x = Mathf.Abs(scale.x); // �������� x���� ����
            transform.localScale = scale; // �÷��̾��� ������ ���� ������
        }

        // ������ ���� ��
        if (h == 0)
        {
            // �̵� �ִϸ��̼�
            anim.SetBool("isMove", false);
        }
        else
        {
            // �̵� �ִϸ��̼�
            anim.SetBool("isMove", true);
        }
    }

    // ���� �Լ�
    void Jump()
    {
        // ����
        rb.velocity = Vector2.up * jumpPower;
    }

    // ���� �Լ�
    void Attack()
    {
        anim.SetFloat("AttackCombo", attackCombo);
        anim.SetTrigger("Attack");

        // ������ ����Ǳ� ��
        if (!isGround) // ���߿� ���� ���� ����
        {
            // ���� ������ ���ϴ� �޺�3
            attackCombo = 3;
        }

        Collider2D[] monsters = Physics2D.OverlapCircleAll(attackTransform[attackCombo].position, attackRadius[attackCombo], LayerMask.GetMask("Monster"));

        if (monsters != null)
        {
            foreach (Collider2D monster in monsters)
            {
                attackPos = transform;
                monster.GetComponent<MonsterManager>().TakeDamage(attackDamage, attackPos);
            }
        }

        // ������ ���� ����
        if (attackCombo >= 2) // �޺��� �ִ� (2)���� ũ�ų� ������
        {
            // �޺� �ʱ�ȭ
            attackCombo = 0;
        }
        else
        {
            // �޺� �ܰ� ����
            attackCombo++;
        }

        nextAttackTime = Time.time + attackCoolDown;

    }

    // �뽬
    void Dash()
    {
        // �̹� �뽬 ���̶�� �뽬���� �ʱ�
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))))
        {
            return;
        }

        dashChargeCount--; // 1�뽬 ���

        anim.SetTrigger("Dash"); // �뽬 �ִϸ��̼�
        rb.velocity = new Vector2(transform.localScale.x * dashDistance, rb.velocity.y); // �뽬
    }

    // �÷��̾ ������� �Դ� �Լ�
    public override void TakeDamage(int damage, Transform Pos)
    {
        if (anim.GetBool("isDeath"))
            return;

        base.TakeDamage(damage, Pos);

        anim.SetTrigger("Hit");

        // �˹�
        float x = transform.position.x - Pos.position.x; // �з��� ����
        if (x > 0)
        {
            Debug.Log("test");
            rb.velocity = new Vector2(3f, rb.velocity.y); // ���������� 3��ŭ �˹� 
        }
        else if (x < 0)
        {
            Debug.Log("test");
            rb.velocity = new Vector2(-3f, rb.velocity.y); // �������� 3��ŭ �˹� // ���� ��ġ�� ���� �޾ƿ��� ���⸶�� ��ġ�� �ٸ��� ����
        }
    }

    // �÷��̾ �״� �Լ�
    public override void Die()
    {
        anim.SetBool("isDeath", true);
        GameManager.instance.isDie = true; // �÷��̾� ���
    }

    // �ٴ��� �ִ��� üũ�ϴ� �Լ�
    private void GroundCheck()
    {
        isGround = false;

        RaycastHit2D rayHit = Physics2D.BoxCast(this.transform.position, boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            isGround = true;
            jumpCount = 0; // ���� Ƚ�� �ʱ�ȭ

            isWallJump = false; // �� ���� ���� �ƴ�
        }

        // ���� �ִϸ��̼�
        anim.SetBool("isJump", !isGround);
    }

    // ���� �پ��ִ��� üũ�ϴ� �Լ�
    private void WallCheck()
    {
        isWall = false;

        RaycastHit2D rayHit = Physics2D.Raycast(this.transform.position, transform.localScale, wallCastMaxDistance, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            Debug.Log("�� ����");
            isWall = true; // �� üũ
        }

        // �� Ÿ�� �ִϸ��̼�
        anim.SetBool("isWall", isWall);
    }

    // ����� �׷��ִ� �Լ�
    private void OnDrawGizmos()
    {
        // �׶��� üũ �����
        RaycastHit2D groundRayHit = Physics2D.BoxCast(transform.position, boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Platform"));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundRayHit.distance, boxCastSize);
    }
}

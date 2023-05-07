using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : EntityManager
{
    Rigidbody2D rb;
    public Animator anim;

    public int jumpCount; // ���߿��� ������ Ƚ��
    private bool isGround; // �ٴڿ� ����ִ��� üũ
    public bool isWall; // ���� ����ִ��� üũ
    private bool isWallJump; // �� ������ �ϴ� ������ üũ
    public float wallSlideSpeed; // ���� �پ����� �� �̲����� �������� �ӵ�
    public int attackCombo; // �� ��° �������� ����

    public bool isCritical; // ũ��Ƽ�� �������� ����

    public float dashDistance; // �뽬 �Ÿ�
    public float dashCooldown; // �뽬 ��Ÿ��
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

    public TMP_Text hpText; // HP�� ��Ÿ���� �ؽ�Ʈ
    public TMP_Text expText; // EXP�� ��Ÿ���� �ؽ�Ʈ
    public TMP_Text levelText; // LV�� ��Ÿ���� �ؽ�Ʈ

    public GameObject dashGauge1; // �뽬 ������ ù ��° ĭ
    public GameObject dashGauge2; // �뽬 ������ �� ��° ĭ
    public GameObject dashGauge3; // �뽬 ������ �� ��° ĭ
    public GameObject dashGauge4; // �뽬 ������ �� ��° ĭ
    public GameObject dashGauge5; // �뽬 ������ �ټ� ��° ĭ
    public GameObject dashGauge6; // �뽬 ������ ���� ��° ĭ

    public GameObject dashFrame3; // �뽬 �� ��° ������
    public GameObject dashFrame4; // �뽬 �� ��° ������
    public GameObject dashFrame5; // �뽬 �ټ� ��° ������
    public GameObject dashFrame6; // �뽬 ���� ��° ������
    public GameObject gaugeLastFrame; // �뽬 ������ ������ �ݱ�

    Transform attackPos; // ���ݹ޾��� �� ��밡 ������ ��ġ

    Image damageScreen; // �ǰ� �� ȭ�� �����ڸ� �Ӿ��� �̹���
    Color damageScreen_Alpha; // ����
    public float damageScreen_AlphaSpeed; // ����� ��ũ���� ���������� �ӵ�

    // Ground üũ �ڽ�ĳ��Ʈ
    private Vector2 boxCastSize = new Vector2(0.6f, 0.8f);
    private float boxCastMaxDistance = 0.5f;

    // Wall üũ ����ĳ��Ʈ
    private float wallCastMaxDistance = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        dashChargeCount = GameManager.instance.maxDashChargeCount; // �뽬 Ǯ����
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

        // UI �ؽ�Ʈ �Ҵ�
        hpText = GameObject.Find("Hp_Text").GetComponent<TextMeshProUGUI>(); // HP �ؽ�Ʈ
        expText = GameObject.Find("Exp_Text").GetComponent<TextMeshProUGUI>(); // EXP �ؽ�Ʈ
        levelText = GameObject.Find("Level_Text").GetComponent<TextMeshProUGUI>(); // LV �ؽ�Ʈ

        // �뽬 ������ �Ҵ�
        dashGauge1 = GameObject.Find("Gauge 1");
        dashGauge2 = GameObject.Find("Gauge 2");
        dashGauge3 = GameObject.Find("Gauge 3");
        dashGauge4 = GameObject.Find("Gauge 4");
        dashGauge5 = GameObject.Find("Gauge 5");
        dashGauge6 = GameObject.Find("Gauge 6");

        dashFrame3 = GameObject.Find("Gauge Frame 3");
        dashFrame4 = GameObject.Find("Gauge Frame 4");
        dashFrame5 = GameObject.Find("Gauge Frame 5");
        dashFrame6 = GameObject.Find("Gauge Frame 6");

        gaugeLastFrame = GameObject.Find("Gauge Last Frame");

        // ����� ��ũ�� �Ҵ�
        damageScreen = GameObject.Find("DamageScreen").GetComponent<Image>();
        damageScreen_Alpha = damageScreen.color;

        // �޸��� ���� ��ƼŬ �ڷ�ƾ ����
        StartCoroutine("FootDustParticle");
    }

    void Update()
    {
        // �������� HP, EXP ��
        hp_fill_Lerp.fillAmount = Mathf.Lerp(hp_fill_Lerp.fillAmount, hp_fill.fillAmount, Time.deltaTime * 7.5f);
        exp_fill_Lerp.fillAmount = Mathf.Lerp(exp_fill_Lerp.fillAmount, exp_fill.fillAmount, Time.deltaTime * 7.5f);

        hp_fill.fillAmount = health / (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth); // HP �� ������Ʈ
        hpText.text = $"HP {health} / {(GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth)}"; // HP �ؽ�Ʈ ������Ʈ
        exp_fill.fillAmount = GameManager.instance.exp / GameManager.instance.levelUp_exp[GameManager.instance.level]; // EXP �� ������Ʈ (���� ����ġ / ���� ���������� ����ġ)
        expText.text = $"EXP {GameManager.instance.exp} / {GameManager.instance.levelUp_exp[GameManager.instance.level]}"; // EXP �ؽ�Ʈ ������Ʈ

        levelText.text = $"Lv.{GameManager.instance.level + 1}"; // LV �ؽ�Ʈ ������Ʈ

        // �������� ����� ��ũ��
        damageScreen_Alpha.a = Mathf.Lerp(damageScreen_Alpha.a, 0, damageScreen_AlphaSpeed * Time.deltaTime); // ���������� �̿��ؼ� �ε巴�� ���� ����
        damageScreen.color = damageScreen_Alpha; // ���� �ݿ�

        // HP�� �ִ� ü���� �� �ѵ��� ����
        if (health > (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth))
        {
            // �ִ� ü���� �Ѿ��� �� �ִ��������� ����
            health = (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth);
        }

        // ������
        while (GameManager.instance.exp >= GameManager.instance.levelUp_exp[GameManager.instance.level])
        {
            GameManager.instance.exp -= GameManager.instance.levelUp_exp[GameManager.instance.level]; // �������� �ʿ��� ����ġ �Ҹ�

            // ����ġ �ٰ� 0�������� �ٽ� ���������� �ʱ�ȭ
            exp_fill.fillAmount = 0;
            exp_fill_Lerp.fillAmount = 0;

            // �ִ� ü�� ������ ���� HPȹ��
            health += GameManager.instance.player_MaxHealth[GameManager.instance.level + 1] - GameManager.instance.player_MaxHealth[GameManager.instance.level]; // ���� ���� + 1�� �ִ� ü�¿��� ���� ������ �ִ� ü���� �� ���� ������

            GameManager.instance.LevelUp(); // ������
        }


        // �뽬 Ÿ�̸�
        if ((timer_Dash > 0) && !(dashChargeCount == (GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount))) // Ÿ�̸Ӱ� �Ϸ���� �ʾҰ�, �뽬�� �ִ�� �������� �ʾҴٸ�
        {
            // Ÿ�̸� ����
            timer_Dash -= Time.deltaTime;
        }
        // Ÿ�̸Ӱ� 0�̸�
        else if (timer_Dash <= 0)
        {
            // �뽬 �������� �ִ� ���������� ���ٸ�
            if (dashChargeCount < (GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount))
            {
                dashChargeCount++; // �뽬 1����
                timer_Dash = dashCooldown; // Ÿ�̸� �ʱ�ȭ
            }
        }

        // �뽬 ������
        dashGauge1.SetActive(false);
        dashGauge2.SetActive(false);
        dashGauge3.SetActive(false);
        dashGauge4.SetActive(false);
        dashGauge5.SetActive(false);
        dashGauge6.SetActive(false);

        switch (dashChargeCount)
        {
            case 1:
                dashGauge1.SetActive(true);
                break;
            case 2:
                dashGauge1.SetActive(true);
                dashGauge2.SetActive(true);
                break;
            case 3:
                dashGauge1.SetActive(true);
                dashGauge2.SetActive(true);
                dashGauge3.SetActive(true);
                break;
            case 4:
                dashGauge1.SetActive(true);
                dashGauge2.SetActive(true);
                dashGauge3.SetActive(true);
                dashGauge4.SetActive(true);
                break;
            case 5:
                dashGauge1.SetActive(true);
                dashGauge2.SetActive(true);
                dashGauge3.SetActive(true);
                dashGauge4.SetActive(true);
                dashGauge5.SetActive(true);
                break;
            case 6:
                dashGauge1.SetActive(true);
                dashGauge2.SetActive(true);
                dashGauge3.SetActive(true);
                dashGauge4.SetActive(true);
                dashGauge5.SetActive(true);
                dashGauge6.SetActive(true);
                break;
        }

        // �뽬 ������
        dashFrame3.SetActive(false);
        dashFrame4.SetActive(false);
        dashFrame5.SetActive(false);
        dashFrame6.SetActive(false);

        switch (GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount)
        {
            case 2:
                gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(180, -40);

                dashGauge3.SetActive(false);
                dashGauge4.SetActive(false);
                dashGauge5.SetActive(false);
                dashGauge6.SetActive(false);
                break;
            case 3:
                gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(270, -40);
                dashFrame3.SetActive(true);

                dashGauge4.SetActive(false);
                dashGauge5.SetActive(false);
                dashGauge6.SetActive(false);
                break;
            case 4:
                gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(360, -40);
                dashFrame3.SetActive(true);
                dashFrame4.SetActive(true);

                dashGauge5.SetActive(false);
                dashGauge6.SetActive(false);
                break;
            case 5:
                gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(450, -40);
                dashFrame3.SetActive(true);
                dashFrame4.SetActive(true);
                dashFrame5.SetActive(true);

                dashGauge6.SetActive(false);
                break;
            case 6:
                gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(540, -40);
                dashFrame3.SetActive(true);
                dashFrame4.SetActive(true);
                dashFrame5.SetActive(true);
                dashFrame6.SetActive(true);
                break;
        }

        // �뽬 �������� �ִ� ���������� ���ٸ�
        if (dashChargeCount > GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount)
        {
            // �뽬 �������� �ִ� ��������ŭ ����
            dashChargeCount = GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount;
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

        // �׾��ٸ� Return
        if (GameManager.instance.isDie)
        {
            return;
        }

        // ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // SŰ�� ������ �ְų� �뽬 ���̰ų� �� ���� ���̰ų� �κ��丮 �����ִ� ���̶�� �������� �ʵ��� ��
            if (Input.GetKey(KeyCode.S)
                || isWallJump
                || (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
                || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                || GameManager.instance.activeInventoty
                || GameManager.instance.activeEscMenu))
            {
                return;
            }

            // �� Ÿ�� ���̶��
            if (isWall && !isGround && !isWallJump)
            {
                isWallJump = true;

                // �� ����
                rb.velocity = new Vector2(0, 0); // �ӵ� �ʱ�ȭ
                rb.AddForce(new Vector2(transform.localScale.x * -4.5f, GameManager.instance.jumpPower + GameManager.instance.increased_JumpPower), ForceMode2D.Impulse);

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
                if (isGround)
                {
                    // ����
                    Jump();
                }
                else
                {
                    if (jumpCount + 1 < (GameManager.instance.maxJump + GameManager.instance.increased_MaxJump))
                    {
                        // ����
                        Jump();
                        jumpCount++; // ���� Ƚ�� ����
                    }
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
                // �뽬 ���̰ų� �� Ÿ�� ���̰ų� �´� ���̰ų� �κ��丮 �����ִ� ���̶�� ���� X
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Wall_Slide") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || GameManager.instance.activeInventoty
                    || GameManager.instance.activeEscMenu))))
                {
                    return;
                }
                else
                {
                    Attack();
                }
            }
        }

        // �뽬
        if (Input.GetMouseButtonDown(1) && dashChargeCount > 0)
        {
            // ���� ���̰ų� ���� �Ŵ޷��ְų� �κ��丮 �����ִٸ� �뽬 ����
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                || anim.GetCurrentAnimatorStateInfo(0).IsName("Wall_Slide") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                || GameManager.instance.activeInventoty
                || GameManager.instance.activeEscMenu)
            {
                return;
            }
            else
            {
                Dash();
            }
        }

        // �� Ÿ��
        if (isWall && !isGround && rb.velocity.y <= 0)
        {
            isWallJump = false;
            // y�ӵ��� �̲����� �������� �ӵ��� ���������� ������ �귯�������� ��
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallSlideSpeed);
        }

        // �κ��丮 ���ݱ�
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameManager.instance.activeInventoty = !GameManager.instance.activeInventoty;
            GameManager.instance.invectoryPanel.SetActive(GameManager.instance.activeInventoty);
        }

        // �Ͻ����� �޴� ���ݱ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ���� �޴��� �����־��ٸ�
            if (GameManager.instance.optionMenuPanel.activeSelf)
            {
                // �Ͻ����� �޴� �ݱ�
                GameManager.instance.optionMenuPanel.SetActive(false);
            }
            // �ƴ϶��
            else
            {
                // �Ͻ����� �޴� ���ݱ�
                GameManager.instance.activeEscMenu = !GameManager.instance.activeEscMenu;
                GameManager.instance.escMenuPanel.SetActive(GameManager.instance.activeEscMenu);
            }

        }
    }
    void FixedUpdate()
    {        
        GroundCheck(); // ���� �ִ��� üũ
        WallCheck(); // ���� �پ��ִ��� üũ

        // �׾��ٸ� Return
        if (GameManager.instance.isDie)
        {
            return;
        }

        Move(); // �÷��̾� �̵�
    }

    // �̵� �Լ�
    void Move()
    {
        // ���� ���̰ų� �´� ���̰ų� �뽬 ���̰ų� �� ���� ���̰ų� �κ��丮�� ���� ���¶�� �������� �ʱ�
        if (isWallJump
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))))))
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal") * (moveSpeed + GameManager.instance.increased_MoveSpeed);

        // �κ��丮�� �����ִٸ�
        if (GameManager.instance.activeInventoty)
        {
            // ������ ����
            h = 0;
        }
        
        rb.velocity = new Vector2(h, rb.velocity.y);

        // �÷��̾� ���� ����
        if (h < 0)
        {
            // ����
            Vector3 scale = transform.localScale; // �÷��̾��� ������ ��
            scale.x = -Mathf.Abs(scale.x); // �������� x���� -����
            transform.localScale = scale; // �÷��̾��� ������ ���� ������

            // ��ƼŬ ���� ��ȯ
            ParticleSystemRenderer particleRenderer = GameManager.instance.footDustParticle.GetComponent<ParticleSystemRenderer>(); // ��ƼŬ ������ ��� ��������
            particleRenderer.flip = new Vector3(1, 0, 0);

            // ��ƼŬ�� �������� ���� ��ȯ
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = GameManager.instance.footDustParticle.velocityOverLifetime;
            velocityOverLifetime.x = 1;

            // ��ƼŬ ��ġ�� �÷��̾�� �� �� �ڷ� �̵�
            ParticleSystem.ShapeModule shape = GameManager.instance.footDustParticle.shape;
            shape.position = new Vector3(0.25f, -0.5f, 0);
        }
        else if (h > 0)
        {
            // ������
            Vector3 scale = transform.localScale; // �÷��̾��� ������ ��
            scale.x = Mathf.Abs(scale.x); // �������� x���� ����
            transform.localScale = scale; // �÷��̾��� ������ ���� ������

            // ��ƼŬ ���� ��ȯ
            ParticleSystemRenderer particleRenderer = GameManager.instance.footDustParticle.GetComponent<ParticleSystemRenderer>(); // ��ƼŬ ������ ��� ��������
            particleRenderer.flip = new Vector3(0, 0, 0);

            // ��ƼŬ�� �������� ���� ��ȯ
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = GameManager.instance.footDustParticle.velocityOverLifetime;
            velocityOverLifetime.x = -1;

            // ��ƼŬ ��ġ�� �÷��̾�� �� �� �ڷ� �̵�
            ParticleSystem.ShapeModule shape = GameManager.instance.footDustParticle.shape;
            shape.position = new Vector3(-0.25f, -0.5f, 0);
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
        rb.velocity = Vector2.up * (GameManager.instance.jumpPower + GameManager.instance.increased_JumpPower);

        // 2�� �������
        if (!isGround)
        {
            // 2�� ���� ��ƼŬ ����
            GameManager.instance.doubleJumpParticle.Play();
        }
    }

    // ���� �Լ�
    void Attack()
    {
        anim.SetFloat("AttackCombo", attackCombo);
        anim.SetTrigger("Attack");

        // ũ��Ƽ��
        float rand_critical = Random.Range(0f, 1f);

        if (rand_critical <= (GameManager.instance.critical_Percentage + GameManager.instance.increased_CriticalPercentage))
        {
            isCritical = true; // ũ��Ƽ�� ����
        }
        else
        {
            isCritical = false; // ũ��Ƽ�� �ƴ�
        }

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
                monster.GetComponent<EntityManager>().TakeDamage((GameManager.instance.player_AttackDamage[GameManager.instance.level] + GameManager.instance.increased_AttackDamage), attackPos, isCritical);
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
    public override void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        if (anim.GetBool("isDeath"))
            return;

        base.TakeDamage(damage, Pos, isCritical); // ����� �ο�

        // ����� ��ũ��
        damageScreen_Alpha.a = 1; // ���� �ʱ�ȭ

        anim.SetTrigger("Hit");

        // �˹�
        float x = transform.position.x - Pos.position.x; // �з��� ����
        if (x > 0)
        {
            rb.velocity = new Vector2(3f, rb.velocity.y); // ���������� 3��ŭ �˹� 
        }
        else if (x < 0)
        {
            rb.velocity = new Vector2(-3f, rb.velocity.y); // �������� 3��ŭ �˹� // ���� ��ġ�� ���� �޾ƿ��� ���⸶�� ��ġ�� �ٸ��� ����
        }
    }

    // �÷��̾ �״� �Լ�
    public override void Die()
    {
        anim.SetBool("isDeath", true);
        GameManager.instance.isDie = true; // �÷��̾� ���

        GameManager.instance.deathMenu.SetActive(true);
    }

    // �ٴ��� �ִ��� üũ�ϴ� �Լ�
    private void GroundCheck()
    {
        isGround = false;

        RaycastHit2D rayHit = Physics2D.BoxCast(this.transform.position, boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null && (Mathf.Abs(rb.velocity.y) < 0.1f))
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

        if (rayHit.collider != null && rayHit.collider.gameObject.tag != "GoDownPlatform") // ������ �� �ִ� �÷����� ������ ��� �� ��
        {
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

    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // �޺� ���� ���� �����
        // �޺� 1
        Color comboOneGizmosColor = Gizmos.color;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackTransform[1].position, attackRadius[1]);
        Gizmos.color = comboOneGizmosColor;

        // �޺� 2
        Color comboTwoGizmosColor = Gizmos.color;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackTransform[2].position, attackRadius[2]);
        Gizmos.color = comboTwoGizmosColor;

        // ���� ����
        Color comboflyGizmosColor = Gizmos.color;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(attackTransform[3].position, attackRadius[3]);
        Gizmos.color = comboflyGizmosColor;
    }

    // �� ���� ��ƼŬ �ڷ�ƾ
    IEnumerator FootDustParticle()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move") && isGround)
        {
            GameManager.instance.footDustParticle.Play(); // �޸��� ���� ��ƼŬ
        }
        yield return new WaitForSeconds(0.45f);
        StartCoroutine("FootDustParticle");
    }
}

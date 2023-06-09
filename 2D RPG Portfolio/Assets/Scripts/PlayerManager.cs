using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : EntityManager
{
    public static PlayerManager instance; // 싱글톤을 할당할 전역변수

    Rigidbody2D rb;
    public Animator anim;

    public int jumpCount; // 공중에서 점프한 횟수
    private bool isGround; // 바닥에 닿아있는지 체크
    public bool isWall; // 벽에 닿아있는지 체크
    private bool isWallJump; // 벽 점프를 하는 중인지 체크
    public float wallSlideSpeed; // 벽에 붙어있을 때 미끄러져 내려오는 속도
    public int attackCombo; // 몇 번째 공격인지 구분
    public float canHitTime; // 무적 시간

    public bool isCritical; // 크리티컬 공격인지 구분

    public float dashDistance; // 대쉬 거리
    public float dashCooldown; // 대쉬 쿨타임
    public int dashChargeCount; // 대쉬 충전량
    private float timer_Dash; // 대쉬 타이머

    public ParticleSystem footDustParticle; // 이동할 때 흩날리는 먼지 파티클
    public ParticleSystem doubleJumpParticle; // 2단 점프할 때 나오는 파티클

    // UI 관련
    public GameObject hp_Bar; // HP 바 게임오브젝트
    public GameObject hp_fill_GameObject; // HP 바 fill 영역 게임오브젝트
    public GameObject hp_fill_Lerp_GameObject; // HP 바 부드러운 fill 영역 게임오브젝트
    public Image hp_fill; // HP 바 fill
    public Image hp_fill_Lerp; // HP 바 부드러운 fill

    public GameObject exp_Bar; // EXP 바 게임오브젝트
    public GameObject exp_fill_GameObject; // EXP 바 fill 영역 게임오브젝트
    public GameObject exp_fill_Lerp_GameObject; // EXP 바 부드러운 fill 영역 게임오브젝트
    public Image exp_fill; // EXP 바 fill
    public Image exp_fill_Lerp; // EXP 바 부드러운fill

    public TMP_Text hpText; // HP를 나타내는 텍스트
    public TMP_Text expText; // EXP를 나타내는 텍스트
    public TMP_Text levelText; // LV을 나타내는 텍스트

    Transform attackPos; // 공격받았을 때 상대가 공격한 위치

    Image damageScreen; // 피격 시 화면 가장자리 붉어짐 이미지
    Color damageScreen_Alpha; // 투명도
    public float damageScreen_AlphaSpeed; // 대미지 스크린이 투명해지는 속도

    // Ground 체크 박스캐스트
    private Vector2 boxCastSize = new Vector2(0.6f, 0.8f);
    private float boxCastMaxDistance = 0.5f;

    // Wall 체크 레이캐스트
    private float wallCastMaxDistance = 0.5f;

    // 싱글톤 패턴
    private void Awake()
    {
        // 싱글톤 변수가 비어있으면
        if (instance == null)
        {
            // 자신을 할당
            instance = this;
        }
        // 싱글톤 변수가 비어있지 않으면
        else
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 PlayerManager가 삭제되지 않음
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        dashChargeCount = GameManager.instance.maxDashChargeCount; // 대쉬 풀충전
        timer_Dash = dashCooldown; // 대쉬 타이머 초기화

        // HP 바 할당
        hp_Bar = GameObject.Find("Player_HP_Bar");
        hp_fill_GameObject = GameObject.Find("Player_HP_fill");
        hp_fill_Lerp_GameObject = GameObject.Find("Player_HP_fill_Lerp");

        hp_fill = hp_fill_GameObject.GetComponent<Image>(); // HP 바 fill 영역 이미지 할당
        hp_fill_Lerp = hp_fill_Lerp_GameObject.GetComponent<Image>(); // HP 바 부드러운 fill 영역 이미지 할당

        // EXP 바 할당
        exp_Bar = GameObject.Find("Player_EXP_Bar");
        exp_fill_GameObject = GameObject.Find("Player_EXP_fill");
        exp_fill_Lerp_GameObject = GameObject.Find("Player_EXP_fill_Lerp");

        exp_fill = exp_fill_GameObject.GetComponent<Image>(); // EXP 바 fill 영역 이미지 할당
        exp_fill_Lerp = exp_fill_Lerp_GameObject.GetComponent<Image>(); // EXP 바 부드러운 fill 영역 이미지 할당

        // UI 텍스트 할당
        hpText = GameObject.Find("Hp_Text").GetComponent<TextMeshProUGUI>(); // HP 텍스트
        expText = GameObject.Find("Exp_Text").GetComponent<TextMeshProUGUI>(); // EXP 텍스트
        levelText = GameObject.Find("Level_Text").GetComponent<TextMeshProUGUI>(); // LV 텍스트

        // 대미지 스크린 할당
        damageScreen = GameObject.Find("DamageScreen").GetComponent<Image>();
        damageScreen_Alpha = damageScreen.color;

        // 달리기 먼지 파티클 코루틴 시작
        StartCoroutine("FootDustParticle");
    }

    void Update()
    {
        // 선형보간 HP, EXP 바
        hp_fill_Lerp.fillAmount = Mathf.Lerp(hp_fill_Lerp.fillAmount, hp_fill.fillAmount, Time.deltaTime * 7.5f);
        exp_fill_Lerp.fillAmount = Mathf.Lerp(exp_fill_Lerp.fillAmount, exp_fill.fillAmount, Time.deltaTime * 7.5f);

        hp_fill.fillAmount = health / (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth); // HP 바 업데이트
        hpText.text = $"HP {health} / {(GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth)}"; // HP 텍스트 업데이트
        exp_fill.fillAmount = GameManager.instance.exp / GameManager.instance.levelUp_exp[GameManager.instance.level]; // EXP 바 업데이트 (현재 경험치 / 다음 레벨까지의 경험치)
        expText.text = $"EXP {GameManager.instance.exp} / {GameManager.instance.levelUp_exp[GameManager.instance.level]}"; // EXP 텍스트 업데이트

        levelText.text = $"Lv.{GameManager.instance.level + 1}"; // LV 텍스트 업데이트

        // 선형보간 대미지 스크린
        damageScreen_Alpha.a = Mathf.Lerp(damageScreen_Alpha.a, 0, damageScreen_AlphaSpeed * Time.deltaTime); // 선형보간을 이용해서 부드럽게 투명도 조절
        damageScreen.color = damageScreen_Alpha; // 투명도 반영

        // HP가 최대 체력을 못 넘도록 제한
        if (health > (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth))
        {
            // 최대 체력을 넘었을 시 최대제력으로 제한
            health = (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth);
        }

        // 레벨업
        while (GameManager.instance.exp >= GameManager.instance.levelUp_exp[GameManager.instance.level])
        {
            GameManager.instance.exp -= GameManager.instance.levelUp_exp[GameManager.instance.level]; // 레벨업에 필요한 경험치 소모

            // 경험치 바가 0에서부터 다시 차오르도록 초기화
            exp_fill.fillAmount = 0;
            exp_fill_Lerp.fillAmount = 0;

            // 최대 체력 증가에 따른 HP획득
            health += GameManager.instance.player_MaxHealth[GameManager.instance.level + 1] - GameManager.instance.player_MaxHealth[GameManager.instance.level]; // 현재 레벨 + 1의 최대 체력에서 현재 레벨의 최대 체력을 뺀 값을 더해줌

            GameManager.instance.LevelUp(); // 레벨업
        }


        // 대쉬 타이머
        if ((timer_Dash > 0) && !(dashChargeCount == (GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount))) // 타이머가 완료되지 않았고, 대쉬가 최대로 충전되지 않았다면
        {
            // 타이머 실행
            timer_Dash -= Time.deltaTime;
        }
        // 타이머가 0이면
        else if (timer_Dash <= 0)
        {
            // 대쉬 충전량이 최대 충전량보다 적다면
            if (dashChargeCount < (GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount))
            {
                dashChargeCount++; // 대쉬 1충전
                timer_Dash = dashCooldown; // 타이머 초기화
            }
        }

        // 대쉬 게이지
        GameManager.instance.dashGauge1.SetActive(false);
        GameManager.instance.dashGauge2.SetActive(false);
        GameManager.instance.dashGauge3.SetActive(false);
        GameManager.instance.dashGauge4.SetActive(false);
        GameManager.instance.dashGauge5.SetActive(false);
        GameManager.instance.dashGauge6.SetActive(false);

        switch (dashChargeCount)
        {
            case 1:
                GameManager.instance.dashGauge1.SetActive(true);
                break;
            case 2:
                GameManager.instance.dashGauge1.SetActive(true);
                GameManager.instance.dashGauge2.SetActive(true);
                break;
            case 3:
                GameManager.instance.dashGauge1.SetActive(true);
                GameManager.instance.dashGauge2.SetActive(true);
                GameManager.instance.dashGauge3.SetActive(true);
                break;
            case 4:
                GameManager.instance.dashGauge1.SetActive(true);
                GameManager.instance.dashGauge2.SetActive(true);
                GameManager.instance.dashGauge3.SetActive(true);
                GameManager.instance.dashGauge4.SetActive(true);
                break;
            case 5:
                GameManager.instance.dashGauge1.SetActive(true);
                GameManager.instance.dashGauge2.SetActive(true);
                GameManager.instance.dashGauge3.SetActive(true);
                GameManager.instance.dashGauge4.SetActive(true);
                GameManager.instance.dashGauge5.SetActive(true);
                break;
            case 6:
                GameManager.instance.dashGauge1.SetActive(true);
                GameManager.instance.dashGauge2.SetActive(true);
                GameManager.instance.dashGauge3.SetActive(true);
                GameManager.instance.dashGauge4.SetActive(true);
                GameManager.instance.dashGauge5.SetActive(true);
                GameManager.instance.dashGauge6.SetActive(true);
                break;
        }

        // 대쉬 프레임
        GameManager.instance.dashFrame3.SetActive(false);
        GameManager.instance.dashFrame4.SetActive(false);
        GameManager.instance.dashFrame5.SetActive(false);
        GameManager.instance.dashFrame6.SetActive(false);

        switch (GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount)
        {
            case 2:
                GameManager.instance.gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(180, -40);

                GameManager.instance.dashGauge3.SetActive(false);
                GameManager.instance.dashGauge4.SetActive(false);
                GameManager.instance.dashGauge5.SetActive(false);
                GameManager.instance.dashGauge6.SetActive(false);
                break;
            case 3:
                GameManager.instance.gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(270, -40);
                GameManager.instance.dashFrame3.SetActive(true);

                GameManager.instance.dashGauge4.SetActive(false);
                GameManager.instance.dashGauge5.SetActive(false);
                GameManager.instance.dashGauge6.SetActive(false);
                break;
            case 4:
                GameManager.instance.gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(360, -40);
                GameManager.instance.dashFrame3.SetActive(true);
                GameManager.instance.dashFrame4.SetActive(true);

                GameManager.instance.dashGauge5.SetActive(false);
                GameManager.instance.dashGauge6.SetActive(false);
                break;
            case 5:
                GameManager.instance.gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(450, -40);
                GameManager.instance.dashFrame3.SetActive(true);
                GameManager.instance.dashFrame4.SetActive(true);
                GameManager.instance.dashFrame5.SetActive(true);

                GameManager.instance.dashGauge6.SetActive(false);
                break;
            case 6:
                GameManager.instance.gaugeLastFrame.GetComponent<RectTransform>().anchoredPosition = new Vector2(540, -40);
                GameManager.instance.dashFrame3.SetActive(true);
                GameManager.instance.dashFrame4.SetActive(true);
                GameManager.instance.dashFrame5.SetActive(true);
                GameManager.instance.dashFrame6.SetActive(true);
                break;
        }

        // 대쉬 충전량이 최대 충전량보다 많다면
        if (dashChargeCount > GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount)
        {
            // 대쉬 충전량을 최대 충전량만큼 제한
            dashChargeCount = GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount;
        }

        // 콤보가 0이 아닐 때
        if (attackCombo != 0)
        {
            // 공격 준비시간 + 0.5f만큼의 시간이 지났다면
            if (Time.time >= nextAttackTime + 0.5f)
            {
                // 콤보를 0으로 초기화
                attackCombo = 0;
            }
        }

        // 죽었다면 또는 움직임 제어가 활성화되었다면 Return
        if (GameManager.instance.isDie || GameManager.instance.dontMove)
        {
            return;
        }

        // 점프 또는 대화
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 대화
            if (GameManager.instance.isTalk)
            {
                // 대화 선택 중이라면
                if (GameManager.instance.isChoiceTalk)
                {
                    // 현재 타이핑이 진행되고 있다면
                    if (GameManager.instance.talkText.isTyping)
                    {
                        // 대화 불러오지 않고 리턴
                        GameManager.instance.talkText.SetMessage("");
                        return;
                    }
                    // 타이핑이 완료되었다면
                    else
                    {
                        return;
                    }
                }

                // 다음 대화 불러오기
                GameManager.instance.Talk(GameManager.instance.currentNpcId, GameManager.instance.currentNpcName);
            }
            // 점프
            else
            {
                // S키를 누르고 있거나 대쉬 중이거나 벽 점프 중이거나 인벤토리 열려있는 중이라면 점프하지 않도록 함
                if (Input.GetKey(KeyCode.S)
                    || isWallJump
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || GameManager.instance.activeInventoty
                    || GameManager.instance.activeEscMenu))
                {
                    return;
                }

                // 벽 타기 중이라면
                if (isWall && !isGround && !isWallJump)
                {
                    isWallJump = true;

                    // 벽 점프
                    rb.velocity = new Vector2(0, 0); // 속도 초기화
                    rb.AddForce(new Vector2(transform.localScale.x * -4.5f, GameManager.instance.jumpPower + GameManager.instance.increased_JumpPower), ForceMode2D.Impulse);

                    SoundManager.instance.PlaySound("Player_Jump"); // 점프 효과음

                    // 방향 전환
                    if (transform.localScale.x > 0) // 오른쪽을 보고 있는 경우
                    {
                        // 왼쪽으로 방향 전환
                        Vector3 scale = transform.localScale; // 플레이어의 스케일 값
                        scale.x = -Mathf.Abs(scale.x); // 스케일의 x값의 -절댓값
                        transform.localScale = scale; // 플레이어의 스케일 값을 
                    }
                    else if (transform.localScale.x < 0) // 왼쪽을 보고 있는 경우
                    {
                        // 오른쪽으로 방향 전환
                        Vector3 scale = transform.localScale; // 플레이어의 스케일 값
                        scale.x = Mathf.Abs(scale.x); // 스케일의 x값의 -절댓값
                        transform.localScale = scale; // 플레이어의 스케일 값을 
                    }
                }
                else
                {
                    if (isGround) // 땅에 있을 때 점프
                    {
                        // 점프
                        Jump();
                    }
                    else // 공중에 있을 때 점프
                    {
                        if (jumpCount + 1 < (GameManager.instance.maxJump + GameManager.instance.increased_MaxJump))
                        {
                            // 점프
                            Jump();
                            jumpCount++; // 점프 횟수 증가
                        }
                    }
                }
            }

            // 점프 애니메이션 매개변수
            anim.SetFloat("y_Velocity", rb.velocity.y);
        }

        // 공격
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 대쉬 중이거나 벽 타기 중이거나 맞는 중이거나 인벤토리 열려있는 중이거나 대화 중이라면 공격 X
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Wall_Slide") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    || GameManager.instance.activeInventoty)
                    || GameManager.instance.activeEscMenu)
                    || GameManager.instance.isTalk))
                {
                    return;
                }
                else
                {
                    Attack();
                }
            }
        }

        // 대쉬
        if (Input.GetMouseButtonDown(1) && dashChargeCount > 0)
        {
            // 공격 중이거나 벽에 매달려있거나 인벤토리 열려있거나 대화 중이라면 대쉬 안함
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                || anim.GetCurrentAnimatorStateInfo(0).IsName("Wall_Slide") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                || GameManager.instance.activeInventoty
                || GameManager.instance.activeEscMenu
                || GameManager.instance.isTalk)
            {
                return;
            }
            else
            {
                Dash();
            }
        }

        // 벽 타기
        if (isWall && !isGround && rb.velocity.y <= 0)
        {
            isWallJump = false;
            // y속도에 미끄러져 내려오는 속도를 곱해줌으로 느리게 흘러내리도록 함
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallSlideSpeed);
        }

        // 인벤토리 여닫기
        if (Input.GetKeyDown(KeyCode.V))
        {
            // 대화 중이거나 상점이 열려있다면 인벤토리 여닫기 안함
            if (GameManager.instance.isTalk || GameManager.instance.townShopPanel.activeSelf || GameManager.instance.dungeonShopPanel.activeSelf)
            {
                return;
            }

            GameManager.instance.activeInventoty = !GameManager.instance.activeInventoty;
            GameManager.instance.invectoryPanel.SetActive(GameManager.instance.activeInventoty);

            // 인벤토리 열림 효과음 재생
            if (GameManager.instance.activeInventoty)
            {
                SoundManager.instance.PlaySound("OpenInventory");
            }
            else // 인벤토리가 닫혀있다면 툴팁 닫기
            {
                GameManager.instance.tooltipObject.SetActive(false); // 툴팁 닫기
            }
        }

        // 일시정지 메뉴 여닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 대화 중이라면 일시정지 메뉴 여닫기 안함
            if (GameManager.instance.isTalk)
            {
                return;
            }

            // 설정 메뉴가 열려있었다면
            if (GameManager.instance.optionMenuPanel.activeSelf)
            {
                // 설정 메뉴 닫기
                GameManager.instance.optionMenuPanel.SetActive(false);
            }
            // 아니라면
            else
            {
                // 인벤토리가 열려있었다면
                if (GameManager.instance.invectoryPanel.activeSelf)
                {
                    // 상점이 열려있었다면
                    if (GameManager.instance.townShopPanel.activeSelf || GameManager.instance.dungeonShopPanel.activeSelf)
                    {
                        // 상점 닫기
                        GameManager.instance.townShopPanel.SetActive(false);
                        GameManager.instance.dungeonShopPanel.SetActive(false);
                    }

                    // 인벤토리 닫기
                    GameManager.instance.activeInventoty = false;
                    GameManager.instance.invectoryPanel.SetActive(GameManager.instance.activeInventoty);

                    GameManager.instance.tooltipObject.SetActive(false); // 툴팁 닫기
                }
                else
                {
                    if (GameManager.instance.playerLocation == "던전")
                    {
                        // 일시정지 메뉴가 열려있었다면
                        if (GameManager.instance.activeEscMenu)
                        {
                            // 게임 일시정지 해제
                            Time.timeScale = 1f;

                            // 툴팁 지우기
                            GameManager.instance.uiTooltipObject.SetActive(false);

                            // 되묻는 창 닫기
                            GameManager.instance.realyEscapeDungeonPanel.SetActive(false);
                            GameManager.instance.realyGoMainPanel.SetActive(false);
                        }
                        else
                        {
                            // 게임 일시정지
                            Time.timeScale = 0f;
                        }

                        // 일시정지 메뉴 여닫기
                        GameManager.instance.activeEscMenu = !GameManager.instance.activeEscMenu;
                        GameManager.instance.escMenuPanel.SetActive(GameManager.instance.activeEscMenu);
                    }
                    else
                    {
                        // 일시정지 메뉴가 열려있었다면
                        if (GameManager.instance.activeEscMenu)
                        {
                            // 게임 일시정지 해제
                            Time.timeScale = 1f;

                            // 툴팁 지우기
                            GameManager.instance.uiTooltipObject.SetActive(false);

                            // 되묻는 창 닫기
                            GameManager.instance.realyEscapeDungeonPanel.SetActive(false);
                            GameManager.instance.realyGoMainPanel.SetActive(false);
                        }
                        else
                        {
                            // 게임 일시정지
                            Time.timeScale = 0f;
                        }

                        // 마을 일시정지 메뉴 여닫기
                        GameManager.instance.activeEscMenu = !GameManager.instance.activeEscMenu;
                        GameManager.instance.townEscMenuPanel.SetActive(GameManager.instance.activeEscMenu);
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        GroundCheck(); // 땅에 있는지 체크
        WallCheck(); // 벽에 붙어있는지 체크

        // 죽었다면 Return
        if (GameManager.instance.isDie)
        {
            return;
        }

        Move(); // 플레이어 이동
    }

    // 이동 함수
    void Move()
    {
        // 공격 중이거나 맞는 중이거나 대쉬 중이거나 벽 점프 중이거나 인벤토리가 열린 상태라면 움직이지 않기
        if (isWallJump
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))))))
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal") * (moveSpeed + GameManager.instance.increased_MoveSpeed);

        // 인벤토리가 열려있거나 대화 중이거나 움직임 제어가 활성화 되었다면
        if (GameManager.instance.activeInventoty || GameManager.instance.isTalk || GameManager.instance.dontMove)
        {
            // 움직임 제한
            h = 0;
        }
        
        rb.velocity = new Vector2(h, rb.velocity.y);

        // 플레이어 보는 방향
        if (h < 0)
        {
            // 왼쪽
            Vector3 scale = transform.localScale; // 플레이어의 스케일 값
            scale.x = -Mathf.Abs(scale.x); // 스케일의 x값의 -절댓값
            transform.localScale = scale; // 플레이어의 스케일 값을 재정의

            // 파티클 방향 전환
            ParticleSystemRenderer particleRenderer = footDustParticle.GetComponent<ParticleSystemRenderer>(); // 파티클 렌더러 모듈 가져오기
            particleRenderer.flip = new Vector3(1, 0, 0);

            // 파티클이 날려가는 방향 전환
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = footDustParticle.velocityOverLifetime;
            velocityOverLifetime.x = 1;

            // 파티클 위치를 플레이어보다 좀 더 뒤로 이동
            ParticleSystem.ShapeModule shape = footDustParticle.shape;
            shape.position = new Vector3(0.25f, -0.5f, 0);
        }
        else if (h > 0)
        {
            // 오른쪽
            Vector3 scale = transform.localScale; // 플레이어의 스케일 값
            scale.x = Mathf.Abs(scale.x); // 스케일의 x값의 절댓값
            transform.localScale = scale; // 플레이어의 스케일 값을 재정의

            // 파티클 방향 전환
            ParticleSystemRenderer particleRenderer = footDustParticle.GetComponent<ParticleSystemRenderer>(); // 파티클 렌더러 모듈 가져오기
            particleRenderer.flip = new Vector3(0, 0, 0);

            // 파티클이 날려가는 방향 전환
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = footDustParticle.velocityOverLifetime;
            velocityOverLifetime.x = -1;

            // 파티클 위치를 플레이어보다 좀 더 뒤로 이동
            ParticleSystem.ShapeModule shape = footDustParticle.shape;
            shape.position = new Vector3(-0.25f, -0.5f, 0);
        }

        // 가만히 있을 때
        if (h == 0)
        {
            // 이동 애니메이션
            anim.SetBool("isMove", false);
        }
        else
        {
            // 이동 애니메이션
            anim.SetBool("isMove", true);
        }
    }

    // 점프 함수
    void Jump()
    {
        // 점프
        rb.velocity = Vector2.up * (GameManager.instance.jumpPower + GameManager.instance.increased_JumpPower);

        SoundManager.instance.PlaySound("Player_Jump"); // 점프 효과음

        // 2단 점프라면
        if (!isGround)
        {
            // 2단 점프 파티클 실행
            doubleJumpParticle.Play();
        }
    }

    // 공격 함수
    void Attack()
    {
        anim.SetFloat("AttackCombo", attackCombo);
        anim.SetTrigger("Attack");

        SoundManager.instance.PlaySound("Player_Attack"); // 공격 효과음

        // 크리티컬
        float rand_critical = Random.Range(0f, 1f);

        if (rand_critical <= (GameManager.instance.critical_Percentage + GameManager.instance.increased_CriticalPercentage))
        {
            isCritical = true; // 크리티컬 맞음
        }
        else
        {
            isCritical = false; // 크리티컬 아님
        }

        // 공격이 진행되기 전
        if (!isGround) // 공중에 있을 떄의 공격
        {
            // 공중 공격을 뜻하는 콤보3
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

        // 공격이 끝난 이후
        if (attackCombo >= 2) // 콤보가 최대 (2)보다 크거나 같으면
        {
            // 콤보 초기화
            attackCombo = 0;
        }
        else
        {
            // 콤보 단계 증가
            attackCombo++;
        }

        nextAttackTime = Time.time + attackCoolDown;

    }

    // 대쉬
    void Dash()
    {
        // 이미 대쉬 중이라면 대쉬하지 않기
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            || (anim.GetCurrentAnimatorStateInfo(0).IsName("Air_Dash") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))))
        {
            return;
        }

        dashChargeCount--; // 1대쉬 사용

        anim.SetTrigger("Dash"); // 대쉬 애니메이션
        SoundManager.instance.PlaySound("Player_Dash"); // 대쉬 효과음

        rb.velocity = new Vector2(transform.localScale.x * dashDistance, rb.velocity.y); // 대쉬
    }

    // 플레이어가 대미지를 입는 함수
    public override void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        // 죽었거나 무적시간이라면 리턴
        if (anim.GetBool("isDeath") || !GameManager.instance.canHitPlayer)
        {
            return;
        }

        // 넉백
        float x = transform.position.x - Pos.position.x; // 밀려날 방향
        if (x > 0)
        {
            rb.velocity = new Vector2(3f, rb.velocity.y); // 오른쪽으로 3만큼 넉백 
            Debug.Log("넉백");
        }
        else if (x < 0)
        {
            rb.velocity = new Vector2(-3f, rb.velocity.y); // 왼쪽으로 3만큼 넉백
            Debug.Log("넉백");
        }

        base.TakeDamage(damage, Pos, isCritical); // 대미지 부여

        // 대미지 스크린
        damageScreen_Alpha.a = 1; // 투명도 초기화

        anim.SetTrigger("Hit");
        SoundManager.instance.PlaySound("Player_Hit"); // 대미지 효과음

        // 무적시간
        StartCoroutine("HitTimeCheck");
    }

    // 플레이어가 죽는 함수
    public override void Die()
    {
        anim.SetBool("isDeath", true);
        GameManager.instance.isDie = true; // 플레이어 사망

        SoundManager.instance.StopBackgroundMusic(); // BGM 끄기
        SoundManager.instance.PlaySound("AdventureFail"); // 탐험 실패 효과음 재생

        GameManager.instance.activeInventoty = false;
        GameManager.instance.invectoryPanel.SetActive(GameManager.instance.activeInventoty); // 인벤토리 닫기

        // 사망 정보창 정보 업데이트
        GameManager.instance.deathMenu_TimeText.text = (GameManager.instance.playDungeonTime / 60).ToString("00") + "분 " + (GameManager.instance.playDungeonTime % 60).ToString("00") + "초"; // 시간
        GameManager.instance.deathMenu_LocationText.text = string.Format("{0} {1}층", GameManager.instance.playerLocation, GameManager.instance.currentDungeonFloor); // 위치
        GameManager.instance.goldPenaltyText.SetActive(true); // 패널티 있음
        GameManager.instance.deathMenu_GoldText.text = string.Format("{0:n0} G", GameManager.instance.gold / 2); // 소지금
        GameManager.instance.clearMenu_GameClearTitleText.text = "사망 이유"; // 제목 텍스트 변경
        GameManager.instance.deathMenu_DeathCauseText.text = GameManager.instance.failCause; // 탐험 실패 이유
        GameManager.instance.clearMenu_ComentTitleText.text = "다음 레벨까지"; // 제목 텍스트 변경
        GameManager.instance.deathMenu_NextExpText.text = string.Format("{0} EXP 남음", GameManager.instance.levelUp_exp[GameManager.instance.level] - GameManager.instance.exp); // 다음 레벨까지 필요한 경험치

        // 획득했던 아이템
        for (int i = 0; i < GameManager.instance.iv.slots.Length; i++)
        {
            // 인벤토리 슬롯에 아이템이 있다면
            if (GameManager.instance.iv.slots[i].item != null)
            {
                // 사망 정보 창 슬롯 배열을 뒤져서
                for (int j = 0; j < GameManager.instance.iv.deathMenuSlots.Length; j++)
                {
                    // 비어있는 사망 정보 창 슬롯을 찾았다면
                    if (GameManager.instance.iv.deathMenuSlots[j].item == null)
                    {
                        // 비어있는 사망 정보 창 슬롯에 인벤토리 슬롯 아이템을 넣고 활성화 시킨다
                        GameManager.instance.iv.deathMenuSlots[j].AddItem(GameManager.instance.iv.slots[i].item);
                        GameManager.instance.iv.deathMenuSlots[j].gameObject.transform.parent.gameObject.SetActive(true);

                        break;
                    }
                }
            }
        }

        GameManager.instance.deathMenu.SetActive(true); // 사망 정보창 활성화
        GameManager.instance.adventureFailPanel.SetActive(true); // 탐험 실패 패널 활성화

        GameManager.instance.deathMenu.GetComponent<Animator>().SetTrigger("Show"); // 사망 정보 패널 애니메이션 재생
        GameManager.instance.adventureFailPanel.GetComponent<Animator>().SetTrigger("Show"); // 탐험 실패 패널 애니메이션 재생
    }

    // 바닥이 있는지 체크하는 함수
    private void GroundCheck()
    {
        isGround = false;

        RaycastHit2D rayHit = Physics2D.BoxCast(this.transform.position, boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null && (Mathf.Abs(rb.velocity.y) < 0.1f))
        {
            isGround = true;
            jumpCount = 0; // 점프 횟수 초기화

            isWallJump = false; // 벽 점프 상태 아님
        }

        // 점프 애니메이션
        anim.SetBool("isJump", !isGround);
    }

    // 벽에 붙어있는지 체크하는 함수
    private void WallCheck()
    {
        isWall = false;

        RaycastHit2D rayHit = Physics2D.Raycast(this.transform.position, transform.localScale, wallCastMaxDistance, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null && rayHit.collider.gameObject.tag != "GoDownPlatform") // 내려갈 수 있는 플랫폼은 벽으로 취급 안 함
        {
            isWall = true; // 벽 체크
        }

        // 벽 타기 애니메이션
        anim.SetBool("isWall", isWall);
    }

    // 기즈모를 그려주는 함수
    private void OnDrawGizmos()
    {
        // 그라운드 체크 기즈모
        RaycastHit2D groundRayHit = Physics2D.BoxCast(transform.position, boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Platform"));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.down * groundRayHit.distance, boxCastSize);
    }

    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // 콤보 공격 범위 기즈모
        // 콤보 1
        Color comboOneGizmosColor = Gizmos.color;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackTransform[1].position, attackRadius[1]);
        Gizmos.color = comboOneGizmosColor;

        // 콤보 2
        Color comboTwoGizmosColor = Gizmos.color;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackTransform[2].position, attackRadius[2]);
        Gizmos.color = comboTwoGizmosColor;

        // 공중 공격
        Color comboflyGizmosColor = Gizmos.color;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(attackTransform[3].position, attackRadius[3]);
        Gizmos.color = comboflyGizmosColor;
    }

    // 발 먼지 파티클 코루틴
    IEnumerator FootDustParticle()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move") && isGround)
        {
            footDustParticle.Play(); // 달리기 먼지 파티클

            SoundManager.instance.PlaySound("Player_Step"); // 발소리 효과음
        }
        yield return new WaitForSeconds(0.45f);
        StartCoroutine("FootDustParticle");
    }

    // 무적시간 코루틴
    IEnumerator HitTimeCheck()
    {
        GameManager.instance.canHitPlayer = false; // 무적 On

        yield return new WaitForSeconds(canHitTime); // 무적 시간 후

        GameManager.instance.canHitPlayer = true; // 무적 Off
    }
}

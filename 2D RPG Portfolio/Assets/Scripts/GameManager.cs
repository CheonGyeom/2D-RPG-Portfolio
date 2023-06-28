using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱����� �Ҵ��� ��������
    public Portal pt; // ��Ż ��ũ��Ʈ
    public Inventory iv; // �κ��丮 ��ũ��Ʈ
    public TownShop town_Shop; // ���� ���� ��ũ��Ʈ
    public DungeonShop dungeon_Shop; // ���� ���� ��ũ��Ʈ
    public GameObject fairySpawnPoint; // �� ��������Ʈ ������Ʈ

    public bool isDropFairy; // ���� ���������� ���� ��ȯ�ߴ��� üũ

    public GameObject player_Prefab; // �÷��̾� ������

    public GameObject player; // DontDestroy�� ����� ĵ���� ������Ʈ

    private GameObject[] monsterInStage; // ���� ���������� �ִ� ���� �迭
    public List<GameObject> monsterInStageList; // ���� ���������� �ִ� ���� ����Ʈ

    public List<string> stageSceneList; // ����(��������) �� ����Ʈ
    public List<string> randomStageSceneList; // �������� ���� ���� �� ����Ʈ

    public string playerLocation; // �÷��̾��� ��ġ
    public int currentDungeonFloor; // ���� ���� ��
    public int highDungeonFloor; // ������ ��
    public float playTime; // ���� �÷��� �ð�
    public float playDungeonTime; // ������ �ӹ� �ð�
    public float saveTime; // �ڵ� ���� ����

    public bool isInGame; // �ΰ������� üũ

    // UI ����
    public GameObject hud_CanvasObject; // HUD ĵ���� ������Ʈ

    public TMP_Text goldText; // ������ ��差 �ؽ�Ʈ (���� ���)

    public GameObject mainMenuPanel; // ���θ޴� â ������Ʈ
    public GameObject dataSlotSelectPanel; // ���θ޴� ������ ���� ���� �г� ������Ʈ
    public GameObject invectoryPanel; // �κ��丮 ������Ʈ
    public GameObject tooltipObject; // ���� ������Ʈ
    public GameObject deathMenu; // ���� �޴� ������Ʈ
    public GameObject escMenuPanel; // �Ͻ����� �޴� ������Ʈ
    public GameObject townEscMenuPanel; // ���� �Ͻ����� �޴� ������Ʈ
    public GameObject optionMenuPanel; // ���� �޴� ������Ʈ
    public GameObject townShopPanel; // ���� ���� â ������Ʈ
    public GameObject dungeonShopPanel; // ���� ���� â ������Ʈ

    public bool activeInventoty; // �κ��丮�� Ȱ�� ����
    public bool activeEscMenu; // �Ͻ����� �޴� Ȱ�� ����
    public bool activeShopPanel; // ���� â Ȱ�� ����

    public GameObject uiTooltipObject; // UI ���� ������Ʈ

    public GameObject realyEscapeDungeonPanel; // ������ ���� ������ �ǹ��� â ������Ʈ
    public GameObject realyGoMainPanel; // ����ȭ������ �� ������ �ǹ��� â ������Ʈ

    public TMP_Text townLocationText; // ���� �Ͻ����� �޴��÷��̾� ���� ��ġ �ؽ�Ʈ
    public TMP_Text townEscGoldText; // ���� �Ͻ����� �޴� ������ �ؽ�Ʈ
    public TMP_Text locationText; // �Ͻ����� �޴��÷��̾� ���� ��ġ �ؽ�Ʈ
    public TMP_Text escGoldText; // �Ͻ����� �޴� ������ �ؽ�Ʈ
    public TMP_Text playtimeText; // �Ͻ����� �޴� �ð� �ؽ�Ʈ
    public TMP_Text enemyText; // �Ͻ����� �޴� ���� �� �ؽ�Ʈ

    public GameObject mouseCursor; // ���콺 Ŀ�� ������Ʈ

    // �뽬 ������ ����
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

    // ��� ����â ����
    public TMP_Text deathMenu_TimeText; // �ð� �ؽ�Ʈ
    public TMP_Text deathMenu_LocationText; // ��ġ �ؽ�Ʈ
    public TMP_Text deathMenu_GoldText; // ������ �ؽ�Ʈ
    public TMP_Text deathMenu_DeathCauseText; // ��� ���� �ؽ�Ʈ
    public TMP_Text deathMenu_NextExpText; // ���� �������� ���� ����ġ �ؽ�Ʈ
    public GameObject adventureFailPanel; // Ž�� ���� �г� ������Ʈ

    // ���� Ŭ���� â ����
    public GameObject goldPenaltyText; // �г�Ƽ �ؽ�Ʈ ������Ʈ
    public TMP_Text clearMenu_GameClearTitleText; // ���� Ŭ���� (��� ����) ���� �ؽ�Ʈ
    public TMP_Text clearMenu_ComentTitleText; // ������ �ڸ�Ʈ (���� ��������) ���� �ؽ�Ʈ
    public GameObject adventureClearPanel; // Ž�� ���� �г� ������Ʈ

    // ��ȭâ ����
    public GameObject talkPanel; // ��ȭâ �г�
    public GameObject choicePanel; // ��ȭ ���� â
    public GameObject choiceBtn_Talk; // ��ȭ ���� ��ư - �̾߱� �ϱ�
    public GameObject choiceBtn_TownShop; // ��ȭ ���� ��ư - ���� ����
    public GameObject choiceBtn_DungeonShop; // ��ȭ ���� ��ư - ���� ����
    public TMP_Text npcNameText; // NPC �̸� �ؽ�Ʈ
    public TypingEffect talkText; // ��ȭ �ؽ�Ʈ
    public int talkIndex; // ��� ��ȣ
    public bool isTalk; // ���� ��ȭ ������ üũ
    public bool isChoiceTalk; // ���� ��ȭ ���� ������ üũ
    public int currentChoiceTalkIndex; // NPC���Լ� ���޹��� ��ȭ ����â�� ���� ��ȭ �ε���
    public int currentNpcId; // ���� ��ȭ ���� NPC ID
    public string currentNpcName; // ���� ��ȭ ���� NPC �̸�

    // ���� ����
    public List<Resolution> resolutionList = new List<Resolution>(); // �ػ� ����Ʈ
    public FullScreenMode[] displayModeList; // ȭ�� ���

    private int displayModePage; // ���� ȭ�� ��� ������
    private int resolutionPage; // ���� �ػ� ������
    public TMP_Text displayModeText; // ���� ȭ�� ��� �ؽ�Ʈ
    public TMP_Text resolutionText; // ���� �ػ� �ؽ�Ʈ

    // ������ ���� ����
    public RectTransform tooltip_rt; // ���� ��Ʈ Ʈ������

    public TMP_Text tooltip_ItemName; // ���� ������ �̸�
    public TMP_Text tooltip_ItemDescription; // ���� ������ ����
    public Image tooltip_ItemImage; // ���� ������ �̹���

    public GameObject tooltip_IncreaseMaxHealthObject; // �ִ� ü�� ������ ������Ʈ
    public GameObject tooltip_IncreaseAttackDamageObject; // ���ݷ� ������ ������Ʈ
    public GameObject tooltip_IncreaseMoveSpeedObject; // �̵� �ӵ� ������ ������Ʈ
    public GameObject tooltip_IncreaseJumpPowerObject; // ������ ������ ������Ʈ
    public GameObject tooltip_IncreaseCriticalPercentageObject; // ũ��Ƽ�� Ȯ�� ������ ������Ʈ
    public GameObject tooltip_IncreaseCriticalValueObject; // ũ��Ƽ�� ��� ������ ������Ʈ
    public GameObject tooltip_IncreaseJumpCountObject; // ���� Ƚ�� ������ ������Ʈ
    public GameObject tooltip_IncreaseDashCountObject; // �뽬 Ƚ�� ������ ������Ʈ

    public TMP_Text tooltip_IncreaseMaxHealthText; // �ִ� ü�� ������ �ؽ�Ʈ
    public TMP_Text tooltip_IncreaseAttackDamageText; // ���ݷ� ������ �ؽ�Ʈ
    public TMP_Text tooltip_IncreaseMoveSpeedText; // �̵� �ӵ� ������ �ؽ�Ʈ
    public TMP_Text tooltip_IncreaseJumpPowerText; // ������ ������ �ؽ�Ʈ
    public TMP_Text tooltip_IncreaseCriticalPercentageText; // ũ��Ƽ�� Ȯ�� ������ �ؽ�Ʈ
    public TMP_Text tooltip_IncreaseCriticalValueText; // ũ��Ƽ�� ��� ������ �ؽ�Ʈ
    public TMP_Text tooltip_IncreaseJumpCountText; // ���� Ƚ�� ������ �ؽ�Ʈ
    public TMP_Text tooltip_IncreaseDashCountText; // �뽬 Ƚ�� ������ �ؽ�Ʈ

    // ���̵� �ξƿ� ���� + �� ��ȯ
    public Image fadePanel; // ���̵��ξƿ� �г� ������Ʈ
    public float fadeTime; // ���̵� �Ǵ� �ð�
    public bool isFade; // ���� ���̵� ������ üũ
    public bool canSceneLoad; // ���̵尡 �Ϸ�Ǿ� ���� �ҷ��;� �ϴ� �������� üũ
    public string loadSceneName; // �ε��� �� �̸�
    public bool isLoadScene; // ���� �ε��ߴ��� üũ
    public AsyncOperation operation; // ���� ���õ� ������ ������� Ŭ����

    // �÷��̾� ����
    public bool isDie; // �÷��̾ �׾�����
    public bool dontMove; // �÷��̾� ������ ���� üũ
    public bool canHitPlayer; // �÷��̾ ���ݴ��� �� �ִ��� üũ (�����ð�)

    public int level; // �÷��̾� ����
    public int maxLevel; // �÷��̾� �ִ� ����
    public int[] levelUp_exp; // �������� �ʿ��� ����ġ�� �迭

    public float gold; // ���
    public float lerpGold; // ����ؽ�Ʈ ������ ���� ����
    public float exp; // ����ġ

    public string failCause; // Ž�� ���� ����

    public bool tutorial_Complete; // Ʃ�丮�� �Ϸ� ����
    public bool isTutorial; // ���� Ʃ�丮�� ���� ������ ����


    // �÷��̾� �ɷ�ġ

    // ������ ���� ��ȭ��
    public int[] player_AttackDamage; // ���ݷ�
    public float[] player_MaxHealth; // �ִ� ü��

    // �⺻ ���� ��ġ
    public float jumpPower;// ������
    public int maxJump; // ����Ƚ��
    public int maxDashChargeCount; // �뽬 Ƚ��
    public float player_MoveSpeed; // �̵��ӵ�    
    public float critical_Percentage; // ũ��Ƽ�� Ȯ��
    public float critical_Value; // ũ��Ƽ�� ����� ����

    // ������ ������ ���� �����Ǵ� �߰� �ɷ�ġ
    public float increased_MaxHealth; // �߰� �ִ� ü��
    public int increased_AttackDamage; // �߰� ���ݷ�
    public float increased_JumpPower; // �߰� ������
    public int increased_MaxJump; // �߰� ���� Ƚ��
    public int increased_MaxDashCount; // �߰� �뽬 Ƚ��
    public float increased_MoveSpeed; // �߰� �̵� �ӵ�
    public float increased_CriticalPercentage; // �߰� ũ��Ƽ�� Ȯ��
    public float increased_CriticalValue; // �߰� ũ��Ƽ�� ���

    // �̱��� ����
    private void Awake()
    {
        // �̱��� ������ ���������
        if (instance == null)
        {
            // �ڽ��� �Ҵ�
            instance = this;
        }
        // �̱��� ������ ������� ������
        else
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� GameManager�� �������� ����

        DontDestroyOnLoad(hud_CanvasObject); // HUD ĵ���� DontDestroy ������ �̵�

        // �̺�Ʈ ���
        SceneManager.sceneLoaded += LoadedsceneEvent;

        randomStageSceneList = stageSceneList.ToList(); // ���� �� �̸� ����Ʈ �ʱ�ȭ

        resolutionList.AddRange(Screen.resolutions); // ȭ�� �ػ� ����Ʈ �ʱ�ȭ
    }

    private void Start()
    {
        invectoryPanel.SetActive(false); // �κ��丮 �г� �ݱ�

        // �ʱ�ȭ ������ ���θ޴� ������ �̵�
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        // ��� ����
        lerpGold = Mathf.Lerp(lerpGold, gold, Time.deltaTime * 60f);

        // ��� ������ �ؽ�Ʈ ������Ʈ
        goldText.text = string.Format("{0:n0} G", lerpGold); // ���� ���
        escGoldText.text = string.Format("{0:n0} G", lerpGold); ; // ���� �Ͻ����� �޴�
        townEscGoldText.text = string.Format("{0:n0} G", lerpGold); ; // �Ͻ����� �޴�

        // Esc Ű�� ������ �� ���� �޴��� �����־��ٸ�
        if (Input.GetKeyDown(KeyCode.Escape) && optionMenuPanel.activeSelf)
        {
            // ���� �޴� �ݱ�
            optionMenuPanel.SetActive(false);
        }

        // ���� �ҷ��;� �� ��
        if (canSceneLoad)
        {
            // ���� �� �ε带 ��û���� �ʾҴٸ�
            if (isLoadScene == false)
            {
                Debug.Log("�� �ε� ��û");

                // �񵿱� ������� �� �ε� (�ε� �Ϸ� ���θ� Ȯ���ϱ� ����
                operation = SceneManager.LoadSceneAsync(loadSceneName);
                isLoadScene = true; // �� �ε� üũ
            }
            // �� �ε带 ��û�ߴٸ�
            else
            {
                if (operation.isDone) // �� �ε��� �����ٸ�
                {
                    StartCoroutine("FadeOut");
                }
            }
        }

        // ������ ������ 1�п� �ѹ��� �ڵ� ����
        if (playerLocation == "����" && playTime > saveTime)
        {
            // ���� ���� ���� ����
            saveTime = playTime;
            saveTime += 15f;

            Save(); // ����
        }

        // ���� �÷��� �ð� ������Ʈ
        if (isInGame)
        {
            playTime += Time.deltaTime;
        }

        // ������ �ӹ� �ð� ������Ʈ
        if (playerLocation == "����" && !isDie) // ������ �ְ� ���� �ʾ��� ��
        {
            playDungeonTime += Time.deltaTime;
            playtimeText.text = (playDungeonTime / 60).ToString("00") + "�� " + (playDungeonTime % 60).ToString("00") + "��";
        }

        // ��Ż �Ŵ����� �Ҵ�Ǿ��ְ�
        if (pt != null)
        {
            // ���������� ��� ���͸� ó���ߴٸ�
            if (monsterInStageList.Count == 0)
            {
                // ��Ż ����
                pt.isPortalOpen = true;

                // ���� �� �ؽ�Ʈ ������Ʈ
                enemyText.text = "����";

                // �� ����
                if (!isDropFairy && fairySpawnPoint != null)
                {
                    isDropFairy = true; // �� ���� ó��

                    float rand_FairySize = Random.Range(0f, 1f);

                    if (rand_FairySize < 0.30f) // None (30%) 
                    {
                        Debug.Log("Not Fairy");
                    }
                    else if (rand_FairySize < 0.70f) // M (40%) 
                    {
                        Debug.Log("Fairy M");
                        GameObject fairy = ObjectPoolingManager.instance.GetObject("Item_FairyM");
                        fairy.transform.position = fairySpawnPoint.transform.position;

                        // �� ���� ȿ���� ���
                        SoundManager.instance.PlaySound("SpawnFairy");
                    }
                    else if (rand_FairySize < 0.95f) // L (25%) 
                    {
                        Debug.Log("Fairy L");
                        GameObject fairy = ObjectPoolingManager.instance.GetObject("Item_FairyL");
                        fairy.transform.position = fairySpawnPoint.transform.position;

                        // �� ���� ȿ���� ���
                        SoundManager.instance.PlaySound("SpawnFairy");
                    }
                    else if (rand_FairySize < 1.0f) // XL (5%) 
                    {
                        Debug.Log("Fairy XL");
                        GameObject fairy = ObjectPoolingManager.instance.GetObject("Item_FairyXL");
                        fairy.transform.position = fairySpawnPoint.transform.position;

                        // �� ���� ȿ���� ���
                        SoundManager.instance.PlaySound("SpawnFairy");
                    }
                }
            }
            // ���������� ���Ͱ� �����ִٸ�
            else
            {
                // ��Ż �̰���
                pt.isPortalOpen = false;

                // ���� �� �ؽ�Ʈ ������Ʈ
                enemyText.text = monsterInStageList.Count.ToString();
            }
        }

        // ���θ޴� ���̰ų� �޴� �Ǵ� �κ��丮�� Ȱ��ȭ �Ǿ��ְų� Ž�����, Ž�輺�� �г��� Ȱ��ȭ �Ǿ��ְų� ��ȭ ���� ���̶��
        if (SceneManager.GetActiveScene().name == "Main" || activeInventoty || activeEscMenu || choicePanel.activeSelf || deathMenu.activeSelf)
        {
            mouseCursor.SetActive(true); // Ŀ�� ���� ����
            Cursor.lockState = CursorLockMode.None; // Ŀ�� ���� ����
        }
        // �޴� �Ǵ� �κ��丮�� ��Ȱ��ȭ �Ǿ��ִٸ�
        else
        {
            mouseCursor.SetActive(false); // Ŀ�� ����
            Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ����
        }
    }

    // �� ���� �� ȣ��Ǵ� �޼���
    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        // �÷��̾� ���� ��ġ ����
        switch (scene.buildIndex)
        {
            case 1: // ���� �� ��ȣ : 1
                SoundManager.instance.PlayBackgroundMusic("MainMenu"); // ���θ޴� BGM ���
                mainMenuPanel.SetActive(true); // ���θ޴� Ȱ��ȭ

                isInGame = false; // �ΰ��� �ƴ�

                // �÷��̾� ����
                Destroy(player);
                break;

            case 2: // Ʃ�丮�� �� ��ȣ : 2
                isInGame = true; // �ΰ���

                mainMenuPanel.SetActive(false); // ���θ޴� ��Ȱ��ȭ
                dataSlotSelectPanel.SetActive(false); // ������ ���� ���� �г� ��Ȱ��ȭ

                // �÷��̾ �Ҵ���� �ʾҴٸ�
                if (player == null)
                {
                    // �÷��̾� ����
                    player = Instantiate(player_Prefab);
                }

                isTutorial = true; // Ʃ�丮�� ���� ����

                playerLocation = "Ʃ�丮��";
                townLocationText.text = playerLocation; // ���� �Ͻ����� �޴� ���� ��ġ �ؽ�Ʈ ������Ʈ

                player.transform.position = new Vector3(-32f, 4.5f, 0); // �÷��̾� ��ġ

                // ü�� �ʱ�ȭ
                PlayerManager.instance.health = player_MaxHealth[level];

                // ��Ż �Ŵ����� �Ҵ���� �ʾҴٸ�
                if (pt == null)
                {
                    pt = GameObject.Find("Portal").GetComponent<Portal>(); // ��Ż �Ҵ�

                    StageMonsterCheck(); // ���ӸŴ����� �������� ���� üũ �Լ� ����
                }

                SoundManager.instance.PlayBackgroundMusic("Cave"); // Ʃ�丮�� BGM ���

                break;

            case 3: // ���� �� ��ȣ : 3
                playerLocation = "����";

                playDungeonTime = 0; // ������ �ӹ� �ð� �ʱ�ȭ
                townLocationText.text = playerLocation; // ���� �Ͻ����� �޴� ���� ��ġ �ؽ�Ʈ ������Ʈ

                isInGame = true; // �ΰ���
                isTutorial = false; // Ʃ�丮�� ����

                mainMenuPanel.SetActive(false); // ���θ޴� ��Ȱ��ȭ
                dataSlotSelectPanel.SetActive(false); // ������ ���� ���� �г� ��Ȱ��ȭ

                // �÷��̾ �Ҵ���� �ʾҴٸ�
                if (player == null)
                {
                    // �÷��̾� ����
                    player = Instantiate(player_Prefab);
                }

                player.transform.position = new Vector3(-20, -1, 0); // �÷��̾� ��ġ

                deathMenu.SetActive(false); // ���� Ŭ���� â ��Ȱ��ȭ
                adventureClearPanel.SetActive(false); // Ž�� ���� �г� ��Ȱ��ȭ

                // ���� �̹� �������� ������ �� ���� �ִ�� ������ �� ������ ũ�ٸ�
                if (currentDungeonFloor > highDungeonFloor)
                {
                    highDungeonFloor = currentDungeonFloor; // ������ �� ��� ����
                }

                // ��Ż �Ŵ����� �Ҵ���� �ʾҴٸ�
                if (pt == null)
                {
                    pt = GameObject.Find("Portal").GetComponent<Portal>(); // ��Ż �Ҵ�

                    StageMonsterCheck(); // ���ӸŴ����� �������� ���� üũ �Լ� ����
                }

                // �׾ �� ���̶��
                if (isDie)
                {
                    adventureFailPanel.SetActive(false); // Ž�� ���� �г� ��Ȱ��ȭ

                    currentDungeonFloor = 0; // ���� �� �ʱ�ȭ

                    // �κ��丮 ����
                    for (int i = 0; i < iv.slots.Length; i++)
                    {
                        // ���Կ� �������� �ִٸ�
                        if (iv.slots[i].item != null)
                        {
                            // ����
                            iv.slots[i].ClearSlot();
                        }
                    }

                    // ��� ���� â ������ ���� ��Ȱ��ȭ, ����
                    for (int i = 0; i < iv.deathMenuSlots.Length; i++)
                    {
                        // ������ Ȱ��ȭ �Ǿ��ִٸ�
                        if (iv.deathMenuSlots[i].gameObject.transform.parent.gameObject.activeSelf)
                        {
                            // ��Ȱ��ȭ
                            iv.deathMenuSlots[i].gameObject.transform.parent.gameObject.SetActive(false);

                            // ����
                            iv.deathMenuSlots[i].ClearSlot();
                        }
                    }

                    // ������ �ݶ�
                    gold = gold / 2;

                    // �÷��̾� ��Ȱ ó��
                    isDie = false;
                    PlayerManager.instance.anim.SetBool("isDeath", false);
                }

                // ü�� �ʱ�ȭ
                PlayerManager.instance.health = player_MaxHealth[level];

                // ���� �� ����Ʈ �ʱ�ȭ
                randomStageSceneList = stageSceneList.ToList(); // ���� �� �̸� ����Ʈ �ʱ�ȭ
                currentDungeonFloor = 0; // ���� �� �ʱ�ȭ

                town_Shop.Restock(); // ���� ���� ���԰�
                dungeon_Shop.RandomRestock(); // ���� ���� ���� ���԰�

                SoundManager.instance.PlayBackgroundMusic("Town"); // ���� BGM ���

                Save(); // ����
                break;

            case >= 4: // ���� �� ��ȣ : 4�̻�
                playerLocation = "����";
                player.transform.position = new Vector3(0, 0, 0); // �÷��̾� ��ġ
                locationText.text = $"{playerLocation} {currentDungeonFloor}��"; // �Ͻ����� �޴� ���� ��ġ �ؽ�Ʈ ������Ʈ
                isDropFairy = false; // �� �������� ����

                // ��Ż �Ŵ����� �Ҵ���� �ʾҴٸ�
                if (pt == null)
                {
                    pt = GameObject.Find("Portal").GetComponent<Portal>(); // ��Ż �Ҵ�

                    StageMonsterCheck(); // ���ӸŴ����� �������� ���� üũ �Լ� ����
                }

                // ���� ���� ���� ���� ���̶��
                if (SceneManager.GetActiveScene().name == "DungeonBoss")
                {
                    SoundManager.instance.StopBackgroundMusic(); // ������� ����
                }
                // ���� ���� ���̶��
                else if (SceneManager.GetActiveScene().name == "DungeonShop")
                {
                    SoundManager.instance.PlayBackgroundMusic("DungeonShop"); // ���� ���� BGM ���
                }
                // ���� �������� ���̶��
                else
                {
                    SoundManager.instance.PlayBackgroundMusic("Dungeon"); // ���� BGM ���

                    // �� ��������Ʈ�� �Ҵ���� �ʾҴٸ�
                    if (fairySpawnPoint == null)
                    {
                        fairySpawnPoint = GameObject.Find("FairySpawnPoint"); // �� ��������Ʈ �Ҵ�
                    }
                }
                break;
        }
    }

    // ���������� �ִ� ���� ���� üũ�ϴ� �Լ�
    public void StageMonsterCheck()
    {
        monsterInStage = GameObject.FindGameObjectsWithTag("Monster");
        monsterInStageList = new List<GameObject>(monsterInStage); // �迭�� ����Ʈ�� ����ȯ
    }

    // ��Ż ž�� �Լ�
    public void GetIntoPortal()
    {
        switch (pt.portal_ID)
        {
            // ��Ż ID�� 0�� �� (�������� ���� ��Ż)
            case 0:
                if (currentDungeonFloor >= 9) // 9������ ž���ߴٸ� ���� ���������� �̵�
                {
                    SetLoadScene("DungeonBoss"); // ���� ���������� �̵�
                    currentDungeonFloor++; // ���� �� �� �ö�
                }
                else
                {
                    int rand = Random.Range(0, randomStageSceneList.Count); // �ҷ��� �� �̸��� ���� �� ����Ʈ���� �������� �̱�
                    SetLoadScene(randomStageSceneList[rand]); // ���� ���� ���� ������ �̵�
                    randomStageSceneList.RemoveAt(rand); // �ѹ� ����� ���� ����Ʈ���� ����
                    currentDungeonFloor++; // ���� �� �� �ö�
                }
                break;

            // ��Ż ID�� 1�� �� (������ ���� ��Ż)
            case 1:
                tutorial_Complete = true; // Ʃ�丮�� �Ϸ�

                // �κ��丮 ����
                for (int i = 0; i < iv.slots.Length; i++)
                {
                    // ���Կ� �������� �ִٸ�
                    if (iv.slots[i].item != null)
                    {
                        // ����
                        iv.slots[i].ClearSlot();
                    }
                }

                SetLoadScene("Town"); // ������ �̵�
                break;

            // ��Ż ID�� 2�� �� (���� Ŭ���� ��Ż)
            case 2:
                dontMove = true; // �÷��̾� ������ ����

                SoundManager.instance.PlaySound("GameClear"); // ���� Ŭ���� ȿ���� ���
                // �κ��丮 �ݱ�
                activeInventoty = false;
                invectoryPanel.SetActive(activeInventoty);

                // ���� Ŭ���� â ���� ������Ʈ
                deathMenu_TimeText.text = (playDungeonTime / 60).ToString("00") + "�� " + (playDungeonTime % 60).ToString("00") + "��"; // �ð�
                deathMenu_LocationText.text = string.Format("{0} {1}��", playerLocation, currentDungeonFloor); // ��ġ
                deathMenu_GoldText.text = string.Format("{0:n0} G", gold); // ������
                goldPenaltyText.SetActive(false); // �г�Ƽ ����
                clearMenu_GameClearTitleText.text = "���� Ŭ����"; // ���� �ؽ�Ʈ ����
                deathMenu_DeathCauseText.text = "�����մϴ�. ������ Ŭ�����ϼ̽��ϴ�!"; // ���� Ŭ���� �ؽ�Ʈ
                clearMenu_ComentTitleText.text = "������ �ڸ�Ʈ"; // ���� �ؽ�Ʈ ����
                instance.deathMenu_NextExpText.text = "�÷������ּż� �����մϴ�. \n������ ��������ϴ�."; // ������ �ڸ�Ʈ

                // ȹ���ߴ� ������
                for (int i = 0; i < iv.slots.Length; i++)
                {
                    // �κ��丮 ���Կ� �������� �ִٸ�
                    if (iv.slots[i].item != null)
                    {
                        // ��� ���� â ���� �迭�� ������
                        for (int j = 0; j < iv.deathMenuSlots.Length; j++)
                        {
                            // ����ִ� ��� ���� â ������ ã�Ҵٸ�
                            if (iv.deathMenuSlots[j].item == null)
                            {
                                // ����ִ� ��� ���� â ���Կ� �κ��丮 ���� �������� �ְ� Ȱ��ȭ ��Ų��
                                iv.deathMenuSlots[j].AddItem(iv.slots[i].item);
                                iv.deathMenuSlots[j].gameObject.transform.parent.gameObject.SetActive(true);

                                break;
                            }
                        }
                    }
                }

                deathMenu.SetActive(true); // ���� Ŭ���� â Ȱ��ȭ
                adventureClearPanel.SetActive(true); // Ž�� ���� �г� Ȱ��ȭ

                deathMenu.GetComponent<Animator>().SetTrigger("Show"); // ��� ���� �г� �ִϸ��̼� ���
                adventureClearPanel.GetComponent<Animator>().SetTrigger("Show"); // Ž�� ���� �г� �ִϸ��̼� ���

                break;
        }
    }

    // ��ȭ �Լ�
    public void Talk(int id, string name)
    {
        string talkData = "";

        // ���� Ÿ������ ����ǰ� �ִٸ�
        if (talkText.isTyping)
        {
            // ��ȭ �ҷ����� �ʰ� ����
            talkText.SetMessage("");
            return;
        }
        // Ÿ������ �Ϸ�Ǿ��ٸ�
        else
        {
            talkData = TalkManager.instance.GetTalk(id, talkIndex); // ��ȭ �ҷ�����
        }

        choicePanel.SetActive(false); // ��ȭ ����â ��Ȱ��ȭ (�ʱ�ȭ)

        // ���� ��ȭ �ε����� ���޹��� ��ȭ ����â�� ���� �ε������
        if (talkIndex == currentChoiceTalkIndex)
        {
            isChoiceTalk = true; // ��ȭ ���� ��
        }
        else
        {
            isChoiceTalk = false; // ��ȭ ���� �� �ƴ�
        }

        if (talkData == null) // �ҷ��� ��ȭ�� ���ٸ�
        {
            talkPanel.SetActive(false); // ��ȭâ �ݱ�
            isTalk = false; // ��ȭ �� �ƴ�
            talkIndex = 0; // ��� ��ȣ �ʱ�ȭ
            return;
        }
        else
        {
            npcNameText.text = name; // �̸� �ؽ�Ʈ ������Ʈ
            talkPanel.SetActive(true); // ��ȭâ Ȱ��ȭ
            choicePanel.SetActive(false); // ��ȭ ����â ��Ȱ��ȭ (�ʱ�ȭ)
            talkText.SetMessage(talkData); // ��ȭ �ؽ�Ʈ ������Ʈ, Ÿ���� ����

            talkIndex++; // ��� ��ȣ + 1
        }
    }

    // �÷��̾� ������ �Լ�
    public void LevelUp()
    {
        // ������ 30���� ���� ��쿡�� ������
        if (level < maxLevel)
        {
            level++; // ���� + 1
            // ȿ����
        }
    }

    // ��Ȱ �Լ�
    public void Retry()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ������ �� ����
        SetLoadScene("Town"); // ������ �̵�
    }

    // ���� Ȱ��ȭ �Լ� (������ ������, ���� ��ġ)
    public void ShowTooltip(ItemData item, Transform transform)
    {
        tooltip_ItemDescription.text = item.itemDescription; // ������ ���� ����
        tooltip_ItemImage.sprite = item.itemImage; // ������ �̹��� ����

        // ��޿� ���� ������ �̸��� ���� ����
        switch (item.itemClass)
        {
            case ItemData.ItemClass.Common:
                tooltip_ItemName.text = $"<color=#FFFFFF>{item.itemName}</color>"; // �Ϲ�
                break;
            case ItemData.ItemClass.Rare:
                tooltip_ItemName.text = $"<color=#FF7500>{item.itemName}</color>"; // ����
                break;
            case ItemData.ItemClass.Epic:
                tooltip_ItemName.text = $"<color=#FF00AE>{item.itemName}</color>"; // ����
                break;
        }

        tooltipObject.transform.position = transform.position; // ���� ��ġ�� ���� ��ġ�� ����

        float halfWidth = hud_CanvasObject.GetComponent<CanvasScaler>().referenceResolution.x * 0.5f;
        float halfHeight = hud_CanvasObject.GetComponent<CanvasScaler>().referenceResolution.y * 0.5f;

        // ������ ȭ�� ������ ���� �� Ƽ�� ���� ( ������ �����ǰ� ����, ĵ���� �����Ϸ��� �̿��� �κ��丮�� �߷ȴ��� ��� )
        if ((tooltip_rt.anchoredPosition.y - tooltip_rt.sizeDelta.y < -halfHeight)
            && (tooltip_rt.anchoredPosition.x - tooltip_rt.sizeDelta.x < -halfWidth)) // �Ʒ�, ���� ��� �߸�
        {
            Debug.Log("���� �Ʒ�, ���� �߸�");
            tooltip_rt.pivot = new Vector2(0, 0); // ���� ��ġ -> ������ ��
        }
        else if (tooltip_rt.anchoredPosition.y - tooltip_rt.sizeDelta.y < -halfHeight) // �Ʒ��� �߸�
        {
            Debug.Log("���� �Ʒ� �߸�");
            tooltip_rt.pivot = new Vector2(1, 0); // ���� ��ġ -> ���� ��
        }
        else if (tooltip_rt.anchoredPosition.x - tooltip_rt.sizeDelta.x < -halfWidth) // ������ �߸�
        {
            Debug.Log("���� ���� �߸�");
            tooltip_rt.pivot = new Vector2(0, 1); // ���� ��ġ -> ������ �Ʒ�
        }
        else // ������
        {
            tooltip_rt.pivot = new Vector2(1, 1); // ���� ��ġ -> ���� �Ʒ�
        }

        // �ִ� ü�� �ؽ�Ʈ
        if (item.increase_Health != 0)
        {
            // �ִ� ü�� �������� ����� ��
            if (item.increase_Health > 0)
            {
                tooltip_IncreaseMaxHealthText.text = $"> <color=#24FF00>+{float.Parse(item.increase_Health.ToString("F1"))}</color> �ִ� ü��"; // �ִ� ü�� ������ �ؽ�Ʈ ���� (����)
            }
            // �ִ� ü�� �������� ������ ��
            else if (item.increase_Health < 0)
            {
                tooltip_IncreaseMaxHealthText.text = $"> <color=#FF4000>{float.Parse(item.increase_Health.ToString("F1"))}</color> �ִ� ü��"; // �ִ� ü�� ������ �ؽ�Ʈ ���� (����)
            }

            tooltip_IncreaseMaxHealthObject.SetActive(true); // �ִ� ü�� ������ ����
        }

        // ���ݷ� ������ �ؽ�Ʈ
        if (item.increase_Damage != 0)
        {
            // ���ݷ� �������� ����� ��
            if (item.increase_Damage > 0)
            {
                tooltip_IncreaseAttackDamageText.text = $"> <color=#24FF00>+{float.Parse(item.increase_Damage.ToString("F1"))}</color> ���ݷ�"; // ���ݷ� ������ �ؽ�Ʈ ���� (����)
            }
            // ���ݷ� �������� ������ ��
            else if (item.increase_Damage < 0)
            {
                tooltip_IncreaseAttackDamageText.text = $"> <color=#FF4000>{float.Parse(item.increase_Damage.ToString("F1"))}</color> ���ݷ�"; // ���ݷ� ������ �ؽ�Ʈ ���� (����)
            }

            tooltip_IncreaseAttackDamageObject.SetActive(true); // ���ݷ� ������ ����
        }

        // �̵� �ӵ� ������ �ؽ�Ʈ
        if (item.increase_MoveSpeed != 0)
        {
            // �������� ����� ��
            if (item.increase_MoveSpeed > 0)
            {
                tooltip_IncreaseMoveSpeedText.text = $"> <color=#24FF00>+{float.Parse(item.increase_MoveSpeed.ToString("F1"))}</color> �̵� �ӵ�"; // �̵� �ӵ� ������ �ؽ�Ʈ ���� (����)
            }
            // �������� ������ ��
            else if (item.increase_MoveSpeed < 0)
            {
                tooltip_IncreaseMoveSpeedText.text = $"> <color=#FF4000>{float.Parse(item.increase_MoveSpeed.ToString("F1"))}</color> �̵� �ӵ�"; // �̵� �ӵ� ������ �ؽ�Ʈ ���� (����)
            }

            tooltip_IncreaseMoveSpeedObject.SetActive(true); // �̵� �ӵ� ������ ����
        }

        // ������ ������ �ؽ�Ʈ
        if (item.increase_JumpPower != 0)
        {
            // �������� ����� ��
            if (item.increase_JumpPower > 0)
            {
                tooltip_IncreaseJumpPowerText.text = $"> <color=#24FF00>+{float.Parse(item.increase_JumpPower.ToString("F1"))}</color> ������"; // ������ ������ �ؽ�Ʈ ���� (����)
            }
            // �������� ������ ��
            else if (item.increase_JumpPower < 0)
            {
                tooltip_IncreaseJumpPowerText.text = $"> <color=#FF4000>{float.Parse(item.increase_JumpPower.ToString("F1"))}</color> ������"; // ������ ������ �ؽ�Ʈ ���� (����)
            }

            tooltip_IncreaseJumpPowerObject.SetActive(true); // ������ ������ �ݱ� ����
        }

        // ġ��Ÿ Ȯ�� ������ �ؽ�Ʈ
        if (item.increase_CriticalPercentage != 0)
        {
            // �������� ����� ��
            if (item.increase_CriticalPercentage > 0)
            {
                tooltip_IncreaseCriticalPercentageText.text = $"> <color=#24FF00>+{float.Parse((item.increase_CriticalPercentage * 100).ToString("F1"))}%</color> ġ��Ÿ Ȯ��"; // ġ��Ÿ Ȯ�� ������ �ؽ�Ʈ ���� (����)
            }
            // �������� ������ ��
            else if (item.increase_CriticalPercentage < 0)
            {
                tooltip_IncreaseCriticalPercentageText.text = $"> <color=#FF4000>{float.Parse((item.increase_CriticalPercentage * 100).ToString("F1"))}%</color> ġ��Ÿ Ȯ��"; // ġ��Ÿ Ȯ�� ������ �ؽ�Ʈ ���� (����)
            }

            tooltip_IncreaseCriticalPercentageObject.SetActive(true); // ġ��Ÿ Ȯ�� ������ ����
        }

        // ġ��Ÿ ��� ������ �ؽ�Ʈ
        if (item.increase_CriticalValue != 0)
        {
            // �������� ����� ��
            if (item.increase_CriticalValue > 0)
            {
                tooltip_IncreaseCriticalValueText.text = $"> <color=#24FF00>+{float.Parse(item.increase_CriticalValue.ToString("F1"))}</color> ġ��Ÿ ����"; // ġ��Ÿ ���� ������ �ؽ�Ʈ ���� (����)
            }
            // �������� ������ ��
            else if (item.increase_CriticalValue < 0)
            {
                tooltip_IncreaseCriticalValueText.text = $"> <color=#FF4000>{float.Parse(item.increase_CriticalValue.ToString("F1"))}</color> ġ��Ÿ ����"; // ġ��Ÿ ���� ������ �ؽ�Ʈ ���� (����)
            }

            tooltip_IncreaseCriticalValueObject.SetActive(true); // ġ��Ÿ ���� ������ ����
        }

        // ���� Ƚ�� ������ �ؽ�Ʈ
        if (item.increase_JumpCount != 0)
        {
            // �������� ����� ��
            if (item.increase_JumpCount > 0)
            {
                tooltip_IncreaseJumpCountText.text = $"> <color=#24FF00>+{float.Parse(item.increase_JumpCount.ToString("F1"))}</color> ���� Ƚ��"; // ���� Ƚ�� ������ �ؽ�Ʈ ���� (����)
            }
            // �������� ������ ��
            else if (item.increase_JumpCount < 0)
            {
                tooltip_IncreaseJumpCountText.text = $"> <color=#FF4000>{float.Parse(item.increase_JumpCount.ToString("F1"))}</color> ���� Ƚ��"; // ���� Ƚ�� ������ �ؽ�Ʈ ���� (����)
            }

            tooltip_IncreaseJumpCountObject.SetActive(true); // ���� Ƚ�� ������ ����
        }

        // �뽬 Ƚ�� ������ �ؽ�Ʈ
        if (item.increase_DashCount != 0)
        {
            // �������� ����� ��
            if (item.increase_DashCount > 0)
            {
                tooltip_IncreaseDashCountText.text = $"> <color=#24FF00>+{float.Parse(item.increase_DashCount.ToString("F1"))}</color> �뽬 Ƚ��"; // �뽬 Ƚ�� ������ �ؽ�Ʈ ���� (����)
            }
            // �������� ������ ��
            else if (item.increase_DashCount < 0)
            {
                tooltip_IncreaseDashCountText.text = $"> <color=#FF4000>{float.Parse(item.increase_DashCount.ToString("F1"))}</color> �뽬 Ƚ��"; // �뽬 Ƚ�� ������ �ؽ�Ʈ ���� (����)
            }

            tooltip_IncreaseDashCountObject.SetActive(true); // �뽬 Ƚ�� ������ ����
        }

        tooltipObject.SetActive(true); // ���� �г� ����
    }

    // ESC �޴� ��ư ���� �Լ�
    public void OpenInventory() // �κ��丮 ����
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ESC �޴� �ݱ�
        activeEscMenu = false;
        if (playerLocation == "����")
        {
            escMenuPanel.SetActive(activeEscMenu);
        }
        else
        {
            townEscMenuPanel.SetActive(activeEscMenu);
        }

        uiTooltipObject.SetActive(false); // UI ���� �ݱ�

        // �Ͻ����� ����
        Time.timeScale = 1f;

        // �κ��丮 ���� ����
        SoundManager.instance.PlaySound("OpenInventory");

        // �κ��丮 ����
        activeInventoty = true;
        invectoryPanel.SetActive(activeInventoty);
    }

    public void OpenOption() // ���� ����
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ���� ����
        optionMenuPanel.SetActive(true);
    }

    public void EscapeDungeon() // �������� ������
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ��¥ �������� �ǹ��� â ����
        realyEscapeDungeonPanel.SetActive(true);
    }

    public void EscapeYes() // �ǹ��� â���� '��'�� ������ �� ������ �Լ�
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // �ǹ��� â �ݱ�
        realyEscapeDungeonPanel.SetActive(false);

        // �Ͻ����� �޴� �ݱ�
        activeEscMenu = false;
        escMenuPanel.SetActive(activeEscMenu);

        // �Ͻ����� ����
        Time.timeScale = 1f;

        failCause = "���� ���⸦ ������ \n�������� �����ƴ�!";
        // �÷��̾� ��� ó��
        PlayerManager.instance.Die();
    }

    public void EscapeNo() // �ǹ��� â���� '�ƴϿ�'�� ������ �� ������ �Լ�
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // �ǹ��� â �ݱ�
        realyEscapeDungeonPanel.SetActive(false);
    }


    public void GoMain() // ����ȭ������ ������
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ��¥ �������� �ǹ��� â ����
        realyGoMainPanel.SetActive(true);
    }

    public void GoMainYes() // �ǹ��� â���� '��'�� ������ �� ������ �Լ�
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // �ǹ��� â �ݱ�
        realyGoMainPanel.SetActive(false);

        // �Ͻ����� �޴� �ݱ�
        activeEscMenu = false;
        escMenuPanel.SetActive(activeEscMenu); // ���� �Ͻ����� �޴�
        townEscMenuPanel.SetActive(activeEscMenu); // ���� �Ͻ����� �޴�

        // �Ͻ����� ����
        Time.timeScale = 1f;

        // BGM ��
        SoundManager.instance.StopBackgroundMusic();

        // ȭ�� ��ο����� �������� �̵�
        SetLoadScene("Main"); // �������� �̵�
    }

    public void GoMainNo() // �ǹ��� â���� '�ƴϿ�'�� ������ �� ������ �Լ�
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // �ǹ��� â �ݱ�
        realyGoMainPanel.SetActive(false);
    }


    // ���� ȭ�� �� ������ ���� �Լ�
    public void DisplayModePageUp() // ȭ�� ��� ������ �� �Լ�
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ������ ���������
        if (displayModePage >= displayModeList.Length - 1)
        {
            displayModePage = 0; // ó�� ��������
        }
        else
        {
            ++displayModePage; // ���� ��������
        }

        ChangeDisplayModePage(); // ȭ�� ��� ������ ������Ʈ
    }

    public void DisplayModePageDown() // ȭ�� ��� ������ �ٿ� �Լ�
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ó�� ���������
        if (displayModePage <= 0)
        {
            Debug.Log(displayModeList.Length);
            displayModePage = displayModeList.Length - 1; // ������ ��������
        }
        else
        {
            --displayModePage; // ���� ��������
        }

        ChangeDisplayModePage(); // ȭ�� ��� ������ ������Ʈ
    }
    public void ResolutionPageUp() // �ػ� ������ �� �Լ�
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ������ ���������
        if (resolutionPage >= resolutionList.Count -1)
        {
            resolutionPage = 0; // ó�� ��������
        }
        else
        {
            ++resolutionPage; // ���� ��������
        }

        ChangeResolutionPage(); // ȭ�� �ػ� ������ ������Ʈ
    }

    public void ResolutionPageDown() // �ػ� ������ �ٿ� �Լ�
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        // ó�� ���������
        if (resolutionPage <= 0)
        {
            resolutionPage = resolutionList.Count - 1; // ������ ��������
        }
        else
        {
            --resolutionPage; // ���� ��������
        }
        ChangeResolutionPage(); // ȭ�� �ػ� ������ ������Ʈ
    }

    // ȭ�� ��� ������ ������Ʈ
    void ChangeDisplayModePage()
    {
        if (displayModeList[displayModePage] == FullScreenMode.FullScreenWindow)
        {
            displayModeText.text = "��ü ȭ��";
        }
        else if (displayModeList[displayModePage] == FullScreenMode.Windowed)
        {
            displayModeText.text = "â ���";
        }
    }

    // ȭ�� �ػ� ������ ������Ʈ
    void ChangeResolutionPage()
    {
        resolutionText.text = $"{resolutionList[resolutionPage].width} X {resolutionList[resolutionPage].height}";
    }

    // ȭ�� ���� ����
    public void ApplyOption()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        Screen.SetResolution(resolutionList[resolutionPage].width, resolutionList[resolutionPage].height, displayModeList[displayModePage]); // ȭ�� ���� ����
    }

    // ���� ����
    public void GameStart()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        dataSlotSelectPanel.SetActive(true); // ������ ���� ���� �г� ����
    }

    // ���� ����
    public void QuitGame()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        Application.Quit(); // ����
    }

    // ������ ���� ���� ȭ�� �ݱ�
    public void CloseSelectDataSlotMenu()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        dataSlotSelectPanel.SetActive(false); // ������ ���� ���� �г� �ݱ�
    }

    // ���̵� ��
    IEnumerator FadeIn()
    {
        dontMove = true; // �÷��̾� ������ ����

        isFade = true; // ���� ���̵� �� üũ

        // ���̵� �г� Ȱ��ȭ
        fadePanel.gameObject.SetActive(true);

        // ���̵� �Ǵ� �ð�
        float time = 0;

        Color alpha = fadePanel.color;

        float volume = PlayerPrefs.GetFloat("BGM_Volume");

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / fadeTime;

            // ���̵� ��
            alpha.a = Mathf.Lerp(0, 1, time);

            // ���� ����
            fadePanel.color = alpha;

            yield return null;
        }

        canSceneLoad = true; // �� �ε��ؾ� �ϴ� ���� üũ

        yield return null;
    }

    // ���̵� �ƿ�
    IEnumerator FadeOut()
    {
        canSceneLoad = false; // �� �ε��ؾ� �ϴ� ���� �ƴ� üũ
        isLoadScene = false; // �� �ҷ����⸦ �ߴ��� üũ ����

        Color alpha = fadePanel.color;

        float volume = PlayerPrefs.GetFloat("BGM_Volume");

        // ���̵� �Ǵ� �ð�
        float time = 0;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / fadeTime;

            // ���̵� �ƿ�
            alpha.a = Mathf.Lerp(1, 0, time);

            // ���� ����
            fadePanel.color = alpha;

            yield return null;
        }

        isFade = false; // ���� ���̵� ���� üũ

        // ���̵� �г� ��Ȱ��ȭ
        fadePanel.gameObject.SetActive(false);

        dontMove = false; // �÷��̾� ������ ���� ����

        yield return null;
    }

    public void SetLoadScene(string sceneName)
    {
        // �ε��� �� �̸� ����
        loadSceneName = sceneName;

        // ���̵� ��
        StartCoroutine("FadeIn");
    }

    // ����
    public void Save()
    {
        DataManager.instance.nowPlayer.tutorial_Complete = tutorial_Complete; // Ʃ�丮�� �Ϸ� ����
        DataManager.instance.nowPlayer.gold = gold; // ������
        DataManager.instance.nowPlayer.exp = exp; // ����ġ
        DataManager.instance.nowPlayer.level = level; // ����
        DataManager.instance.nowPlayer.highFloor = highDungeonFloor; // ������ ��
        DataManager.instance.nowPlayer.playTime = playTime; // �÷��� �ð�

        DataManager.instance.SaveData();
    }

    // ������ ����
    public void ApplyData()
    {
        tutorial_Complete = DataManager.instance.nowPlayer.tutorial_Complete; // Ʃ�丮�� �Ϸ� ����
        gold = DataManager.instance.nowPlayer.gold; // ������
        exp = DataManager.instance.nowPlayer.exp; // ����ġ
        level = DataManager.instance.nowPlayer.level; // ����
        highDungeonFloor = DataManager.instance.nowPlayer.highFloor; // ������ ��
        playTime = DataManager.instance.nowPlayer.playTime; // �÷��� �ð�
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱����� �Ҵ��� ��������
    public PortalManager pt; // ��Ż�Ŵ��� ��ũ��Ʈ
    public PlayerManager pm; // �÷��̾� �Ŵ��� ��ũ��Ʈ
    public Inventory iv; // �κ��丮 ��ũ��Ʈ

    public ParticleSystem footDustParticle; // �÷��̾ �̵��� �� �𳯸��� ���� ��ƼŬ
    public ParticleSystem doubleJumpParticle; // �÷��̾ 2�� ������ �� ������ ��ƼŬ

    private GameObject player; // DontDestroy�� ����� �÷��̾� ������Ʈ
    private GameObject mainCamera; // DontDestroy�� ����� ����ī�޶� ������Ʈ
    private GameObject canvas; // DontDestroy�� ����� ĵ���� ������Ʈ

    private GameObject[] monsterInStage; // ���� ���������� �ִ� ���� �迭
    public List<GameObject> monsterInStageList; // ���� ���������� �ִ� ���� ����Ʈ

    private List<string> stageSceneList; // ����(��������) �� ����Ʈ

    // UI ����
    private GameObject goldTextObject; // ������ ��差 �ؽ�Ʈ ������Ʈ (���� ���)
    public TMP_Text goldText; // ������ ��差 �ؽ�Ʈ (���� ���)

    public GameObject invectoryPanel; // �κ��丮 ������Ʈ
    public GameObject tooltipObject; // ���� ������Ʈ
    public GameObject deathMenu; // ���� �޴� ������Ʈ
    public GameObject escMenuPanel; // �Ͻ����� �޴� ������Ʈ
    public GameObject optionMenuPanel; // ���� �޴� ������Ʈ
    public bool activeInventoty; // �κ��丮�� Ȱ�� ����
    public bool activeEscMenu; // �Ͻ����� �޴� Ȱ�� ����

    public GameObject mouseCursor; // ���콺 Ŀ�� ������Ʈ

    // ���� ����
    public List<Resolution> resolutionList = new List<Resolution>(); // �ػ� ����Ʈ
    public FullScreenMode[] displayModeList; // ȭ�� ���

    private int displayModePage; // ���� ȭ�� ��� ������
    private int resolutionPage; // ���� �ػ� ������
    public TMP_Text displayModeText; // ���� ȭ�� ��� �ؽ�Ʈ
    public TMP_Text resolutionText; // ���� �ػ� �ؽ�Ʈ

    // �÷��̾� ����
    public bool isDie; // �÷��̾ �׾�����

    public int level; // �÷��̾� ����
    public int maxLevel; // �÷��̾� �ִ� ����
    public int[] levelUp_exp; // �������� �ʿ��� ����ġ�� �迭

    public float gold; // ���
    public float exp; // ����ġ


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

        // Find �Լ��� ã�Ƽ� �Ҵ�
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        canvas = GameObject.Find("HUD_Canvas");
        goldTextObject = GameObject.Find("Gold_Text");

        // ������Ʈ �Ҵ�
        goldText = goldTextObject.GetComponent<TextMeshProUGUI>();

        DontDestroyOnLoad(pt); // �� ��ȯ �ÿ��� PortalManager�� �������� ����
        DontDestroyOnLoad(player); // �� ��ȯ �ÿ��� �÷��̾ �������� ����
        DontDestroyOnLoad(mainCamera); // �� ��ȯ �ÿ��� ����ī�޶� �������� ����
        DontDestroyOnLoad(canvas); // �� ��ȯ �ÿ��� ĵ������ �������� ����

        stageSceneList = new List<string> { "Dungeon01", "Dungeon02" }; // ���� �� �̸� ����Ʈ �ʱ�ȭ

        resolutionList.AddRange(Screen.resolutions); // ȭ�� �ػ� ����Ʈ �ʱ�ȭ
    }

    private void Start()
    {
        invectoryPanel.SetActive(false); // �κ��丮 �г� �ݱ�
    }

    void Update()
    {
        goldText.text = string.Format("{0:n0}", Mathf.Lerp(float.Parse(goldText.text), gold, Time.deltaTime * 60f));

        // ���������� ��� ���͸� ó���ߴٸ�
        if (monsterInStageList.Count == 0)
        {
            // ��Ż ����
            pt.isPortalOpen = true;
        }
        // ���������� ���Ͱ� �����ִٸ�
        else
        {
            // ��Ż �̰���
            pt.isPortalOpen = false;
        }

        // �޴� �Ǵ� �κ��丮�� Ȱ��ȭ �Ǿ��ְų� �׾��ٸ�
        if (activeInventoty || activeEscMenu || isDie)
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

        // �κ��丮�� �����ִµ� ������ Ȱ��ȭ ���ִٸ�
        if (!activeInventoty && tooltipObject.activeSelf)
        {
            tooltipObject.SetActive(false); // ���� �ݱ�
        }

        // �Ͻ����� �޴��� Ȱ��ȭ �Ǿ��ִٸ�
        if (activeEscMenu)
        {
            // ���� �Ͻ�����
            Time.timeScale = 0f;
        }
        // �ƴ϶��
        else
        {
            // ���� �Ͻ����� ����
            Time.timeScale = 1f;
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
        switch (pt.portal.portal_ID)
        {
            // ��Ż ID�� 0�� �� (�������� ���� ��Ż)
            case 0:
                int rand = Random.Range(0, stageSceneList.Count); // �ҷ��� �� �̸��� ���� �� ����Ʈ���� �������� �̱�
                SceneManager.LoadScene(stageSceneList[rand]); // ���� ���� ���� ������ �̵�
                stageSceneList.RemoveAt(rand); // �ѹ� ����� ���� ����Ʈ���� ����
                player.transform.position = new Vector3(0, 0, 0); // �÷��̾� ��ġ
                break;
            // ��Ż ID�� 1�� �� (������ ���� ��Ż)
            case 1:
                SceneManager.LoadScene("Town"); // ������ �̵�
                player.transform.position = new Vector3(-20, -1, 0); // �÷��̾� ��ġ

                stageSceneList = new List<string> { "Dungeon01", "Dungeon02" }; // ���� �� �̸� ����Ʈ �ʱ�ȭ
                break;
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
        // ���� �޴� �ݱ�
        deathMenu.SetActive(false);

        // ������ �� ����
        SceneManager.LoadScene("Town");
        player.transform.position = new Vector3(-20, -1, 0); // �÷��̾� ��ġ

        // �κ��丮 ����
        for (int i = 0; i < iv.slots.Length; i++)
        {
            // ���Կ� �������� �ִٸ�
            if (iv.slots[i].item != null)
            {
                iv.slots[i].ClearSlot();
            }

        }

        // ������ �ݶ�
        gold = gold / 2;

        // �÷��̾� ��Ȱ ó��
        isDie = false;
        pm.health = pm.maxHealth;
        pm.anim.SetBool("isDeath", false);

        // ���� �� ����Ʈ �ʱ�ȭ
        stageSceneList = new List<string> { "Dungeon01", "Dungeon02" }; // ���� �� �̸� ����Ʈ �ʱ�ȭ
    }

    // ���� ȭ�� �� ������ ���� �Լ�
    public void DisplayModePageUp() // ȭ�� ��� ������ �� �Լ�
    {
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
        Screen.SetResolution(resolutionList[resolutionPage].width, resolutionList[resolutionPage].height, displayModeList[displayModePage]); // ȭ�� ���� ����
    }
}

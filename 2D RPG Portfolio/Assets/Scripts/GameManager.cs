using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱����� �Ҵ��� ��������
    public PortalManager pm; // ��Ż�Ŵ���

    private GameObject player; // DontDestroy�� ����� �÷��̾� ������Ʈ
    private GameObject mainCamera; // DontDestroy�� ����� ����ī�޶� ������Ʈ
    private GameObject canvas; // DontDestroy�� ����� ĵ���� ������Ʈ

    private GameObject[] monsterInStage; // ���� ���������� �ִ� ���� �迭
    public List<GameObject> monsterInStageList; // ���� ���������� �ִ� ���� ����Ʈ

    private List<string> stageSceneList; // ����(��������) �� ����Ʈ

    // UI ����
    private GameObject goldTextObject; // ������ ��差 �ؽ�Ʈ ������Ʈ (���� ���)
    public TMP_Text goldText; // ������ ��差 �ؽ�Ʈ (���� ���)


    public bool isDie; // �÷��̾ �׾�����

    public float gold; // ���

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


        DontDestroyOnLoad(pm); // �� ��ȯ �ÿ��� PortalManager�� �������� ����
        DontDestroyOnLoad(player); // �� ��ȯ �ÿ��� �÷��̾ �������� ����
        DontDestroyOnLoad(mainCamera); // �� ��ȯ �ÿ��� ����ī�޶� �������� ����
        DontDestroyOnLoad(canvas); // �� ��ȯ �ÿ��� ĵ������ �������� ����

        stageSceneList = new List<string> { "Dungeon01", "Dungeon02" }; // ���� �� �̸� ����Ʈ �ʱ�ȭ
    }
    

    void Update()
    {
        //goldText.text = string.Format("{0:n0}", Mathf.SmoothStep(float.Parse(goldText.text), gold, 5f));
        goldText.text = string.Format("{0:n0}", Mathf.Lerp(float.Parse(goldText.text), gold, Time.deltaTime * 40f));

        // ���������� ��� ���͸� ó���ߴٸ�
        if (monsterInStageList.Count == 0)
        {
            // ��Ż ����
            pm.isPortalOpen = true;
        }
        // ���������� ���Ͱ� �����ִٸ�
        else
        {
            // ��Ż �̰���
            pm.isPortalOpen = false;
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
        switch (pm.portal.portal_ID)
        {
            // ��Ż ID�� 0�� �� (�������� ���� ��Ż)
            case 0:
                Debug.Log("�������� ������Ż ž��!");

                int rand = Random.Range(0, stageSceneList.Count); // �ҷ��� �� �̸��� ���� �� ����Ʈ���� �������� �̱�
                SceneManager.LoadScene(stageSceneList[rand]); // ���� ���� ���� ������ �̵�
                stageSceneList.RemoveAt(rand); // �ѹ� ����� ���� ����Ʈ���� ����
                player.transform.position = new Vector3(0, 0, 0); // �÷��̾� ��ġ
                break;
            // ��Ż ID�� 1�� �� (������ ���� ��Ż)
            case 1:
                Debug.Log("������ ������Ż ž��!");

                SceneManager.LoadScene("Town"); // ������ �̵�
                player.transform.position = new Vector3(-20, -1, 0); // �÷��̾� ��ġ

                stageSceneList = new List<string> { "Dungeon01", "Dungeon02" }; // ���� �� �̸� ����Ʈ �ʱ�ȭ
                break;
        }
    }
}

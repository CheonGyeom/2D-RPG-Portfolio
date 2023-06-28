using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance; // �̱����� �Ҵ��� ��������

    public Dictionary<int, string[]> talkData; // ��ȭ ������ ��ųʸ�< id, ��ȭ �迭 >

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
        DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� TalkManager�� �������� ����


        talkData = new Dictionary<int, string[]>();

        GenerateData();
    }

    void GenerateData()
    {
        // ���� ����
        talkData.Add(1000, new string[] { "�ݰ��� ��翩..", "�츮 ������ ���� �� �� ���־� ����.." });
        talkData.Add(1010, new string[] { "�� ������ �������� ���� ��� ���� �����̾���.. ���� ������� ��� ģ���߰� ������ �� ���� �������� ������ ����..",
            "�׷��� �Ϸ�� ������ ��ǳ�� �Ҿ� ū ���ذ� �־���.. �ǹ��� �������� ����� �����..", "���� ������� ������ �������� ���� �ƴϾ���.. �� ������ ��� ��� �;��ŵ�..", "�ᱹ ���� ����� ������� ������ ��� �������.. �� ���ķ� �� ���� ����鿡�� ���� ������ �� �ڸ����� �� �� �־�����.." });

        // �׺� ����
        talkData.Add(2000, new string[] { "���谡�� �ݰ���.", "���� �� ������ ������ �Ǵ�. �ѹ� ���� ����"});

        // ������
        talkData.Add(3000, new string[] { "�ȳ��ϼ��� ����! ������ ���͵帱���?"});

        // ��
        talkData.Add(4000, new string[] { "������: ���ذ�\n���۱Ⱓ: �� 3����\n\"���״� �����մϴ�.\" ��� �����ִ�." });
    }

    // ��ȭ �ҷ����� �Լ�
    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length) // ������ �����
        {
            return null; // ��� ����
        }
        else
        {
            return talkData[id][talkIndex]; // ��� ��ȯ
        }
    }

    // ��ȭ ���� - �̾߱� �ϱ� �Լ�
    public void choiceBtn_Talk()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        GameManager.instance.currentNpcId = GameManager.instance.currentNpcId + 10; // ��ȭ ID�� 10�� ���� �б⸦ ���� �ٸ� ��ȭ�� �ϵ��� �Ѵ�.
        GameManager.instance.talkIndex = 0; // ��� ��ȣ �ʱ�ȭ
        GameManager.instance.currentChoiceTalkIndex = -1; // ��ȭ ����â �ε��� ��ȣ �ʱ�ȭ

        GameManager.instance.Talk(GameManager.instance.currentNpcId, GameManager.instance.currentNpcName); // ��ȭ ����
    }

    // ��ȭ ���� - ���� ���� �Լ�
    public void choiceBtn_TownShop()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        GameManager.instance.townShopPanel.SetActive(true); // ���� ����

        // �κ��丮 Ȱ��ȭ
        GameManager.instance.activeInventoty = true;
        GameManager.instance.invectoryPanel.SetActive(GameManager.instance.activeInventoty);

        SoundManager.instance.PlaySound("OpenInventory"); // ȿ���� ���

        GameManager.instance.Talk(GameManager.instance.currentNpcId, GameManager.instance.currentNpcName); // ��ȭ ������ (���� ��簡 �����Ƿ�)
    }

    // ��ȭ ���� - ���� ���� �Լ�
    public void choiceBtn_DungeonShop()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        GameManager.instance.dungeonShopPanel.SetActive(true); // ���� ����

        // �κ��丮 Ȱ��ȭ
        GameManager.instance.activeInventoty = true;
        GameManager.instance.invectoryPanel.SetActive(GameManager.instance.activeInventoty);

        SoundManager.instance.PlaySound("OpenInventory"); // ȿ���� ���

        GameManager.instance.Talk(GameManager.instance.currentNpcId, GameManager.instance.currentNpcName); // ��ȭ ������ (���� ��簡 �����Ƿ�)
    }

    // ��ȭ ���� - �ƹ��͵� �Լ�
    public void choiceBtn_Nothing()
    {
        // ��ư Ŭ�� ����
        SoundManager.instance.PlaySound("ButtonClick");

        GameManager.instance.Talk(GameManager.instance.currentNpcId, GameManager.instance.currentNpcName); // ��ȭ ������ (���� ��簡 �����Ƿ�)
    }
}

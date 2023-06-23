using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public enum NpcType // NPC ��ȭ ����
    {
        common, // ��ȭ�� �������� �Ϲ����� ����
        Talk, // �̾߱� �ϱⰡ ������ ����
        Shop_Town, // ���� ���� �̿��� ������ ����
        shop_Dungeon // ���� ���� �̿��� ������ ����
    }

    public GameObject keyUI_F; // FŰ UI

    public int id; // NPC ID
    public string npcName; // NPC �̸�
    private bool isTrigger; // NPC ��ó�� �ִ��� üũ
    public int choiceTalkIndexNumber; // ��ȭ ����â�� ���� �ε��� ��ȣ

    public NpcType type; // NPC ��ȭ ����

    private void Update()
    {
        keyUI_F.SetActive(isTrigger); // �Ӹ� �� F��ư UI

        // NPC ��ó���� FŰ�� ������ ��, �ٸ� ��ȭ ���� �ƴϰ� �Ͻ����� �Ǵ� �κ��丮, ������ �������� �ʰ� ���� �ʾ��� ����
        if (Input.GetKeyDown(KeyCode.F) && isTrigger
            && !GameManager.instance.isTalk && !GameManager.instance.activeEscMenu && !GameManager.instance.activeInventoty && GameManager.instance.townShopPanel && GameManager.instance.townShopPanel && !GameManager.instance.isDie)
        {
            // ���� ��ȭ ���� NPC ���� �Ѱ��ֱ�
            GameManager.instance.currentNpcId = id;
            GameManager.instance.currentNpcName = npcName;
            GameManager.instance.currentChoiceTalkIndex = choiceTalkIndexNumber;

            // NPC ������ ���� ��������
            switch (type)
            {
                case NpcType.Talk: // �̾߱� �ϱⰡ ������ NPC
                    GameManager.instance.choiceBtn_Talk.SetActive(true);
                    GameManager.instance.choiceBtn_TownShop.SetActive(false);
                    GameManager.instance.choiceBtn_DungeonShop.SetActive(false);
                    break;
                case NpcType.Shop_Town: // ���� ���� �̿��� ������ NPC
                    GameManager.instance.choiceBtn_Talk.SetActive(false);
                    GameManager.instance.choiceBtn_TownShop.SetActive(true);
                    GameManager.instance.choiceBtn_DungeonShop.SetActive(false);
                    break;
                case NpcType.shop_Dungeon: // ���� ���� �̿��� ������ NPC
                    GameManager.instance.choiceBtn_Talk.SetActive(false);
                    GameManager.instance.choiceBtn_TownShop.SetActive(false);
                    GameManager.instance.choiceBtn_DungeonShop.SetActive(true);
                    break;
            }

            GameManager.instance.Talk(id, npcName); // ��ȭ �ҷ�����
            GameManager.instance.isTalk = true; // ��ȭ ��
        }
    }

    // Ʈ���Ű� ó�� �ߵ��� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            isTrigger = true; // Ʈ���� Ȱ��ȭ
        }
    }

    // Ʈ������ �ߵ��� ���� ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            isTrigger = false; // Ʈ���� Ȱ��ȭ
        }
    }
}

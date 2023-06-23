using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonShop : MonoBehaviour
{
    public ShopSlot[] shopslots;

    // ���� ������ ǰ�� ����Ʈ
    public List<GameObject> shopItemList;

    // ���� ������ ����Ʈ
    public List<ItemData> common_ItemList; // �Ϲ� ��� ������ ����Ʈ
    public List<ItemData> rare_ItemList; // ���� ��� ������ ����Ʈ
    public List<ItemData> epic_ItemList; // ���� ��� ������ ����Ʈ

    public List<ItemData> targetList; // ������ ������ �������� ���� ����Ʈ
    public List<ItemData> chosenItemList = new List<ItemData>(); // �̹� ������ ������ ��ϵ� ������ ����Ʈ (�ߺ� ������ ���� ����)

    public int minItemAmount; // ������ ������ �ּ� ������ ����
    public int maxItemAmount; // ������ ������ �ִ� ������ ����

    private int minItemPrice; // ���� ǰ���� �ּ� ����
    private int maxItemPrice; // ���� ǰ���� �ִ� ����

    public int minItemPrice_Common; // �Ϲ� �������� �ּ� ����
    public int maxItemPrice_Common; // �Ϲ� �������� �ִ� ����
    public int minItemPrice_Rare; // ���� �������� �ּ� ����
    public int maxItemPrice_Rare; // ���� �������� �ִ� ����
    public int minItemPrice_Epic; // ���� �������� �ּ� ����
    public int maxItemPrice_Epic; // ���� �������� �ִ� ����

    private bool isChosen; // �̹� ���� ���������� üũ

    // ���� ���԰� �Լ�
    public void RandomRestock()
    {
        // ���� ǰ�� �ʱ�ȭ (��Ȱ��ȭ)
        ResetShopItem();

        // �ߺ� ������ ����Ʈ ����
        chosenItemList.Clear();

        // ���� �Ǹ� ǰ�� �� �����ϰ� �̱�
        int rand_shopItemAmount = Random.Range(minItemAmount, maxItemAmount + 1);

        // ���� �Ǹ� ǰ�� �� ��ŭ ������ �̱�
        for (int i = 0; i < rand_shopItemAmount; i++)
        {
            float rand_ItemClass = Random.Range(0f, 1f);

            // ��޸��� �ٸ� Ȯ��
            if (rand_ItemClass < 0.6f) // �Ϲ� (60%) 
            {
                Debug.Log("�Ϲ� ���");
                targetList = common_ItemList; // Ÿ�� ����Ʈ�� �Ϲ� ��� ����
            }
            else if (rand_ItemClass < 0.95f) // ���� (35%) 
            {
                Debug.Log("���� ���");
                targetList = rare_ItemList; // Ÿ�� ����Ʈ�� ���� ��� ����
            }
            else if (rand_ItemClass < 1.0f) // ���� (5%) 
            {
                Debug.Log("���� ���");
                targetList = epic_ItemList; // Ÿ�� ����Ʈ�� ���� ��� ����
            }

            int rand_Item = Random.Range(0, targetList.Count); // Ÿ�� ��� ����Ʈ�� ũ�⺸�� ���� ���� ���ϱ�

            isChosen = false; // �ߺ� üũ �ʱ�ȭ

            // �ߺ� �˻�
            for (int k = 0; k < chosenItemList.Count; k++)
            {
                // ���� �������� �̹� �����ߴ� ������ ����Ʈ�� �ִٸ�
                if (targetList[rand_Item] == chosenItemList[k])
                {
                    isChosen = true; // �ߺ� ������ üũ
                    i--; // �������� �߰����� �������Ƿ�, �� �� �� �̵��� ��
                    break; // �ߺ� �˻� �ݺ��� Ż��
                }
            }

            // �ߺ� �������� �ƴ� ��
            if (!isChosen)
            {
                shopItemList[i].GetComponent<ShopSlot>().item = targetList[rand_Item]; // ���� ǰ�� �����ۿ� ���
                chosenItemList.Add(targetList[rand_Item]); // ���� ������ ����Ʈ�� ���

                // ��޸��� ������ ������ �ٸ�
                switch (targetList[rand_Item].itemClass)
                {
                    case ItemData.ItemClass.Common: // �Ϲ� ���
                        minItemPrice = minItemPrice_Common;
                        maxItemPrice = maxItemPrice_Common;
                        break;
                    case ItemData.ItemClass.Rare: // ���� ���
                        minItemPrice = minItemPrice_Rare;
                        maxItemPrice = maxItemPrice_Rare;
                        break;
                    case ItemData.ItemClass.Epic: // ���� ���
                        minItemPrice = minItemPrice_Epic;
                        maxItemPrice = maxItemPrice_Epic;
                        break;
                }

                int rand_Price = Random.Range(minItemPrice / 10, maxItemPrice / 10) * 10; // ���� ���� ���ϱ� - 10������ ���Ͽ� �� �ڸ����� ������ �ʰ� ��.
                shopslots[i].itemPrice = rand_Price;

                shopslots[i].ShopItemInfoEnter(); // ǰ�� ���� ���Է�

                shopItemList[i].SetActive(true); // ǰ�� Ȱ��ȭ
            }
        }
    }

    // ǰ�� ��Ȱ��ȭ �Լ�
    void ResetShopItem()
    {
        for (int i = 0; i < shopItemList.Count; i++)
        {
            shopItemList[i].SetActive(false);
        }
    }
}

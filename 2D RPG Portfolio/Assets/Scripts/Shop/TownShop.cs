using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownShop : MonoBehaviour
{
    public ShopSlot[] shopslots;

    public List<GameObject> shopItemList;

    // ���԰� �Լ�
    public void Restock()
    {
        // ��� ���� ������ ǰ�� Ȱ��ȭ
        for (int i = 0; i < shopItemList.Count; i++)
        {
            shopItemList[i].SetActive(true);
        }
    }
}

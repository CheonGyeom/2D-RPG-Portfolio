using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item; // �Ǹ��� ������
    public int itemPrice; // ������ ����

    public Image itemImage;  // �������� �̹���
    public TMP_Text itemNameText; // ������ �̸� �ؽ�Ʈ
    public TMP_Text itemPriceText; // ������ ���� �ؽ�Ʈ

    public Transform tooltipTransform; // ���� ��ġ

    private void Awake()
    {
        // ������ ���� �Է�
        ShopItemInfoEnter();
    }

    public void BuyItem()
    {
        // 1. ���� ������ ��
        if (GameManager.instance.gold < itemPrice)
        {
            // ���� ���� ȿ���� ���
            SoundManager.instance.PlaySound("CantBuyClick");
        }
        // 2. ���� ����� ��
        else
        {
            // ������ ȹ��
            for (int i = 0; i < GameManager.instance.iv.slots.Length; i++)
            {
                if (GameManager.instance.iv.slots[i].item == null)
                {
                    // �� ����
                    GameManager.instance.gold -= itemPrice;

                    // ���� ȿ���� ���
                    SoundManager.instance.PlaySound("HandleCoins");

                    GameManager.instance.iv.slots[i].AddItem(item);
                    Debug.Log($"{item.itemName}�������� �����ߴ�!");

                    // ���� ���� ��Ȱ��ȭ (�Ǹ� �Ϸ�) 
                    gameObject.SetActive(false);

                    CloseTooltip(); // ���� �ݱ�

                    return;
                }
            }

            Debug.Log("������ ���� ��");
            // ���� ���� ȿ���� ���
            SoundManager.instance.PlaySound("CantBuyClick");
        }
    }

    // ǰ�� ������ ���� �Է�
    public void ShopItemInfoEnter()
    {
        // �̹���
        itemImage.sprite = item.itemImage;

        // �̸�
        switch (item.itemClass) // ��޿� ���� ������ �̸��� ���� ����
        {
            case ItemData.ItemClass.Common:
                itemNameText.text = $"<color=#FFFFFF>{item.itemName}</color>"; // �Ϲ�
                break;
            case ItemData.ItemClass.Rare:
                itemNameText.text = $"<color=#FF7500>{item.itemName}</color>"; // ����
                break;
            case ItemData.ItemClass.Epic:
                itemNameText.text = $"<color=#FF00AE>{item.itemName}</color>"; // ����
                break;
        }

        // ����
        itemPriceText.text = itemPrice.ToString();
    }

    // �����Ͱ� ���� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.instance.ShowTooltip(item, tooltipTransform); // ���� Ȱ��ȭ
    }

    // �����Ͱ� ������ ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        CloseTooltip(); // ���� �ݱ�
    }

    // ���� �ݱ� �Լ�
    void CloseTooltip()
    {
        GameManager.instance.tooltipObject.SetActive(false); // ���� �г� �ݱ�

        GameManager.instance.tooltip_IncreaseMaxHealthObject.SetActive(false); // �ִ� ü�� ������ �ݱ�
        GameManager.instance.tooltip_IncreaseAttackDamageObject.SetActive(false); // ���ݷ� ������ �ݱ�
        GameManager.instance.tooltip_IncreaseMoveSpeedObject.SetActive(false); // �̵� �ӵ� ������ �ݱ�
        GameManager.instance.tooltip_IncreaseJumpPowerObject.SetActive(false); // ������ ������ �ݱ�
        GameManager.instance.tooltip_IncreaseCriticalPercentageObject.SetActive(false); // ũ��Ƽ�� Ȯ�� ������ �ݱ�
        GameManager.instance.tooltip_IncreaseCriticalValueObject.SetActive(false); // ũ��Ƽ�� ��� ������ �ݱ�
        GameManager.instance.tooltip_IncreaseJumpCountObject.SetActive(false); // ���� Ƚ�� ������ �ݱ�
        GameManager.instance.tooltip_IncreaseDashCountObject.SetActive(false); // �뽬 Ƚ�� ������ �ݱ�
    }
}

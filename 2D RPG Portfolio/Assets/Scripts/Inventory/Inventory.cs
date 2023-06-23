using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public Slot[] slots;
    public Slot[] deathMenuSlots;

    public TMP_Text goldText; // ��� �ؽ�Ʈ
    public TMP_Text getItemPanel_ItemName; // ������ ȹ�� �г� ������ �̸�
    public Image getItemPanel_ItemImage; // ������ ȹ�� �г� ������ �̹���
    public GameObject getItemPanel; // ������ ȹ�� �г� ������Ʈ

    // �÷��̾� ���� �ؽ�Ʈ
    public TMP_Text maxHealth_StateText; // �ִ� ü��
    public TMP_Text criticalPercentage_StateText; // ġ��Ÿ Ȯ��
    public TMP_Text attackDamage_StateText; // ���ݷ�
    public TMP_Text criticalValue_StateText; // ġ��Ÿ ���
    public TMP_Text moveSpeed_StateText; // �̵��ӵ�
    public TMP_Text dashCount_StateText; // �뽬 Ƚ��
    public TMP_Text jumpPower_StateText; // ������
    public TMP_Text jumpCount_StateText; // ���� Ƚ��

    private void Update()
    {
        // UI
        goldText.text = GameManager.instance.goldText.text; // ���ӸŴ����� ����ؽ�Ʈ�� ����ȭ

        UpdateStatus(); // �������ͽ� ������Ʈ
    }

    // ������ ȹ��
    public void AcquireItem(GameObject _item)
    {
        ItemData item = _item.gameObject.GetComponent<Item>().item;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item);

                Debug.Log($"{item.itemName}�������� �ֿ���!");

                // ������ ȹ�� �г� ����
                getItemPanel_ItemImage.sprite = item.itemImage; // ������ ȹ�� �г� ������ �̸� �ؽ�Ʈ ���� 
                getItemPanel.SetActive(item);

                // ��޿� ���� ������ �̸��� ���� ����
                switch (item.itemClass)
                {
                    case ItemData.ItemClass.Common:
                        getItemPanel_ItemName.text = $"<color=#FFFFFF>{item.itemName}</color>"; // �Ϲ�
                        break;
                    case ItemData.ItemClass.Rare:
                        getItemPanel_ItemName.text = $"<color=#FF7500>{item.itemName}</color>"; // ����
                        break;
                    case ItemData.ItemClass.Epic:
                        getItemPanel_ItemName.text = $"<color=#FF00AE>{item.itemName}</color>"; // ����
                        break;
                }

                // 2.5�� �� ������ �г� �ݱ�
                if (IsInvoking("DestroyGetItemPanel")) // �̹� ������ �г��� ����� �ִ� ���
                {
                    CancelInvoke("DestroyGetItemPanel"); // �κ�ũ �Լ� ���� ��� ��
                    Invoke("DestroyGetItemPanel", 2.5f); // �ٽ� ����
                }
                else
                {
                    Invoke("DestroyGetItemPanel", 2.5f); // �ƴ϶�� �׳� ����
                }

                _item.SetActive(false); // ������Ʈ Ǯ�� ������Ʈ ��ȯ
                return;
            }
        }
        Debug.Log("������ ���� ��");
    }

    // �������ͽ� UI ������Ʈ
    void UpdateStatus()
    {
        // �ִ� ü��
        if (GameManager.instance.increased_MaxHealth != 0) // �߰� ������ �ִٸ�
        {
            maxHealth_StateText.text = $"�ִ� ü�� {float.Parse((GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth).ToString("F1"))} <color=#FFA20D>+({float.Parse(GameManager.instance.increased_MaxHealth.ToString("F1"))})</color>"; // ���� �� ���� ǥ��
        }
        else
        {
            maxHealth_StateText.text = $"�ִ� ü�� {float.Parse((GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth).ToString("F1"))}";
        }

        // ġ��Ÿ Ȯ��
        if (Mathf.Abs(GameManager.instance.increased_CriticalPercentage * 100) > 0.1f) // �߰� ������ �ִٸ�
        {
            criticalPercentage_StateText.text = $"ġ��Ÿ Ȯ�� {float.Parse(((GameManager.instance.critical_Percentage + GameManager.instance.increased_CriticalPercentage) * 100).ToString("F1"))}% <color=#FFA20D>+({float.Parse((GameManager.instance.increased_CriticalPercentage * 100).ToString("F1"))})%</color>"; // ���� �� ���� ǥ��
        }
        else
        {
            criticalPercentage_StateText.text = $"ġ��Ÿ Ȯ�� {float.Parse(((GameManager.instance.critical_Percentage + GameManager.instance.increased_CriticalPercentage) * 100).ToString("F1"))}%";
        }

        // ���ݷ�
        if (GameManager.instance.increased_AttackDamage != 0) // �߰� ������ �ִٸ�
        {
            attackDamage_StateText.text = $"���ݷ� {GameManager.instance.player_AttackDamage[GameManager.instance.level] + GameManager.instance.increased_AttackDamage} <color=#FFA20D>+({GameManager.instance.increased_AttackDamage})</color>"; // ���� �� ���� ǥ��
        }
        else
        {
            attackDamage_StateText.text = $"���ݷ� {GameManager.instance.player_AttackDamage[GameManager.instance.level] + GameManager.instance.increased_AttackDamage}";
        }

        // ġ��Ÿ ����
        if (GameManager.instance.increased_CriticalValue != 0) // �߰� ������ �ִٸ�
        {
            criticalValue_StateText.text = $"ġ��Ÿ ���� x{float.Parse((GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue).ToString("F1"))}  <color=#FFA20D>+( {float.Parse(GameManager.instance.increased_CriticalValue.ToString("F1"))})</color>"; // ���� �� ���� ǥ��
        }
        else
        {
            criticalValue_StateText.text = $"ġ��Ÿ ���� x{float.Parse((GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue).ToString("F1"))}";
        }

        // �̵� �ӵ�
        if (GameManager.instance.increased_MoveSpeed != 0) // �߰� ������ �ִٸ�
        {
            moveSpeed_StateText.text = $"�̵� �ӵ� {float.Parse((GameManager.instance.player_MoveSpeed + GameManager.instance.increased_MoveSpeed).ToString("F1"))} <color=#FFA20D>+({float.Parse(GameManager.instance.increased_MoveSpeed.ToString("F1"))})</color>"; // ���� �� ���� ǥ��
        }
        else
        {
            moveSpeed_StateText.text = $"�̵� �ӵ� {float.Parse((GameManager.instance.player_MoveSpeed + GameManager.instance.increased_MoveSpeed).ToString("F1"))}";
        }

        // �뽬 Ƚ�� 
        if (GameManager.instance.increased_MaxDashCount != 0) // �߰� ������ �ִٸ�
        {
            dashCount_StateText.text = $"�뽬 Ƚ�� {GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount}ȸ <color=#FFA20D>+({GameManager.instance.increased_MaxDashCount})</color>"; // ���� �� ���� ǥ��
        }
        else
        {
            dashCount_StateText.text = $"�뽬 Ƚ�� {GameManager.instance.maxDashChargeCount + GameManager.instance.increased_MaxDashCount}ȸ";
        }

        // ������
        if (GameManager.instance.increased_JumpPower != 0) // �߰� ������ �ִٸ�
        {
            jumpPower_StateText.text = $"������ {float.Parse((GameManager.instance.jumpPower + GameManager.instance.increased_JumpPower).ToString("F1"))} <color=#FFA20D>+({float.Parse(GameManager.instance.increased_JumpPower.ToString("F1"))})</color>"; // ���� �� ���� ǥ��
        }
        else
        {
            jumpPower_StateText.text = $"������ {float.Parse((GameManager.instance.jumpPower + GameManager.instance.increased_JumpPower).ToString("F1"))}";
        }

        // ���� Ƚ��
        if (GameManager.instance.increased_MaxJump != 0) // �߰� ������ �ִٸ�
        {
            jumpCount_StateText.text = $"���� Ƚ�� {GameManager.instance.maxJump + GameManager.instance.increased_MaxJump}ȸ <color=#FFA20D>+({GameManager.instance.increased_MaxJump})</color>"; // ���� �� ���� ǥ��
        }
        else
        {
            jumpCount_StateText.text = $"���� Ƚ�� {GameManager.instance.maxJump + GameManager.instance.increased_MaxJump}ȸ";
        }
    }


    // ������ ȹ�� �г� ����
    private void DestroyGetItemPanel()
    {
        getItemPanel.SetActive(false);
    }
}


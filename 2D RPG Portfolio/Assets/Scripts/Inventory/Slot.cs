using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item; // ȹ���� ������
    public Image itemImage;  // �������� �̹���

    public bool isEquipmentSlot; // �ش� ������ ��� ���� �������� ����
    public Transform playerTr; // �÷��̾� ��ġ
    public PlayerManager pm; // �÷��̾� �Ŵ���

    private Rect inventoryRect; // �κ��丮 �̹����� Rect ����

    // ����
    private RectTransform tooltip_rt; // ���� ��Ʈ Ʈ������

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

    // �巡�� �� ���
    [SerializeField]
    private Transform _targetTr; // �̵��� UI

    private void Awake()
    {
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        inventoryRect = transform.parent.parent.parent.parent.GetComponent<RectTransform>().rect;

        tooltip_IncreaseMaxHealthText = tooltip_IncreaseMaxHealthObject.GetComponent<TextMeshProUGUI>();
        tooltip_IncreaseAttackDamageText = tooltip_IncreaseAttackDamageObject.GetComponent<TextMeshProUGUI>();
        tooltip_IncreaseMoveSpeedText = tooltip_IncreaseMoveSpeedObject.GetComponent<TextMeshProUGUI>();
        tooltip_IncreaseJumpPowerText = tooltip_IncreaseJumpPowerObject.GetComponent<TextMeshProUGUI>();
        tooltip_IncreaseCriticalPercentageText = tooltip_IncreaseCriticalPercentageObject.GetComponent<TextMeshProUGUI>();
        tooltip_IncreaseCriticalValueText = tooltip_IncreaseCriticalValueObject.GetComponent<TextMeshProUGUI>();
        tooltip_IncreaseJumpCountText = tooltip_IncreaseJumpCountObject.GetComponent<TextMeshProUGUI>();
        tooltip_IncreaseDashCountText = tooltip_IncreaseDashCountObject.GetComponent<TextMeshProUGUI>();

        // �̵� ��� UI�� �������� ���� ���, �ڵ����� �ڽ����� �ʱ�ȭ
        if (_targetTr == null)
        {
            _targetTr = this.transform;
        }
    }

    private void Start()
    {
        tooltip_rt = GameManager.instance.tooltipObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // ������ �̹����� ��Ⱦ�� ���� ������ �����ִٸ�
        if (!itemImage.preserveAspect)
        {
            itemImage.preserveAspect = true; // �ѱ�
        }
    }

    // ������ �̹����� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // �κ��丮�� ���ο� ������ ���� �߰�
    public void AddItem(ItemData _item)
    {
        item = _item;
        itemImage.sprite = item.itemImage;

        if (isEquipmentSlot)
        {
            // ��� �ɷ�ġ Up
            EquipItem();
        }
    
        SetColor(1);
    }

    // �ش� ���� �ϳ� ����
    public void ClearSlot()
    {
        if (isEquipmentSlot)
        {
            // ��� �ɷ�ġ ����
            UnequipItem();
        }

        item = null;
        itemImage.sprite = null;
        SetColor(0);
    }

    // ������ ����
    private void EquipItem()
    {
        GameManager.instance.increased_MaxHealth += item.increase_Health; // �ִ� ü�� ����
        pm.health += item.increase_Health; // �ִ� ü�� �����ϸ鼭 ü�µ� ���� ����
        GameManager.instance.increased_CriticalPercentage += item.increase_CriticalPercentage; // ġ��Ÿ Ȯ�� ����
        GameManager.instance.increased_AttackDamage += item.increase_Damage; // ���ݷ� ����
        GameManager.instance.increased_CriticalValue += item.increase_CriticalValue; // ġ��Ÿ ��� ����
        GameManager.instance.increased_MoveSpeed += item.increase_MoveSpeed; // �̵� �ӵ� ����
        GameManager.instance.increased_MaxDashCount += item.increase_DashCount; // �뽬 Ƚ�� ����
        pm.dashChargeCount += item.increase_DashCount; // �뽬 Ƚ�� �����ϸ鼭 �뽬 �������� ���� ����
        GameManager.instance.increased_JumpPower += item.increase_JumpPower; // ������ ����
        GameManager.instance.increased_MaxJump += item.increase_JumpCount; // ���� Ƚ�� ����
    }

    // ������ ���� ����
    private void UnequipItem()
    {
        GameManager.instance.increased_MaxHealth -= item.increase_Health; // �ִ� ü�� ����
        pm.health -= item.increase_Health; // �ִ� ü�� �����ϸ鼭 ü�µ� ���� ����
        GameManager.instance.increased_CriticalPercentage -= item.increase_CriticalPercentage; // ġ��Ÿ Ȯ�� ����
        GameManager.instance.increased_AttackDamage -= item.increase_Damage; // ���ݷ� ����
        GameManager.instance.increased_CriticalValue -= item.increase_CriticalValue; // ġ��Ÿ ��� ����
        GameManager.instance.increased_MoveSpeed -= item.increase_MoveSpeed; // �̵� �ӵ� ����
        GameManager.instance.increased_MaxDashCount -= item.increase_DashCount; // �뽬 Ƚ�� ����
        GameManager.instance.increased_JumpPower -= item.increase_JumpPower; // ������ ����
        GameManager.instance.increased_MaxJump -= item.increase_JumpCount; // ���� Ƚ�� ����
    }

    // ���콺 �巡�װ� ���� ���� �� �߻��ϴ� �̺�Ʈ
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���Կ� �������� ���� ����
        if (item != null)
        {
            SetColor(0); // ���� ������ �� ���̰�

            DragSlot.instance.dragSlot = this; // �巡�� ���Կ� ����
            DragSlot.instance.DragSetImage(itemImage); // �巡�� ���Կ� �̹��� ����
            DragSlot.instance.transform.position = eventData.position; // �巡�� ���� ��ġ ����
        }
    }

    // ���콺 �巡�� ���� �� ��� �߻��ϴ� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position; // �̵�
        }
           
    }

    // ���콺 �巡�װ� ������ �� �߻��ϴ� �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            SetColor(1); // ���� ������ ���̰�

            // �巡�� ������ ��ġ(���콺 Ŀ���� ��ġ)�� �κ��丮 �ۿ� ���� ��
            if (DragSlot.instance.transform.localPosition.x < inventoryRect.xMin)
            {
                GameObject throwItem = ObjectPoolingManager.instance.GetObject(DragSlot.instance.dragSlot.item.objectPoolName); // ������ �ʵ忡 ����

                Debug.Log("������ ����");
                DragSlot.instance.dragSlot.ClearSlot(); // ������ ����

                throwItem.transform.position = playerTr.position; // �÷��̾� ��ġ�� �̵�
            }

            DragSlot.instance.SetColor(0); // �巡�� ���� �� ���̰�
            DragSlot.instance.dragSlot = null; // �巡�� ���� ���� ����
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            DragSlot.instance.SetColor(0); // �巡�� ���� �� ���̰�
            ChangeSlot();
        }
            
    }

    // ������ �̵��� ��ȯ
    private void ChangeSlot()
    {
        ItemData _tempItem = item; // ���� �������� �ӽ� ������ ����

        AddItem(DragSlot.instance.dragSlot.item); // �ڽ��� ���Կ� �巡�� �� �������� ����

        // �ڽ��� �������� ������ �ִ� ��Ȳ�̾��ٸ�
        if (_tempItem != null)
        {
            // ������ �ٲٱ�

            // �巡�� ���Կ� �ڽ��� �������� ����
            DragSlot.instance.dragSlot.AddItem(_tempItem);

            if (isEquipmentSlot)
            {
                // ���� ���� ��� ���� ����
                DragSlot.instance.dragSlot.UnequipItem();
            }
            
            if (DragSlot.instance.dragSlot.isEquipmentSlot)
            {
                // �巡�� �� ���� ��� ���� ����
                UnequipItem();
            }
        }
        else
        {
            // ������ �̵�
            DragSlot.instance.dragSlot.ClearSlot(); // �巡�� �� ������ ���
        }        
    }

    // �����Ͱ� ���� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ������ ������� ���� ����
        if (item != null)
        {
            ShowTooltip(); // ���� Ȱ��ȭ
        }
    }

    // �����Ͱ� ������ ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.tooltipObject.SetActive(false); // ���� �г� �ݱ�

        tooltip_IncreaseMaxHealthObject.SetActive(false); // �ִ� ü�� ������ �ݱ�
        tooltip_IncreaseAttackDamageObject.SetActive(false); // ���ݷ� ������ �ݱ�
        tooltip_IncreaseMoveSpeedObject.SetActive(false); // �̵� �ӵ� ������ �ݱ�
        tooltip_IncreaseJumpPowerObject.SetActive(false); // ������ ������ �ݱ�
        tooltip_IncreaseCriticalPercentageObject.SetActive(false); // ũ��Ƽ�� Ȯ�� ������ �ݱ�
        tooltip_IncreaseCriticalValueObject.SetActive(false); // ũ��Ƽ�� ��� ������ �ݱ�
        tooltip_IncreaseJumpCountObject.SetActive(false); // ���� Ƚ�� ������ �ݱ�
        tooltip_IncreaseDashCountObject.SetActive(false); // �뽬 Ƚ�� ������ �ݱ�
    }

    // ���� Ȱ��ȭ �Լ�
    public void ShowTooltip()
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

        GameManager.instance.tooltipObject.transform.position = transform.position; // ���� ��ġ�� ���� ��ġ�� ����

        // ������ ȭ�� ������ ���� �� Ƽ�� ����
        if (Mathf.Abs(tooltip_rt.anchoredPosition.y) + Mathf.Abs(tooltip_rt.sizeDelta.y) > Mathf.Abs(inventoryRect.yMin))
        {
            Debug.Log("���� ��������");
            tooltip_rt.pivot = new Vector2(1, 0);
        }
        else
        {
            tooltip_rt.pivot = new Vector2(1, 1);
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

        GameManager.instance.tooltipObject.SetActive(true); // ���� �г� ����
    }
}

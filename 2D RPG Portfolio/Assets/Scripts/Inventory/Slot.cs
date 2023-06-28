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
    public bool isDeathMenuUISlot; // �ش� ������ ��� ���� â �������� ����

    // �巡�� �� ���
    [SerializeField]
    private Transform _targetTr; // �̵��� UI

    private void Awake()
    {
        // �̵� ��� UI�� �������� ���� ���, �ڵ����� �ڽ����� �ʱ�ȭ
        if (_targetTr == null)
        {
            _targetTr = this.transform;
        }
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
        Debug.Log($"���� �� ü��: {PlayerManager.instance.health} " +
            $"\n���� �� �ִ�ü��: {GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth} " +
            $"\n ���� �� ü��: {Mathf.Round((PlayerManager.instance.health * (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth + item.increase_Health)) / (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth))} " +
            $"\n ���� �� �ִ�ü��: {(GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth + item.increase_Health)}");

        // ������ ���� �� ü�� ���ϱ�
        PlayerManager.instance.health = Mathf.Round((PlayerManager.instance.health * (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth + item.increase_Health))
            / (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth));

        GameManager.instance.increased_MaxHealth += item.increase_Health; // �ִ� ü�� ����
        GameManager.instance.increased_CriticalPercentage += item.increase_CriticalPercentage; // ġ��Ÿ Ȯ�� ����
        GameManager.instance.increased_AttackDamage += item.increase_Damage; // ���ݷ� ����
        GameManager.instance.increased_CriticalValue += item.increase_CriticalValue; // ġ��Ÿ ��� ����
        GameManager.instance.increased_MoveSpeed += item.increase_MoveSpeed; // �̵� �ӵ� ����
        GameManager.instance.increased_MaxDashCount += item.increase_DashCount; // �뽬 Ƚ�� ����
        PlayerManager.instance.dashChargeCount += item.increase_DashCount; // �뽬 Ƚ�� �����ϸ鼭 �뽬 �������� ���� ����
        GameManager.instance.increased_JumpPower += item.increase_JumpPower; // ������ ����
        GameManager.instance.increased_MaxJump += item.increase_JumpCount; // ���� Ƚ�� ����
    }

    // ������ ���� ����
    private void UnequipItem()
    {
        Debug.Log($"���� �� ü��: {PlayerManager.instance.health} " +
            $"\n���� �� �ִ�ü��: {GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth} " +
            $"\n ���� �� ü��: {Mathf.Round((PlayerManager.instance.health * (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth - item.increase_Health)) / (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth))} " +
            $"\n ���� �� �ִ�ü��: {(GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth - item.increase_Health)}");

        // ������ ���� ���� �� ü�� ���ϱ�
        PlayerManager.instance.health = Mathf.Round((PlayerManager.instance.health * (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth - item.increase_Health))
            / (GameManager.instance.player_MaxHealth[GameManager.instance.level] + GameManager.instance.increased_MaxHealth));

        GameManager.instance.increased_MaxHealth -= item.increase_Health; // �ִ� ü�� ����
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
        // ���Կ� �������� �ְ� ��� ���� â ������ �ƴ� ����
        if (item != null && !isDeathMenuUISlot)
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
        if (item != null && !isDeathMenuUISlot)
        {
            DragSlot.instance.transform.position = eventData.position; // �̵�
        }
           
    }

    // ���콺 �巡�װ� ������ �� �߻��ϴ� �̺�Ʈ
    public void OnEndDrag(PointerEventData eventData)
    {
        if (item != null && !isDeathMenuUISlot)
        {
            SetColor(1); // ���� ������ ���̰�

            // �巡�� ������ ��ġ(���콺 Ŀ���� ��ġ)�� �κ��丮 �ۿ� ���� ��
            if (DragSlot.instance.transform.localPosition.x < GameManager.instance.invectoryPanel.GetComponent<RectTransform>().rect.xMin)
            {
                GameObject throwItem = ObjectPoolingManager.instance.GetObject(DragSlot.instance.dragSlot.item.objectPoolName); // ������ �ʵ忡 ����

                Debug.Log("������ ����");
                DragSlot.instance.dragSlot.ClearSlot(); // ������ ����

                throwItem.transform.position = GameManager.instance.player.transform.position; // �÷��̾� ��ġ�� �̵�
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

            // ��� ���� ȿ���� ���
            if (isEquipmentSlot)
            {
                SoundManager.instance.PlaySound("EquipItem");
            }
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
        // ���Կ� �������� ���� ���� ����
        if (item != null)
        {
            GameManager.instance.ShowTooltip(item, transform); // ���� Ȱ��ȭ
        }
    }

    // �����Ͱ� ������ ȣ��
    public void OnPointerExit(PointerEventData eventData)
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

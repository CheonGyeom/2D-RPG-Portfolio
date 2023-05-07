using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class ItemData : ScriptableObject
{
    public enum ItemClass
    {
        Common,
        Rare,
        Epic,
    }

    public string itemName; // ������ �̸�
    public string itemDescription; // ������ ����
    public ItemClass itemClass; // ������ ���
    public Sprite itemImage; // ������ �̹���
    public string objectPoolName; // ������Ʈ Ǯ�� ��� �� ������ �̸�

    public float increase_Health; // ü�� ������
    public float increase_CriticalPercentage; // ġ��Ÿ Ȯ�� ������
    public float increase_CriticalValue; // ġ��Ÿ ��� ������
    public float increase_MoveSpeed; // �̵� �ӵ� ������
    public int increase_Damage; // ����� ������
    public float increase_JumpPower; // ������ ������
    public int increase_DashCount; // �뽬 Ƚ�� ������
    public int increase_JumpCount; // ���� Ƚ�� ������
}

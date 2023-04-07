using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManager : EntityManager
{
    public float followDistance; // �÷��̾ �����ϴ� �Ÿ�
    public float attackDistance; // �÷��̾ �����ϴ� �Ÿ�

    public float nextThinkTime; // ���� �ൿ�� �����ϱ���� �ɸ��� �ð�
    public float thinkTime; // �ൿ�� �����ϰ���� ��������� �ð�
    public int nextMove; // ������ �̵� ���� (-1, 0, 1)
    public bool isAttack; // ���� ������ üũ
    public bool isDie; // ���Ͱ� �׾����� üũ

    // HP �� ����
    public GameObject hp_Bar; // HP �� ���ӿ�����Ʈ
    public GameObject hp_fill_GameObject; // HP �� fill ���� ���ӿ�����Ʈ
    public GameObject hp_fill_Lerp_GameObject; // HP �� �ε巯�� fill ���� ���ӿ�����Ʈ
    public Image hp_fill; // HP �� fill
    public Image hp_fill_Lerp; // HP �� �ε巯�� fill

    // ��� ������ ����
    public int minItemDrop; // �ּ� ��� ������ ����
    public int maxItemDrop; // �ִ� ��� ������ ����

    public override void Die()
    {
        base.Die();

        isDie = true; // ���� ���

        // ���ӸŴ����� ���� �������� ���� ����Ʈ���� ������ ���� ����
        GameManager.instance.monsterInStageList.RemoveAt(GameManager.instance.monsterInStageList.Count - 1);
    }
    public virtual void MonsterAI() { }
}


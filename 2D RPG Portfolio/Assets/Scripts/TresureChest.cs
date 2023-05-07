using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TresureChest : MonoBehaviour
{
    public Animator anim; // �������� �ִϸ�����

    public GameObject keyUI_F; // FŰ UI

    // ���� ������ ����Ʈ
    public List<ItemData> common_ItemList; // �Ϲ� ��� ������ ����Ʈ
    public List<ItemData> rare_ItemList; // ���� ��� ������ ����Ʈ
    public List<ItemData> epic_ItemList; // ���� ��� ������ ����Ʈ

    public List<ItemData> targetList; // ��� �� ������ ����Ʈ

    private bool isOpen; // ���� ���� ����

    private void Update()
    {
        // Ʈ���� �ߵ� �߿� ���ڰ� ������ �ʾ��� ��
        if (keyUI_F.activeSelf && !isOpen)
        {
            // FŰ�� ������
            if (Input.GetKeyDown(KeyCode.F))
            {
                ChestOpen(); // ���� ����
            }
        }
    }

    // Ʈ���Ű� ó�� �ߵ��� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� ���ڰ� ������ �ʾ��� ��
        if (collision.gameObject.CompareTag("Player") && !isOpen)
        {
            keyUI_F.SetActive(true);
        }
    }

    // Ʈ������ �ߵ��� ���� ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰ų� ���ڰ� ������ ��
        if (collision.gameObject.CompareTag("Player") || isOpen)
        {
            keyUI_F.SetActive(false);
        }
    }

    // ���� ���� �Լ�
    private void ChestOpen()
    {
        isOpen = true; // ���� ���� ó��

        keyUI_F.SetActive(false);

        anim.SetBool("isOpen", true); // ���� ���� �ִϸ��̼� ���

        float rand_ItemClass = Random.Range(0f, 1f);

        if (rand_ItemClass < 0.6f) // (60%) 
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

        Debug.Log(rand_Item);

        GameObject dropItem = ObjectPoolingManager.instance.GetObject(targetList[rand_Item].objectPoolName);
        dropItem.transform.position = transform.position; // ��ġ ����
    }
}

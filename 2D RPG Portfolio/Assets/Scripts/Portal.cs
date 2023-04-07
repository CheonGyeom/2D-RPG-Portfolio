using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    PortalManager pm;
    private GameObject pmObject; // ��Ż�Ŵ����� �Ҵ��� ������Ʈ
    public int portal_ID; // � ��Ż���� ��Ÿ���� ID

    private void Awake()
    {
        // ��Ż�Ŵ��� �Ҵ�
        pmObject = GameObject.Find("PortalManager");
        pm = pmObject.GetComponent<PortalManager>();

        // ���� �ʱ�ȭ
        pm.isTrigger = false;
    }
    // Ʈ���Ű� ó�� �ߵ��� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ʈ���� �ߵ�
            pm.isTrigger = true;
        }
    }

    // Ʈ������ �ߵ��� ���� ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ʈ���� ����
            pm.isTrigger = false;
        }
    }
}

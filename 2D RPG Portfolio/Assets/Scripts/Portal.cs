using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int portal_ID; // � ��Ż���� ��Ÿ���� ID

    public Animator anim; // ��Ż �ִϸ�����
    public GameObject keyUI_F; // FŰ UI

    public bool isPortalOpen; // ��Ż�� ���� �������� üũ
    public bool isTrigger; // ��Ż Ʈ���Ű� �ߵ� ���ΰ� (��Ż ��ó�ΰ�)

    private void Awake()
    {
        // ���� �ʱ�ȭ
        isTrigger = false;
    }

    private void Update()
    {
        // ��Ż ����
        if (isPortalOpen) // ��Ż�� ���ȴٸ�
        {
            // ��Ż �ִϸ��̼� �۵�
            anim.SetBool("OpenPortal", isPortalOpen);

            // ��Ż �� F��ư UI
            keyUI_F.SetActive(isTrigger);

            // ��Ż ž��
            if (isTrigger && anim.GetCurrentAnimatorStateInfo(0).IsName("Open") && !GameManager.instance.dontMove) // ��Ż Ʈ���Ÿ� �ߵ� ���̰�, ��Ż ���� �ִϸ��̼��� ���� ���̰� ������ ���� ���� �ƴ� ��
            {
                // FŰ�� ������
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // ���ӸŴ����� ��Ż ž�� �Լ� ����
                    GameManager.instance.GetIntoPortal();

                    keyUI_F.SetActive(false); // ��Ż �� UI ����
                }
            }
        }
        else
        {
            // ��Ż �ִϸ��̼� �۵�
            anim.SetBool("OpenPortal", isPortalOpen);
        }
    }

    // Ʈ���Ű� ó�� �ߵ��� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ʈ���� �ߵ�
            isTrigger = true;
        }
    }

    // Ʈ������ �ߵ��� ���� ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ʈ���� ����
            isTrigger = false;
        }
    }
}

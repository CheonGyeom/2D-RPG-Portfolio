using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    Animator anim; // ��Ż �ִϸ�����
    public Portal portal; // ��Ż ��ũ��Ʈ

    public GameObject portalObject; // ��Ż�Ŵ����� �Ҵ��� ������Ʈ
    public GameObject keyUI_F; // FŰ UI
    public GameObject animObj; // �ִϸ����Ͱ� �� �ִ� ������Ʈ

    public bool isPortalOpen; // ��Ż�� ���� �������� üũ
    public bool isTrigger; // ��Ż Ʈ���Ű� �ߵ� ���ΰ� (��Ż ��ó�ΰ�)


    void Update()
    {
        // �Ҵ���� ���� ������Ʈ�� �ִٸ� �Ҵ�
        if ((keyUI_F == null) || (anim == null) || (portal == null))
        {
            SetComponent();
        }

        // ��Ż ����
        if (isPortalOpen) // ��Ż�� ���ȴٸ�
        {
            // ��Ż �ִϸ��̼� �۵�
            anim.SetBool("OpenPortal", isPortalOpen);

            // ��Ż �� F��ư UI
            keyUI_F.SetActive(isTrigger);

            // ��Ż ž��
            if (isTrigger && anim.GetCurrentAnimatorStateInfo(0).IsName("Open")) // ��Ż Ʈ���Ÿ� �ߵ� ���̰�, ��Ż ���� �ִϸ��̼��� ���� ���� ��
            {
                // FŰ�� ������
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // ���ӸŴ����� ��Ż ž�� �Լ� ����
                    GameManager.instance.GetIntoPortal();
                }
            }
        }
        else
        {
            // ��Ż �ִϸ��̼� �۵�
            anim.SetBool("OpenPortal", isPortalOpen);
        }
    }

    // ������Ʈ �Ҵ�� ���� �� üũ
    public void SetComponent()
    {
        portalObject = GameObject.Find("Portal");
        keyUI_F = GameObject.Find("F");
        animObj = GameObject.Find("PortalAnim");

        anim = animObj.GetComponent<Animator>();
        portal = portalObject.GetComponent<Portal>();

        GameManager.instance.StageMonsterCheck(); // ���ӸŴ����� �������� ���� üũ �Լ� ����
    }
}

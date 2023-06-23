using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneDoor : MonoBehaviour
{
    public Animator anim; // �� �ִϸ�����
    public BoxCollider2D col; // �ݶ��̴�
    public void DoorOpen()
    {
        // �ݶ��̴� ����
        col.enabled = false;

        // �� ���� �ִϸ��̼�
        anim.SetBool("isClose", false);

        // ���� ���� ���
        SoundManager.instance.PlaySound("StoneDoor");
    }

    public void DoorClose()
    {
        // �ݶ��̴� �ѱ�
        col.enabled = true;

        // �� �ݱ� �ִϸ��̼�
        anim.SetBool("isClose", true);

        // ���� ���� ���
        SoundManager.instance.PlaySound("StoneDoor");
    }
}

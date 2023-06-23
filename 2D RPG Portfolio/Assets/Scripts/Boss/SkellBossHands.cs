using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellBossHands : MonoBehaviour
{
    public Animator anim; // �� �ִϸ�����

    public SkellBossLaser laser; // ������ ��ũ��Ʈ

    private Transform targetTransform; // �÷��̾� ��ġ 

    public float moveSpeed; // �� �̵��ӵ�
    public float handPosY; // ���� �̵��� y��ġ
    private void Start()
    {
        // �÷��̾� ��ġ �Ҵ�
        targetTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        MoveHand();
    }
    public void MoveHand()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector2(transform.position.x, handPosY), moveSpeed);
    }

    public void SetHandPos()
    {
        // �÷��̾� y��ġ�� ���� �� �̵� ��ġ
        if (targetTransform.position.y > 11f) // ���� ��
        {
            handPosY = 12f;
        }
        else if (targetTransform.position.y > 9f)
        {
            handPosY = 10f;
        }
        else if (targetTransform.position.y > 7f)
        {
            handPosY = 8f;
        }
        else if (targetTransform.position.y > 5f)
        {
            handPosY = 6f;
        }
        else if (targetTransform.position.y > 3f)
        {
            handPosY = 4f;
        }
        else if (targetTransform.position.y > 1f)
        {
            handPosY = 2f;
        }
        else // ���� �Ʒ�
        {
            handPosY = 0f;
        }
    }
}

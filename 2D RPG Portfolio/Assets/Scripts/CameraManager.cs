using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private GameObject player; // �÷��̾� ������Ʈ
    private Transform targetTransform; // ī�޶� ����ٴ� ������Ʈ(�÷��̾�)�� ��ġ
    private CinemachineVirtualCamera virtualCam; // �ó׸ӽ� ī�޶�

    void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        player = null;
    }

    private void Update()
    {
        // �÷��̾� ������Ʈ�� null�̸�
        if (player == null)
        {
            // ���� �Ҵ�
            player = GameObject.FindWithTag("Player");
        }
        // �÷��̾� ������Ʈ�� �ִٸ�
        else
        {
            // �ó׸ӽ� ī�޶��� Follow�� �÷��̾��� ��ġ�� ����
            virtualCam.Follow = player.transform;
        }
    }
}

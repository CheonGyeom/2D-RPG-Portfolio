using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDownPlatform : MonoBehaviour
{
    PlatformEffector2D pe; // �÷��� ������ 2D

    private bool isGoDown; // ���� �÷��� �������⸦ ���� ������ üũ
    private bool canGoDown; // �÷������� ������ �� �ִ� ��Ȳ(�÷����� ����ִ� ��Ȳ)���� üũ

    void Start()
    {
        pe = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        // �÷��̾ �׾��ٸ� Return
        if (GameManager.instance.isDie)
        {
            return;
        }

        // SŰ�� ���� ���·� �����̽� �ٸ� ������ (��, ���� �÷��� �������⸦ ���������� �ʰ�, �÷����� ����ִٸ�)
        if (Input.GetKey(KeyCode.S) && (Input.GetKeyDown(KeyCode.Space) && !isGoDown && canGoDown))
        {
            // �÷��� �������� �ڷ�ƾ ����
            StartCoroutine(GoDownPlatformCoroutine());
        }
    }

    // �÷��� �������� �ڷ�ƾ
    IEnumerator GoDownPlatformCoroutine()
    {
        isGoDown = true; // �÷��� �������⸦ ���� ��
        pe.rotationalOffset = 180f; // �ܹ��� �ݶ��̴� ���� �Ʒ������� ����
        yield return new WaitForSeconds(0.5f); // 0.5�� ���
        pe.rotationalOffset = 0f; // �ܹ��� �ݶ��̴� ���� �������� ����
        isGoDown = false; // �÷��� �������� ���� ����
    }
    
    // �ݶ��̴��� ����� �� ȣ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷������� ������ �� �ִ� ��Ȳ
        canGoDown = true;
    }

    // �ݶ��̴��� �������� �� ȣ��
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �÷������� ������ �� ���� ��Ȳ
        canGoDown = false;
    }
}

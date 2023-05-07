using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelHead : MonoBehaviour
{
    Rigidbody2D rb; // ���� �Ӹ��� ������ٵ�
    SpriteRenderer sr; // ���� �Ӹ��� ��������Ʈ ������
    void Awake()
    {
        // ������Ʈ �Ҵ�
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // ������Ʈ�� Ȱ��ȭ �Ǿ��� �� ȣ��
    private void OnEnable()
    {
        Invoke("DestroyHead", 5f); // 5�� �� ������Ʈ ��ȯ ó��

        // ƨ�ܳ��� ���� ����� ���� ��
        float randomForce_X = Random.Range(-5f, 5f);
        float randomForce_Y = Random.Range(4f, 5f);

        // ������ �������� ƨ�ܳ���
        Vector2 dropForce = new Vector2(randomForce_X, randomForce_Y);
        rb.AddForce(dropForce, ForceMode2D.Impulse);

        // ƨ�ܳ��� ���⿡ ���� ��������Ʈ �ø�
        if (randomForce_X < 0)
        {
            sr.flipX = true;
        }
    }

    private void DestroyHead()
    {
        gameObject.SetActive(false); // ������Ʈ Ǯ�� ��ȯ
    }
}

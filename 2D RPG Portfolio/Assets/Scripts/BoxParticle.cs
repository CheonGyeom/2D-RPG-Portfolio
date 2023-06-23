using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxParticle : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 resetPos; // �ʱ�ȭ ��ġ
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        resetPos = transform.position;
    }

    private void OnEnable()
    {
        StartCoroutine("DestroyBoxPiece");

        transform.parent.gameObject.transform.position = new Vector2(0, 0); // �θ� ��ġ �ʱ�ȭ
        transform.position = resetPos; // ���� ��ġ �ʱ�ȭ

        // ƨ�ܳ��� ���� ����� ���� ��
        float randomForce_X = Random.Range(-7f, 7f);
        float randomForce_Y = Random.Range(6f, 8f);

        // ������ �������� ƨ�ܳ���
        Vector2 dropForce = new Vector2(randomForce_X, randomForce_Y);
        rb.AddForce(dropForce, ForceMode2D.Impulse);
    }

    IEnumerator DestroyBoxPiece()
    {
        yield return new WaitForSeconds(5f); // 5�� ��
        gameObject.transform.parent.gameObject.SetActive(false); // ������Ʈ Ǯ�� ��ȯ
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    Rigidbody2D rb; // ����ġ�� ������ٵ�
    CircleCollider2D cc; // ����ġ�� Ʈ������ ����Ŭ �ݶ��̴� 

    public Transform hudPos; // ����ؽ�Ʈ ���� ��ġ

    void Awake()
    {
        // ������Ʈ �Ҵ�
        rb = gameObject.GetComponent<Rigidbody2D>();
        cc = gameObject.GetComponent<CircleCollider2D>();
    }

    // ������Ʈ�� Ȱ��ȭ �Ǿ��� �� ȣ��
    private void OnEnable()
    {
        StartCoroutine(TriggerDelayTime()); // Ʈ���� ������ Ÿ�� �ڷ�ƾ�� �����ؼ� ����ġ�� �������ڸ��� ȹ���ϴ� ���� ����

        // ƨ�ܳ��� ���� ����� ���� ��
        float randomForce_X = Random.Range(-7f, 7f);
        float randomForce_Y = Random.Range(4f, 6f);

        // ������ �������� ƨ�ܳ���
        Vector2 dropForce = new Vector2(randomForce_X, randomForce_Y);
        rb.AddForce(dropForce, ForceMode2D.Impulse);
    }

    // Ʈ���ſ� ����� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� �÷��̾ ���� �ʾ��� ��
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDie)
        {
            GameManager.instance.exp += 10f; // ����ġ 10 ȹ��

            // ��� �ؽ�Ʈ ����
            GameObject getExp_HudText = ObjectPoolingManager.instance.GetObject("HudText_GetExp");
            getExp_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            getExp_HudText.transform.GetChild(0).GetComponent<GetExp_HudText>().ShowGetExpText(10); // ��� �ؽ�Ʈ�� ǥ�� �� ����ġ�� ���� (�׸���)
            getExp_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<GetExpHudText_Shadow>().ShowGetExpText(10); // ��� �ؽ�Ʈ�� ǥ�� �� ����ġ�� ���� (�ؽ�Ʈ)

            cc.enabled = false; // �ٽ� Ʈ���� ��Ȱ��ȭ

            gameObject.SetActive(false); // ������Ʈ Ǯ�� ������Ʈ ��ȯ
        }
    }

    // ����ġ�� �������ڸ��� ȹ��Ǵ� ���� �����ϴ� ������ �ڷ�ƾ
    IEnumerator TriggerDelayTime()
    {
        yield return new WaitForSeconds(0.5f);
        cc.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody2D rb; // ������ ������ٵ�
    CircleCollider2D cc; // ������ Ʈ������ ����Ŭ �ݶ��̴� 

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
        StartCoroutine(TriggerDelayTime()); // Ʈ���� ������ Ÿ�� �ڷ�ƾ�� �����ؼ� ������ �������ڸ��� ȹ���ϴ� ���� ����

        // ƨ�ܳ��� ���� ����� ���� ��
        float randomForce_X = Random.Range(-4.5f, 4.5f);
        float randomForce_Y = Random.Range(3.5f, 5f);

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
            GameManager.instance.gold += 10f; // ��� 10 ȹ��

            // ��� �ؽ�Ʈ ����
            GameObject getGold_HudText = ObjectPoolingManager.instance.GetObject("HudText_GetGold");
            getGold_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            getGold_HudText.transform.GetChild(0).GetComponent<GetGold_HudText>().ShowGetGoldText(10); // ��� �ؽ�Ʈ�� ǥ�� �� ��差 ���� (�׸���)
            getGold_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<GetGoldHudText_Shadow>().ShowGetGoldText(10); // ��� �ؽ�Ʈ�� ǥ�� �� ��差 ���� (�ؽ�Ʈ)

            cc.enabled = false; // �ٽ� Ʈ���� ��Ȱ��ȭ

            gameObject.SetActive(false); // ������Ʈ Ǯ�� ������Ʈ ��ȯ
        }
    }

    // ������ �������ڸ��� ȹ��Ǵ� ���� �����ϴ� ������ �ڷ�ƾ
    IEnumerator TriggerDelayTime()
    {
        yield return new WaitForSeconds(0.5f);
        cc.enabled = true;
    }
}

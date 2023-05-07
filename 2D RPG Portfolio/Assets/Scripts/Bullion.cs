using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullion : MonoBehaviour
{
    Rigidbody2D rb; // �ݱ��� ������ٵ�
    BoxCollider2D bc; // �ݱ��� Ʈ������ �ڽ� �ݶ��̴� 

    public Transform hudPos; // ����ؽ�Ʈ ���� ��ġ

    void Awake()
    {
        // ������Ʈ �Ҵ�
        rb = gameObject.GetComponent<Rigidbody2D>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    // ������Ʈ�� Ȱ��ȭ �Ǿ��� �� ȣ��
    private void OnEnable()
    {
        StartCoroutine(TriggerDelayTime()); // Ʈ���� ������ Ÿ�� �ڷ�ƾ�� �����ؼ� �ݱ��� �������ڸ��� ȹ���ϴ� ���� ����

        // ƨ�ܳ��� ���� ����� ���� ��
        float randomForce_X = Random.Range(-3f, 3f);
        float randomForce_Y = Random.Range(3.5f, 4.5f);

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
            GameManager.instance.gold += 100f; // ��� 100 ȹ��

            // ��� �ؽ�Ʈ ����
            GameObject getGold_HudText = ObjectPoolingManager.instance.GetObject("HudText_GetGold");
            getGold_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            getGold_HudText.transform.GetChild(0).GetComponent<GetGold_HudText>().ShowGetGoldText(100); // ��� �ؽ�Ʈ�� ǥ�� �� ��差 ���� (�׸���)
            getGold_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<GetGoldHudText_Shadow>().ShowGetGoldText(100); // ��� �ؽ�Ʈ�� ǥ�� �� ��差 ���� (�ؽ�Ʈ)

            bc.enabled = false; // �ٽ� Ʈ���� ��Ȱ��ȭ

            gameObject.SetActive(false); // ������Ʈ Ǯ�� ������Ʈ ��ȯ
        }
    }

    // �ݱ��� �������ڸ��� ȹ��Ǵ� ���� �����ϴ� ������ �ڷ�ƾ
    IEnumerator TriggerDelayTime()
    {
        yield return new WaitForSeconds(0.5f);
        bc.enabled = true;
    }
}

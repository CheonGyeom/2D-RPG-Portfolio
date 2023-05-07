using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData item;

    public Inventory inventory;

    Rigidbody2D rb; // �������� ������ٵ�
    BoxCollider2D bc; // �������� Ʈ������ �ڽ� �ݶ��̴� 

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        inventory = FindObjectOfType<Inventory>();

        rb = gameObject.GetComponent<Rigidbody2D>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    // ������Ʈ�� Ȱ��ȭ �Ǿ��� �� ȣ��
    private void OnEnable()
    {
        StartCoroutine(TriggerDelayTime()); // Ʈ���� ������ Ÿ�� �ڷ�ƾ�� �����ؼ� �������� �������ڸ��� ȹ���ϴ� ���� ����

        // ƨ�ܳ��� ���� ����� ���� ��
        float randomForce_Y = Random.Range(5f, 7f);

        // ������ �������� ƨ�ܳ���
        Vector2 dropForce = new Vector2(0, randomForce_Y);
        rb.AddForce(dropForce, ForceMode2D.Impulse);
    }

    // Ʈ���ſ� ����� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� �÷��̾ ���� �ʾ��� ��
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDie)
        {
            inventory.AcquireItem(gameObject);

            bc.enabled = false; // �ٽ� Ʈ���� ��Ȱ��ȭ
        }
    }

    // �������� �������ڸ��� ȹ��Ǵ� ���� �����ϴ� ������ �ڷ�ƾ
    IEnumerator TriggerDelayTime()
    {
        yield return new WaitForSeconds(0.5f);
        bc.enabled = true;
    }
}

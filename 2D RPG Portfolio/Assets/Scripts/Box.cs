using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : EntityManager
{
    // ��� ������ ����
    public int minItemDrop; // �ּ� ��� ������ ����
    public int maxItemDrop; // �ִ� ��� ������ ����

    public override void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        base.TakeDamage(damage, Pos, isCritical);
    }

    public override void Die()
    {
        Explode(); // ���� �ı�

        gameObject.SetActive(false);
    }

    // ���� �ı� �Լ�
    private void Explode()
    {
        // Ǯ���� ���� ������ ����
        GameObject boxPiece =  ObjectPoolingManager.instance.GetObject("Object_BoxPiece");
        boxPiece.transform.position = transform.position;

        DropItem(); // ������ ���
    }

    // ������ ��� �Լ�
    private void DropItem()
    {
        // ����� ������ ���� ���� ��
        int rand_dropAmount = Random.Range(minItemDrop, maxItemDrop);

        // ����� ������ ������ŭ for�� ����
        for (int i = 0; i < rand_dropAmount; i++)
        {
            float rand_dropItem = Random.Range(0f, 1f);

            if (rand_dropItem < 0.95f) // ���� (95%) 
            {
                Debug.Log("���� ���");
                GameObject coin = ObjectPoolingManager.instance.GetObject("Item_Coin"); // ������Ʈ Ǯ���� ���� �뿩
                coin.transform.position = this.transform.position; // ��ġ �ʱ�ȭ
            }
            else if (rand_dropItem < 1.0f) // �ݱ� (5%) 
            {
                Debug.Log("�ݱ� ���");
                GameObject bullion = ObjectPoolingManager.instance.GetObject("Item_Bullion"); // ������Ʈ Ǯ���� �ݱ� �뿩
                bullion.transform.position = this.transform.position; // ��ġ �ʱ�ȭ
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour
{
    public enum FairySize
    {
        Size_S,
        Size_M,
        Size_L,
        Size_XL,
    }

    CircleCollider2D cc; // ���� Ʈ������ ����Ŭ �ݶ��̴� 
    public Transform hudPos; // ����ؽ�Ʈ ���� ��ġ

    PlayerManager pm; // �÷��̾� �Ŵ���

    public FairySize fairySize; // �� ������

    private int getHealthAmount; // HP ȹ�淮

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        cc = gameObject.GetComponent<CircleCollider2D>();
    }

    // ������Ʈ�� Ȱ��ȭ �Ǿ��� �� ȣ��
    private void OnEnable()
    {
        StartCoroutine(TriggerDelayTime()); // Ʈ���� ������ Ÿ�� �ڷ�ƾ�� �����ؼ� ���� �������ڸ��� ȹ���ϴ� ���� ����

        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>(); // �÷��̾� �Ŵ��� �Ҵ�

        // ����� ���� HP ȹ�淮 ����
        switch (fairySize)
        {
            // ������ S
            case FairySize.Size_S:
                getHealthAmount = 5; // HP ȹ�淮 5�� ����
                break;
            // ������ M
            case FairySize.Size_M:
                getHealthAmount = 15; // HP ȹ�淮 15�� ����
                break;
            // ������ L
            case FairySize.Size_L:
                getHealthAmount = 25; // HP ȹ�淮 25�� ����
                break;
            // ������ XL
            case FairySize.Size_XL:
                getHealthAmount = 50; // HP ȹ�淮 50���� ����
                break;
        }
    }

    // Ʈ���ſ� ����� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� �÷��̾ ���� �ʾ��� ��
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDie)
        {
            pm.health += getHealthAmount; // HP ȹ�淮 ��ŭ ü�� ����

            // ��� �ؽ�Ʈ ����
            GameObject getHp_HudText = ObjectPoolingManager.instance.GetObject("HudText_GetHp");
            getHp_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            getHp_HudText.transform.GetChild(0).GetComponent<GetHealth_HudText>().ShowGetHealthText(getHealthAmount); // ��� �ؽ�Ʈ�� ǥ�� �� ü�� ȸ���� ���� (�׸���)
            getHp_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<GetHealthHudText_Shadow>().ShowGetHealthText(getHealthAmount); // ��� �ؽ�Ʈ�� ǥ�� �� ü�� ȸ���� ���� (�ؽ�Ʈ)

            // ȹ�� ���� ���
            SoundManager.instance.PlaySound("GetFairy");

            cc.enabled = false; // �ٽ� Ʈ���� ��Ȱ��ȭ
            gameObject.SetActive(false); // ������Ʈ Ǯ�� ������Ʈ ��ȯ
        }
    }

    // ���� �������ڸ��� ȹ��Ǵ� ���� �����ϴ� ������ �ڷ�ƾ
    IEnumerator TriggerDelayTime()
    {
        yield return new WaitForSeconds(0.5f);
        cc.enabled = true;
    }
}

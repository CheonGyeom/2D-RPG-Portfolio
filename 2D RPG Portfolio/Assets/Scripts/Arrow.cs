using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Skel_Bow_Monster skel;

    private void Update()
    {
        // ȭ��� ���� �ü��� ��ġ ���
        float distance = Vector2.Distance(skel.transform.position, transform.position);

        // ȭ���� ��ġ�� ���� �ü� ��ġ�� ��Ÿ����� �� �־����� �� ȭ���� �������� �ʾҴٸ�
        if (distance > skel.arrowDistance)
        {
            // ȭ�� ����
            DestroyArrow();
        }
    }

    // Ʈ���ſ� ����� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� �÷��̾ ���� �ʾ��� ��
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDie)
        {
            // �ǰ�
            GameManager.instance.failCause = "���̷��� �ü����� �й�"; // ��� ����

            skel.attackPos = transform;
            collision.GetComponent<PlayerManager>().TakeDamage(skel.attackDamage, skel.attackPos, false); // ������ �μ� = ���ʹ� ũ��Ƽ�� ���� ����

            // ȭ�� ����
            DestroyArrow();
        }
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷����� ��
        else if (collision.gameObject.CompareTag("Platform"))
        {
            // ȭ�� ����
            DestroyArrow();
        }
    }

    public void DestroyArrow()
    {
        // ����Ʈ ����
        GameObject hitEffect = ObjectPoolingManager.instance.GetObject("Effect_ArrowHit"); // ����Ʈ ������Ʈ Ǯ���� �뿩
        hitEffect.transform.position = transform.position; // ��ġ ����
        hitEffect.transform.rotation = transform.rotation; // ���� ����

        skel.arrowHitFXAnim = hitEffect.GetComponent<Animator>();

        // �ִϸ��̼� Ʈ���� Ȱ��ȭ
        skel.arrowHitFXAnim.SetTrigger("Hit_Arrow");

        // ȭ�� ������Ʈ Ǯ�� ��ȯ
        gameObject.SetActive(false);
    }
}

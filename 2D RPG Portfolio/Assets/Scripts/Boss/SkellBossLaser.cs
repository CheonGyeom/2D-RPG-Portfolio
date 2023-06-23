using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellBossLaser : MonoBehaviour
{
    public Animator[] anims; // ������ �ִϸ��̼� �迭
    public bool isAttack; // ���� ������ üũ

    public Transform attackPos; // ������ ������ ��ġ
    public int attackDamage; // ���ݷ�

    public CapsuleCollider2D hitCollider; // �ǰ� ���� �ݶ��̴�


    // Ʈ���ſ� ���� ���� �� ȣ��
    private void OnTriggerStay2D(Collider2D player)
    {
        // ���� ���� ��
        if (isAttack)
        {
            // �±װ� �÷��̾���
            if (player.gameObject.tag == "Player")
            {
                GameManager.instance.failCause = "�����˿��� �й�"; // ��� ����

                attackPos = transform;
                player.gameObject.GetComponent<PlayerManager>().TakeDamage(attackDamage, attackPos, false); // ������ �μ� = ���ʹ� ũ��Ƽ�� ���� ����

                isAttack = false; // ���� ���� ����
            }
        }
    }

    // ������ �߻� �Լ�
    public void LaserShot()
    {
        isAttack = true; // ���� ���� ����

        hitCollider.enabled = true; // �ǰ� ���� Ȱ��ȭ

        SoundManager.instance.PlaySound("LaserShot"); // ���� ���

        // ��� ������ �߻� �ִϸ��̼� ���
        for (int i = 0; i < anims.Length; i++)
        {            
            anims[i].SetBool("isShot", true);
        }

        Invoke("LaserOff", 0.75f);
    }

    // ������ �߻� ���� �Լ�
    public void LaserOff()
    {
        // ��� ������ ���� �ִϸ��̼� ���
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].SetBool("isShot", false);
        }

        Invoke("AttackOff", 0.25f);
    }

    // ���� ���� ���� �Լ�
    public void AttackOff()
    {
        isAttack = false; // ���� ���� �� �ƴ�

        hitCollider.enabled = false; // �ǰ� ���� ��Ȱ��ȭ
    }
}

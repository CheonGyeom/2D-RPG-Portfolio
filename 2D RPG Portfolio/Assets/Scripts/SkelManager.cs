using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelManager : MonsterManager
{
    public Rigidbody2D rb;
    public Animator skelAnim; // ���� �ִϸ�����
    public Animator handsAnim; // �� �ִϸ�����
    public Animator WeaponAnim; // ���� �ִϸ�����

    public GameObject handsPosObject; // �� ��ġ ������Ʈ

    public Transform attackPos; // ���ݹ޾��� �� ��밡 ������ ��ġ

    // �̵� �Լ�
    public virtual void Move()
    {
        // ���Ͱ� �׾��ų� ���� ���̶�� �̵� ����
        if (isDie || (WeaponAnim.GetCurrentAnimatorStateInfo(0).IsName("Skel_ShortSword_Attack") && (WeaponAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f || WeaponAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)))
        {
            return;
        }

        // �̵�
        rb.velocity = new Vector2(nextMove * moveSpeed, rb.velocity.y);

        // �̵� �ִϸ��̼�
        skelAnim.SetBool("isMove", true);

        // ���� ���� ����
        if (nextMove < 0)
        {
            // ����
            Vector3 scale = transform.localScale; // ������ ������ ��
            scale.x = -Mathf.Abs(scale.x); // �������� x���� -����
            transform.localScale = scale; // ������ ������ ���� ������

            Vector3 hpScale = hp_Bar.transform.localScale;
            hpScale.x = -Mathf.Abs(hpScale.x);
            hp_Bar.transform.localScale = hpScale; // HP �ٴ� �׻� �������� ��
        }
        else if (nextMove > 0)
        {
            // ������
            Vector3 scale = transform.localScale; // ������ ������ ��
            scale.x = Mathf.Abs(scale.x); // �������� x���� ����
            transform.localScale = scale; // ������ ������ ���� ������

            Vector3 hpScale = hp_Bar.transform.localScale;
            hpScale.x = Mathf.Abs(hpScale.x);
            hp_Bar.transform.localScale = hpScale; // HP �ٴ� �׻� �������� ��
        }
        else
        {
            skelAnim.SetBool("isMove", false);
        }
    }

    // ���� �Լ�
    public virtual void Attack() { }

    // ����� �޴� �Լ�
    public override void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        // �׾��ٸ� ����
        if (isDie)
        {
            return;
        }

        hp_Bar.SetActive(true); // HP �� Ȱ��ȭ

        CancelInvoke("Hide_HpBar"); // �ʱ�ȭ
        Invoke("Hide_HpBar", 2f); // 2�� �� HP �� ��Ȱ��ȭ

        base.TakeDamage(damage, Pos, isCritical); // ����� ����

        skelAnim.SetTrigger("Hit"); // �´� �ִϸ��̼�
        handsAnim.SetTrigger("Hit"); // �� �´� �ִϸ��̼�

        // ũ��Ƽ���̶��
        if (isCritical)
        {
            // ũ��Ƽ�� ����� ��� �ؽ�Ʈ ����
            GameObject damage_HudText = ObjectPoolingManager.instance.GetObject("HudText_CriticalDam");
            damage_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            damage_HudText.transform.GetChild(0).GetComponent<Damage_HudText>().ShowDamageText(Mathf.RoundToInt(damage * (GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue))); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�׸���)
            damage_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<DamageHudText_Shadow>().ShowDamageText(Mathf.RoundToInt(damage * (GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue))); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�ؽ�Ʈ)

            // �˹�
            float x = transform.position.x - Pos.position.x; // �з��� ����
            if (x > 0)
            {
                rb.velocity = new Vector2(5f, rb.velocity.y); // ���������� 5��ŭ �˹� 
            }
            else if (x < 0)
            {
                rb.velocity = new Vector2(-5f, rb.velocity.y); // �������� 5��ŭ �˹� 
            }
        }
        else
        {
            // ����� ��� �ؽ�Ʈ ����
            GameObject damage_HudText = ObjectPoolingManager.instance.GetObject("HudText_Damage");
            damage_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            damage_HudText.transform.GetChild(0).GetComponent<Damage_HudText>().ShowDamageText(damage); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�׸���)
            damage_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<DamageHudText_Shadow>().ShowDamageText(damage); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�ؽ�Ʈ)

            // �˹�
            float x = transform.position.x - Pos.position.x; // �з��� ����
            if (x > 0)
            {
                rb.velocity = new Vector2(3f, rb.velocity.y); // ���������� 3��ŭ �˹� 
            }
            else if (x < 0)
            {
                rb.velocity = new Vector2(-3f, rb.velocity.y); // �������� 3��ŭ �˹� 
            }
        }

        // ���� ���� (����)
        //nextAttackTime = Time.time + attackCoolDown;

        hp_fill.fillAmount = health / maxHealth; // ü�¹� ����
    }

    public override void Die()
    {
        base.Die();

        skelAnim.SetTrigger("Die"); // �״� �ִϸ��̼�

        //GetComponent<Collider2D>().enabled = false; // �ݶ��̴� ����
        //rb.isKinematic = true; // ��ġ ����
        //rb.velocity = new Vector2(0, 0); // ��ġ ����

        handsPosObject.SetActive(false); // �հ� ���� ��Ȱ��ȭ
        hp_Bar.gameObject.SetActive(false); // HP �� ��Ȱ��ȭ
    }

    // ���� AI
    public override void MonsterAI()
    {
        base.MonsterAI();
    }

    // ���Ͱ� ���������� ���� ���� ���� �Լ�
    public virtual void PlatformCheck()
    {
        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            nextMove = 0; // ����
        }
    }

    // ������ ��� �Լ�
    public override void DropItem()
    {
        base.DropItem();

        // ����� ������ ���� ���� ��
        int rand_dropAmount = Random.Range(minItemDrop, maxItemDrop);

        // ����� ������ ������ŭ for�� ����
        for (int i = 0; i < rand_dropAmount; i++)
        {
            float rand_dropItem = Random.Range(0f, 1f);

            if (rand_dropItem < 0.55f) // ����ġ (55%) 
            {
                Debug.Log("����ġ ���");
                GameObject exp = ObjectPoolingManager.instance.GetObject("Item_Exp"); // ������Ʈ Ǯ���� ����ġ �뿩
                exp.transform.position = this.transform.position; // ��ġ �ʱ�ȭ
            }
            else if (rand_dropItem < 0.95f) // ���� (40%) 
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

        // ���� �Ӹ� ������Ʈ ���
        GameObject skelHead = ObjectPoolingManager.instance.GetObject("Object_SkelHead"); // ������Ʈ Ǯ���� ���� �Ӹ� �뿩
        skelHead.transform.position = this.transform.position; // ��ġ �ʱ�ȭ
    }
}

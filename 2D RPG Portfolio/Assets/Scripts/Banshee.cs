using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banshee : MonsterManager
{
    public Rigidbody2D rb; // ���� �Ŵ� ���� ������ٵ�
    public Animator anim; // ���� �Ŵ� ���� �ִϸ�����

    public Animator dieAnim; // ��� �� ����Ʈ �ִϸ�����

    // �߻�ü ����
    MusicNote[] mn; // ��ǥ ��ũ��Ʈ �迭

    public int shotMusicNoteNum; // �߻��� ��ǥ ����
    public float musicNoteSpeed; // ��ǥ �ӵ�
    public float musicNoteDistance; // ��ǥ ��Ÿ�

    public GameObject aimPosObject; // ��ǥ�� �߻��� ���� ���� ���� ������Ʈ
    public Transform bulletPos; // ��ǥ�� �߻��� ��ġ

    private void Start()
    {
        // �÷��̾� ��ġ �Ҵ�
        targetTransform = GameObject.FindWithTag("Player").transform;

        // �迭 �ʱ�ȭ
        mn = new MusicNote[shotMusicNoteNum];

        hp_Bar.SetActive(false); // HP �� ������
    }

    private void FixedUpdate()
    {
        // �������� HP ��
        hp_fill_Lerp.fillAmount = Mathf.Lerp(hp_fill_Lerp.fillAmount, hp_fill.fillAmount, Time.deltaTime * 7.5f);

        // ���Ͱ� �׾��ٸ� ����
        if (isDie)
        {
            return;
        }

        MonsterAI();

        // ���� ���� ����
        if (targetTransform.position.x < transform.position.x)
        {
            // ����
            Vector3 scale = transform.localScale; // ������ ������ ��
            scale.x = -Mathf.Abs(scale.x); // �������� x���� -����
            transform.localScale = scale; // ������ ������ ���� ������

            Vector3 hpScale = hp_Bar.transform.localScale;
            hpScale.x = -Mathf.Abs(hpScale.x);
            hp_Bar.transform.localScale = hpScale; // HP �ٴ� �׻� �������� ��
        }
        else if (targetTransform.position.x > transform.position.x)
        {
            // ������
            Vector3 scale = transform.localScale; // ������ ������ ��
            scale.x = Mathf.Abs(scale.x); // �������� x���� ����
            transform.localScale = scale; // ������ ������ ���� ������

            Vector3 hpScale = hp_Bar.transform.localScale;
            hpScale.x = Mathf.Abs(hpScale.x);
            hp_Bar.transform.localScale = hpScale; // HP �ٴ� �׻� �������� ��
        }

        // ���� ���¶��
        if (isAttack)
        {
            StartCoroutine("SetMusicNote"); // ��ǥ ����

            // ����
            Attack();

            // ���� �ִϸ��̼�
            anim.SetTrigger("Attack");

            // ���� ���� ���
            SoundManager.instance.PlaySound("Banshee_Scream");

            isAttack = false; // ���� �� �ƴ�
        }
    }

    // ���� �Լ�
    public void Attack()
    {
        // ��� ��ǥ �߻� �Լ� ����
        for (int i = 0; i < mn.Length; i++)
        {
            mn[i].Shot();
        }
    }

    // ���� �ൿ AI �Լ�
    public override void MonsterAI()
    {
        // �÷��̾ ���� ���� �ȿ� �ִ�.
        if (Vector2.Distance(transform.position, targetTransform.position) < attackDistance)
        {
            if (Time.time >= nextAttackTime)
            {
                isAttack = true; // ���� ���¸� true�� ����

                nextAttackTime = Time.time + attackCoolDown;
            }
        }
    }

    // ����� ���� �Լ�
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

        // ũ��Ƽ���̶��
        if (isCritical)
        {
            // ũ��Ƽ�� ����� ��� �ؽ�Ʈ ����
            GameObject damage_HudText = ObjectPoolingManager.instance.GetObject("HudText_CriticalDam");
            damage_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            damage_HudText.transform.GetChild(0).GetComponent<Damage_HudText>().ShowDamageText(Mathf.RoundToInt(damage * (GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue))); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�׸���)
            damage_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<DamageHudText_Shadow>().ShowDamageText(Mathf.RoundToInt(damage * (GameManager.instance.critical_Value + GameManager.instance.increased_CriticalValue))); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�ؽ�Ʈ)
        }
        else
        {
            // ����� ��� �ؽ�Ʈ ����
            GameObject damage_HudText = ObjectPoolingManager.instance.GetObject("HudText_Damage");
            damage_HudText.transform.position = hudPos.position; // ��� �ؽ�Ʈ ��ġ ����
            damage_HudText.transform.GetChild(0).GetComponent<Damage_HudText>().ShowDamageText(damage); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�׸���)
            damage_HudText.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<DamageHudText_Shadow>().ShowDamageText(damage); // ��� �ؽ�Ʈ�� ǥ�� �� ����� ���� (�ؽ�Ʈ)
        }

        hp_fill.fillAmount = health / maxHealth; // ü�¹� ����
    }

    // ��� �Լ�
    public override void Die()
    {
        base.Die();

        anim.SetTrigger("Die"); // �״� �ִϸ��̼�
        hp_Bar.gameObject.SetActive(false); // HP �� ��Ȱ��ȭ
    }

    // ������ ���
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
    }

    // ��ǥ ���� �Լ�
    IEnumerator SetMusicNote()
    {
        // �׻� ������ �������� �����ϱ� ���� ���� ȸ��
        int rand_Angle = Random.Range(0, 360); // (0 ~ 359)
        aimPosObject.transform.Rotate(new Vector3(0, 0, rand_Angle)); // ȸ��

        // ��ǥ ����
        for (int i = 0; i < shotMusicNoteNum; i++)
        {
            // ������Ʈ Ǯ���� ��ǥ ������ �뿩
            GameObject musicNote = ObjectPoolingManager.instance.GetObject("Bullet_MusicNote");
            mn[i] = musicNote.GetComponent<MusicNote>();

            // ��ǥ ���� ����
            mn[i].speed = musicNoteSpeed; // ��ǥ �ӵ�
            mn[i].distance = musicNoteDistance; // ��ǥ ��Ÿ�
            mn[i].aimPos = aimPosObject.transform; // ��ǥ �߻� ����
            mn[i].bulletPos = bulletPos; // ��ǥ �߻� ��ġ
            mn[i].shooterPos = transform; // �߻��� ��ü�� ��ġ
            mn[i].damage = attackDamage; // ���ݷ�
            mn[i].failCause = "��ÿ��� �й�"; // ��� ����

            // ��ǥ ����
            mn[i].Setting();

            float angle = 360 / shotMusicNoteNum;
            aimPosObject.transform.Rotate(new Vector3(0, 0, angle)); // ȸ��
        }

        return null;
    }

    // HP�ٸ� ����� �Լ�
    public override void Hide_HpBar()
    {
        base.Hide_HpBar();
    }
}

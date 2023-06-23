using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantRedBat : MonsterManager
{
    public Rigidbody2D rb; // ���� �Ŵ� ���� ������ٵ�
    public Animator anim; // ���� �Ŵ� ���� �ִϸ�����

    public Animator dieAnim; // ��� �� ����Ʈ �ִϸ�����

    // �߻�ü ����
    FireBall[] fb; // ���̾ ��ũ��Ʈ �迭

    public int shotFireBallNum; // �߻��� ���̾ ����
    public float shotTime; // ���̾ ���� �ϴµ� �ɸ��� �ð�
    public float fireBallSpeed; // ���̾ �ӵ�
    public float fireBallDistance; // ���̾ ��Ÿ�

    public GameObject axisArmObject; // ���̾ ���� �� ȸ����ų �� (��)
    public GameObject aimPosObject; // ���̾�� �߻��� ���� ���� ���� ������Ʈ
    public Transform bulletPos; // ���̾�� �߻��� ��ġ

    public bool isShot; // ���̾�� ������ üũ

    private void Start()
    {
        // �÷��̾� ��ġ �Ҵ�
        targetTransform = GameObject.FindWithTag("Player").transform;

        // �迭 �ʱ�ȭ
        fb = new FireBall[shotFireBallNum + 1];

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

        AimPlayer();
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
            StartCoroutine("SetFireBall"); // ���̾ ����

            isAttack = false; // ���� �� �ƴ�
        }

        // ���� �ִϸ��̼��� ����� ����Ǿ��ٸ�
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.55f)
        {
            // ����
            Attack();
        }
    }

    // ���� �Լ�
    public void Attack()
    {
        // ���̾�� �̹� �� ���¶�� ����
        if (isShot)
        {
            return;
        }

        // ��� ���̾ �߻� �Լ� ����
        for (int i = 0; i < fb.Length; i++)
        {
            fb[i].Shot();
        }

        isShot = true; // ���̾ �� ����
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

    // �÷��̾� ���� �Լ�
    public void AimPlayer()
    {
        Vector3 dir = targetTransform.position - transform.position;

        // ���⿡ ���� �ٸ� ���� (��Ȯ�� ���� �Ʒ��� ������ �� ����� ���׸� ���� ����)
        if (transform.localScale.x > 0)
        {
            aimPosObject.transform.right = dir.normalized;
        }
        else if (transform.localScale.x < 0)
        {
            aimPosObject.transform.right = -dir.normalized;
        }
    }

    // ���̾ ���� �Լ�
    IEnumerator SetFireBall()
    {
        isShot = false; // ���̾ ���� ���� ����

        // ���̾ ����
        for (int i = 0; i <= shotFireBallNum; i++)
        {
            // ������Ʈ Ǯ���� ���̾ ������ �뿩
            GameObject fireBall = ObjectPoolingManager.instance.GetObject("Bullet_FireBall");
            fb[i] = fireBall.GetComponent<FireBall>();

            // ���̾ ���� ����
            fb[i].speed = fireBallSpeed; // ���̾ �ӵ�
            fb[i].distance = fireBallDistance; // ���̾ ��Ÿ�
            fb[i].aimPos = aimPosObject.transform; // ���̾ �߻� ����
            fb[i].bulletPos = bulletPos; // ���̾ �߻� ��ġ
            fb[i].shooterPos = transform; // �߻��� ��ü�� ��ġ
            fb[i].damage = attackDamage; // ���ݷ�
            fb[i].failCause = "���� �Ŵ� ���㿡�� �й�"; // ��� ����

            // ���̾ ����
            fb[i].Setting(); 

            // ��� ��
            yield return new WaitForSeconds(shotTime / shotFireBallNum);

            float angle = 360 / shotFireBallNum;
            axisArmObject.transform.Rotate(new Vector3(0, 0, angle)); // ȸ��
        }

        // ���� �ִϸ��̼�
        anim.SetTrigger("Attack");
    }

    // HP�ٸ� ����� �Լ�
    public override void Hide_HpBar()
    {
        base.Hide_HpBar();
    }
}

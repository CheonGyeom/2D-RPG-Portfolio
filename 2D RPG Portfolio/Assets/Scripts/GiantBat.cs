using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBat : MonsterManager
{
    public Rigidbody2D rb; // �Ŵ� ���� ������ٵ�
    public Animator anim; // �Ŵ� ���� �ִϸ�����

    public Animator dieAnim; // ��� �� ����Ʈ �ִϸ�����

    public bool isFireBallSetting; // ���� ���̾ ���� ������ üũ

    // �߻�ü ����
    FireBall[] fb; // ���̾ ��ũ��Ʈ �迭

    public float sideFireBallAngle; // ���̵� ���̾ ����
    public float fireBallSpeed; // ���̾ �ӵ�
    public float fireBallDistance; // ���̾ ��Ÿ�
    public int shotNumber; // ���� �� ���� �߻� Ƚ��
    public float shotDelay; // ���� �߻� �� ������

    public GameObject aimPosObject; // ���̾�� �߻��� ���� ���� ���� ������Ʈ
    public Transform bulletPos; // ���̾�� �߻��� ��ġ

    public Transform leftSideBulletPos; // ���� ���̾�� �߻��� ��ġ
    public Transform rightSideBulletPos; // ���� ���̾�� �߻��� ��ġ
    public Transform laftSidePosObject; // ���� ���̾ �߻縦 ���� ���� ���� ���� ������Ʈ
    public Transform rightSidePosObject; // ���� ���̾ �߻縦 ���� ���� ���� ���� ������Ʈ

    private void Start()
    {
        // �÷��̾� ��ġ �Ҵ�
        targetTransform = GameObject.FindWithTag("Player").transform;

        // �迭 �ʱ�ȭ
        fb = new FireBall[shotNumber * 3];

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

        // ���� ���¶��
        if (isAttack)
        {
            StartCoroutine("SetFireBall"); // ���̾ ����

            isAttack = false; // ���� �� �ƴ�
        }

        // ���� ���̾ ���� ���̶�� ����
        if (isFireBallSetting)
        {
            return;
        }

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
        // ���̾ ���� ���̶�� ���� �� ��
        if (isFireBallSetting)
        {
            return;
        }

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
        // ���̾ ���� �� üũ
        isFireBallSetting = true;

        int k = 0; // ���̾ �ε��� ����

        // ���̾ ����
        for (int i = 0; i < shotNumber; i++)
        {
            // [���� ���̾]
            // ������Ʈ Ǯ���� ���̾ ������ �뿩
            GameObject fireBall_Left = ObjectPoolingManager.instance.GetObject("Bullet_FireBall");
            fb[k] = fireBall_Left.GetComponent<FireBall>();

            // ���̾ ���� ����
            fb[k].speed = fireBallSpeed; // ���̾ �ӵ�
            fb[k].distance = fireBallDistance; // ���̾ ��Ÿ�

            laftSidePosObject.rotation = aimPosObject.transform.rotation * Quaternion.Euler(new Vector3(0, 0, -sideFireBallAngle)); // ���� ���⿡ ���̵� ���̾ ������ �� ���� ����
            fb[k].aimPos = laftSidePosObject.transform; // ���̾ �߻� ����

            fb[k].bulletPos = leftSideBulletPos; // ���̾ �߻� ��ġ
            fb[k].shooterPos = transform; // �߻��� ��ü�� ��ġ
            fb[k].damage = attackDamage; // ���ݷ�
            fb[k].failCause = "���̾�Ʈ ���㿡�� ����"; // ��� ����

            // ���̾ ����
            fb[k].Setting();

            k++;

            // [�߾� ���̾]
            // ������Ʈ Ǯ���� ���̾ ������ �뿩
            GameObject fireBall_Middle = ObjectPoolingManager.instance.GetObject("Bullet_FireBall");
            fb[k] = fireBall_Middle.GetComponent<FireBall>();

            // ���̾ ���� ����
            fb[k].speed = fireBallSpeed; // ���̾ �ӵ�
            fb[k].distance = fireBallDistance; // ���̾ ��Ÿ�

            fb[k].aimPos = aimPosObject.transform; // ���̾ �߻� ����

            fb[k].bulletPos = bulletPos; // ���̾ �߻� ��ġ
            fb[k].shooterPos = transform; // �߻��� ��ü�� ��ġ
            fb[k].damage = attackDamage; // ���ݷ�
            fb[k].failCause = "���̾�Ʈ ���㿡�� �й�"; // ��� ����

            // ���̾ ����
            fb[k].Setting();

            k++;

            // [���� ���̾]
            // ������Ʈ Ǯ���� ���̾ ������ �뿩
            GameObject fireBall_Right = ObjectPoolingManager.instance.GetObject("Bullet_FireBall");
            fb[k] = fireBall_Right.GetComponent<FireBall>();

            // ���̾ ���� ����
            fb[k].speed = fireBallSpeed; // ���̾ �ӵ�
            fb[k].distance = fireBallDistance; // ���̾ ��Ÿ�

            rightSidePosObject.rotation = aimPosObject.transform.rotation * Quaternion.Euler(new Vector3(0, 0, sideFireBallAngle)); // ���� ���⿡ ���̵� ���̾ ������ ���� ���� ����
            fb[k].aimPos = rightSidePosObject.transform; // ���̾ �߻� ����

            fb[k].bulletPos = rightSideBulletPos; // ���̾ �߻� ��ġ
            fb[k].shooterPos = transform; // �߻��� ��ü�� ��ġ
            fb[k].damage = attackDamage; // ���ݷ�
            fb[k].failCause = "���̾�Ʈ ���㿡�� �й�"; // ��� ����

            // ���̾ ����
            fb[k].Setting();

            k++;

            // ��� ��
            yield return new WaitForSeconds(shotDelay);
        }

        // ���� �ִϸ��̼�
        anim.SetTrigger("Attack");

        // 0.4�� ��ٸ� ��
        yield return new WaitForSeconds(0.4f);

        // ���̾ �߻� �ڷ�ƾ ����
        StartCoroutine("Shot");
    }

    // ���̾ �߻� �Լ�
    IEnumerator Shot()
    {
        Debug.Log("�Ŵ� ���� �߻�!");

        int k = 0; // ���̾ �ε��� ���� ����

        // ���̾ �߻�
        for (int i = 0; i < shotNumber; i++)
        {
            fb[k].Shot(); // �߾�
            k++;

            fb[k].Shot(); // ����
            k++;

            fb[k].Shot(); // ����
            k++;

            yield return new WaitForSeconds(shotDelay);
        }

        // ���̾ ���� �� �ƴ�
        isFireBallSetting = false;
    }

    // HP�ٸ� ����� �Լ�
    public override void Hide_HpBar()
    {
        base.Hide_HpBar();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkellBoss : MonsterManager
{
    public Animator head_Anim; // �Ӹ� �ִϸ�����
    public Animator skellBoss_Anim; // ������ �ִϸ�����

    public SkellBossHands leftHand; // ���� �� ��ũ��Ʈ
    public SkellBossHands rightHand; // ������ �� ��ũ��Ʈ

    public int patternNumber; // ���� ������ ����
    public bool isSetPattern; // ���� ���� ������ ������� üũ

    public int min_LaserShotNum; // ������ ������ �� �ּ� Ƚ��
    public int max_LaserShotNum; // ������ ������ �� �ִ� Ƚ��

    public bool isLeftHand; // �޼��� ����� �������� üũ

    public bool isBattleStart; // ���� �����ߴ��� üũ
    public bool isFindPlayer; // �÷��̾ �����ߴ��� üũ

    public ParticleSystem bossDie_Particle; // ���� ��� ��ƼŬ
    public ParticleSystem bossDieBoom_Particle; // ���� ��� ���� ��ƼŬ

    public GameObject bossSkullPrefab; // ���� �ذ� ������


    // ī�޶�, �ƾ� ����
    public PlayableDirector director_Show; // ���� ���� �� Ÿ�Ӷ��� ����� ����
    public PlayableDirector director_BossDie; // ���� ��� �� Ÿ�Ӷ��� ����� ����

    public GameObject defaultCam; // �⺻ ī�޶�
    public GameObject battleCam; // ���� ī�޶�

    // �߻�ü ���� - �����캼
    ShadowBall[] sb; // �����캼 ��ũ��Ʈ �迭

    public int shotShadowBallNum; // �߻��� �����캼 ����
    public float shotTime; // �����캼�� �߻��� �ð�
    public float rotateAngle; // ȸ�� ����
    public float shadowBallSpeed; // �����캼 �ӵ�
    public float shadowBallDistance; // �����캼 ��Ÿ�

    public Transform bulletPos; // �����캼�� �߻� ��ġ
    public Transform axisPos; // �����캼�� �߻� ����

    // �߻�ü ���� - ������ ��
    BossSword[] bs; // �� ��ũ��Ʈ �迭

    public float bossSword_shotTime; // ���� �߻��� �ð�
    public float bossSwordSpeed; // �� �ӵ�
    public float bossSwordlDistance; // �� ��Ÿ�
    public int bossSword_Damage; // �� �����

    public Transform[] bossSword_BulletPos; // ���� �߻� ��ġ
    public Transform bossSword_axisPos; // ���� �߻� ����

    // �ڷ�ƾ ����
    IEnumerator laserAttackCoroutine; // ������ ����
    IEnumerator setShadowBallCoroutine; // �����캼 ����
    IEnumerator swordAttackCoroutine; // �� ����

    // �� ��ũ��Ʈ
    public StoneDoor leftDoor; // ���� �� ��ũ��Ʈ
    public StoneDoor rightDoor; // ������ �� ��ũ��Ʈ

    private void Start()
    {
        // �÷��̾� ��ġ �Ҵ�
        targetTransform = GameObject.FindWithTag("Player").transform;

        // �迭 �ʱ�ȭ
        sb = new ShadowBall[shotShadowBallNum * 4 + 4];
        bs = new BossSword[bossSword_BulletPos.Length];

        // �ڷ�ƾ ���� �Ҵ�
        laserAttackCoroutine = LaserAttack();
        setShadowBallCoroutine = SetShadowBall();
        swordAttackCoroutine = SwordAttack();
    }

    private void Update()
    {
        // ������ ���۵��� �ʾҴٸ� �÷��̾� ������ �õ��ϰ� ����
        if (!isBattleStart)
        {
            PlayerSensor(); // �÷��̾� ���� �Լ�
            return;
        }

        SetPattern(); // ���� ���� ���� �Լ�
    }

    // ���� ���� ���� �Լ�
    public void SetPattern()
    {
        if (!isSetPattern) // ���� ���� ������ ������ �ʾҴٸ�
        {
            float rand_Pattern = Random.Range(0f, 1f); // ���� ���� ������ ����ϱ� ���� ���� �� ���ϱ�

            if (rand_Pattern < 0.45f) // ������ ���� (45%) 
            {
                // ������ ���� �ڷ�ƾ ����
                StartCoroutine("LaserAttack");
            }
            else if (rand_Pattern < 0.6f) // źȯ �߻� (15%) 
            {
                // źȯ �߻� �ڷ�ƾ ����
                StartCoroutine("BulletAttack");
            }
            else if (rand_Pattern < 1.0f) // ��� ��ȯ (40%) 
            {
                // ��� ��ȯ �ڷ�ƾ ����
                StartCoroutine("SwordAttack");
            }

            isSetPattern = true; // ���� ���� ���� üũ
        }
    }

    // �÷��̾� ���� �Լ�
    public void PlayerSensor()
    {
        // �ν� ���� ���� �÷��̾ ã��
        Collider2D player = Physics2D.OverlapCircle(attackTransform[0].position, attackRadius[0], LayerMask.GetMask("Player"));

        if (player != null && !isFindPlayer) // �÷��̾ �����ϰ� ���� ���� �ƴ϶��
        {
            // ��������
            Debug.Log("�÷��̾� ����! ���� ����");
            isFindPlayer = true;
            skellBoss_Anim.SetBool("isBattle", true);
            SoundManager.instance.PlaySound("SkellBossLaugh"); // ���� ���� ���� ���
            SoundManager.instance.PlayBackgroundMusic("Boss"); // ���� ���� BGM ���

            GameManager.instance.dontMove = true; // �÷��̾� ������ �Ұ���

            // HP�� Ȱ��ȭ
            Invoke("Show_HpBar", 2f);

            director_Show.Play(); // ���� ���� �ƾ� ���

            // ī�޶� ��ȯ - ���� ī�޶�
            Invoke("ChangeBattaleCam", 5.5f);

            // �� �ݱ�
            leftDoor.DoorClose();
            rightDoor.DoorClose();
        }
    }

    // ����� ���� �Լ�
    public override void TakeDamage(int damage, Transform Pos, bool isCritical)
    {
        // �׾��ų� ������ ���۵��� �ʾҴٸ� ����
        if (isDie || !isBattleStart)
        {
            return;
        }

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
        isDie = true; // ���� ���

        // ���� ���� ����
        StopCoroutine(laserAttackCoroutine);
        StopCoroutine(setShadowBallCoroutine);
        StopCoroutine(swordAttackCoroutine);

        GameManager.instance.canHitPlayer = false; // �÷��̾� ����

        // �״� ����
        SoundManager.instance.PlaySound("EnemyDie");

        skellBoss_Anim.SetBool("isBattle", false);
        SoundManager.instance.StopBackgroundMusic();
        isBattleStart = false;

        // HP�� ��Ȱ��ȭ
        hp_Bar.SetActive(false);

        // �ƾ� ���
        director_BossDie.Play();

        // ��� ���� �ڷ�ƾ ���
        StartCoroutine("DieEffect");
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
            Debug.Log("�ݱ� ���"); // ������ �ݱ��� ���
            GameObject bullion = ObjectPoolingManager.instance.GetObject("Item_Bullion"); // ������Ʈ Ǯ���� �ݱ� �뿩
            bullion.transform.position = this.transform.position; // ��ġ �ʱ�ȭ
        }
    }

    // ü�¹� Ȱ��ȭ �Լ�
    public void Show_HpBar()
    {
        hp_Bar.SetActive(true); // HP �� Ȱ��ȭ
    }

    // ī�޶� ��ȯ �Լ� ( ���� ķ���� ��ȯ )
    public void ChangeBattaleCam()
    {
        // ī�޶� ��ȯ - ���� ī�޶�
        defaultCam.SetActive(false);
        battleCam.SetActive(true);

        GameManager.instance.dontMove = false; // �÷��̾� ������ ����
    }

    // ������ ���� �ڷ�ƾ
    IEnumerator LaserAttack()
    {
        int rand_ShotNum = Random.Range(min_LaserShotNum - 1, max_LaserShotNum - 1);

        for (int i = 0; i < rand_ShotNum + 1; i++)
        {
            // �׾��ٸ� ����
            if (isDie)
            {
                yield break;
            }

            // ��� ������ üũ
            if (isLeftHand)
            {
                // �� ��ġ �̵�
                leftHand.SetHandPos();

                yield return new WaitForSeconds(0.5f);

                // ���� �ִϸ��̼� ���
                leftHand.anim.SetTrigger("Attack");

                yield return new WaitForSeconds(0.75f);

                // ������ �߻� �Լ� ����
                leftHand.laser.LaserShot();
            }
            else
            {
                // �� ��ġ �̵�
                rightHand.SetHandPos();

                yield return new WaitForSeconds(0.5f);

                // ���� �ִϸ��̼� ���
                rightHand.anim.SetTrigger("Attack");

                yield return new WaitForSeconds(0.75f);

                // ������ �߻� �Լ� ����
                rightHand.laser.LaserShot();
            }

            isLeftHand = !isLeftHand; // �� ����
        }

        yield return new WaitForSeconds(1f);

        isSetPattern = false; // ���� ���� ���� üũ ����
    }

    // źȯ �߻� �Լ�
    IEnumerator BulletAttack()
    {
        // �� ������
        head_Anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.5f);

        // źȯ �߻�
        StartCoroutine("SetShadowBall");

        yield return new WaitForSeconds(shotTime);

        // �� �ݱ�
        head_Anim.SetBool("isAttack", false);

        yield return new WaitForSeconds(1f);

        isSetPattern = false; // ���� ���� ���� üũ ����
    }

    // ��� ��ȯ �Լ�
    IEnumerator SwordAttack()
    {
        // �� ����
        for (int i = 0; i < bossSword_BulletPos.Length; i++)
        {
            // �׾��ٸ� ����
            if (isDie)
            {
                yield break;
            }

            SoundManager.instance.PlaySound("CreateSword");

            // ������Ʈ Ǯ���� �����캼 ������ �뿩
            GameObject bossSword = ObjectPoolingManager.instance.GetObject("Bullet_BossSword");
            bs[i] = bossSword.GetComponent<BossSword>();

            bs[i].speed = bossSwordSpeed; // �� �ӵ�
            bs[i].distance = bossSwordlDistance; // �� ��Ÿ�
            bs[i].targetTransform = targetTransform; // �÷��̾� ��ġ
            bs[i].bulletPos = bossSword_BulletPos[i]; // �� �߻� ��ġ
            bs[i].shooterPos = transform; // �߻��� ��ü�� ��ġ
            bs[i].damage = bossSword_Damage; // �� ���ݷ�
            bs[i].failCause = "�����˿��� �й�"; // ��� ����

            bs[i].createAnim.SetTrigger("Create"); // �� ���� �ִϸ��̼� ���
            bs[i].chargeAnim.SetBool("isCharge", true); // �� ���� �ִϸ��̼� ���

            bs[i].Setting(); // ����

            yield return new WaitForSeconds(bossSword_shotTime / bossSword_BulletPos.Length);
        }

        yield return new WaitForSeconds(1f);

        // �� �߻�
        for (int j = 0; j < bossSword_BulletPos.Length; j++)
        {
            // �׾��ٸ� ����
            if (isDie)
            {
                // �߻� ��� ���� �� ��� ����
                for (int k = 0; k < bossSword_BulletPos.Length; k++)
                {
                    bs[k].gameObject.SetActive(false);
                }

                yield break;
            }

            bs[j].Shot(); // �߻�

            bs[j].anim.SetBool("isShot", true); // �� �ִϸ��̼� ���
            bs[j].chargeAnim.SetBool("isCharge", false); // ���� �ִϸ��̼� ����

            yield return new WaitForSeconds(0.275f);
        }


        yield return new WaitForSeconds(1f);

        isSetPattern = false; // ���� ���� ���� üũ ����
    }

    // �����캼 ���� �Լ�
    IEnumerator SetShadowBall()
    {
        // �����캼 ����
        for (int i = 0; i <= shotShadowBallNum; i++)
        {
            // �׾��ٸ� ����
            if (isDie)
            {
                yield break;
            }

            // 4�������� ����
            for (int k = 0; k < 4; k++)
            {
                // ������Ʈ Ǯ���� �����캼 ������ �뿩
                GameObject shadowBall = ObjectPoolingManager.instance.GetObject("Bullet_ShadowBall");
                sb[i * 4 + k] = shadowBall.GetComponent<ShadowBall>();

                // �����캼 ���� ����
                sb[i * 4 + k].speed = shadowBallSpeed; // �����캼 �ӵ�
                sb[i * 4 + k].distance = shadowBallDistance; // �����캼 ��Ÿ�
                sb[i * 4 + k].aimPos = axisPos; // �����캼 �߻� ����
                sb[i * 4 + k].bulletPos = bulletPos; // �����캼 �߻� ��ġ
                sb[i * 4 + k].shooterPos = transform; // �߻��� ��ü�� ��ġ
                sb[i * 4 + k].damage = attackDamage; // ���ݷ�
                sb[i * 4 + k].failCause = "�����˿��� �й�"; // ��� ����

                // ���̾ ���� �� �߻�
                sb[i * 4 + k].Setting();
                sb[i * 4 + k].Shot();

                axisPos.transform.Rotate(new Vector3(0, 0, 90)); // 4�������� ��� ���� ȸ��
            }

            SoundManager.instance.PlaySound("ShadowBallShot"); // źȯ �߻� ���� ���

            // ��� ��
            yield return new WaitForSeconds(shotTime / shotShadowBallNum);

            axisPos.transform.Rotate(new Vector3(0, 0, rotateAngle)); // ȸ��
        }

        axisPos.rotation = new Quaternion(0, 0, 0, 0); // ���� �ʱ�ȭ   
    }

    // ��� ����
    IEnumerator DieEffect()
    {
        GameManager.instance.dontMove = true; // �÷��̾� ������ ����

        Time.timeScale = 0.5f; // ���ο� ��� ȿ��
        Debug.Log("���ο�");

        yield return new WaitForSecondsRealtime(5f); // ���� �ð����� 5�� ��

        Time.timeScale = 1.0f; // ���ο� ��� ȿ�� ����
        Debug.Log("���ο� ����");

        bossDie_Particle.Play();

        // ��ƼŬ�� ����Ǵ� ���� 0.25�� ���� ���� ���
        while (bossDie_Particle.isPlaying)
        {
            SoundManager.instance.PlaySound("EnemyDie");

            yield return new WaitForSeconds(0.25f);
        }

        bossDieBoom_Particle.Play();
        SoundManager.instance.PlaySound("EnemyDie");

        // ���� �ذ� ��ȯ
        GameObject bossSkull = Instantiate(bossSkullPrefab);
        bossSkull.transform.position = bulletPos.position;

        // ���� ��� �ִϸ��̼�
        skellBoss_Anim.SetTrigger("Die");

        yield return new WaitForSeconds(2.5f);

        GameManager.instance.dontMove = false;

        // ī�޶� ��ȯ - �⺻ ī�޶�
        defaultCam.SetActive(true);
        battleCam.SetActive(false);

        // �÷��̾� ���� ����
        GameManager.instance.canHitPlayer = true;

        // [��� ó��]
        GameManager.instance.monsterInStageList.RemoveAt(GameManager.instance.monsterInStageList.Count - 1);

        // ���� ����Ʈ�� ���� 0�� �Ǿ��ٸ�
        if (GameManager.instance.monsterInStageList.Count == 0)
        {
            // ��Ż ���� ���� ���
            SoundManager.instance.PlaySound("OpenPortal");
        }

        // ����
        DropItem(); // ������ ���

        // �� ����
        leftDoor.DoorOpen();
        rightDoor.DoorOpen();
    }
}

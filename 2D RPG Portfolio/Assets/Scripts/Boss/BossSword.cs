using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSword : MonoBehaviour
{
    Rigidbody2D BossSwordRB; // �� ������ٵ�

    public Animator anim; // �� �ִϸ�����
    public Animator chargeAnim; // �� ���� ȿ�� �ִϸ�����
    public Animator createAnim; // �� ���� ȿ�� �ִϸ�����
    public Animator hitAnim; // �� �浹 ȿ�� �ִϸ�����

    public float speed; // �� �ӵ�
    public float distance; // �� ��Ÿ�

    public Transform shooterPos; // ���� �߻��� ��ü�� ��ġ ��

    public Transform aimPos; // ���޹��� �� �߻� ���� ��
    public Transform bulletPos; // ���޹��� �� �߻� ��ġ

    public Transform targetTransform; // ���޹��� �÷��̾� ��ġ ��

    public int damage; // ���޹��� ���ݷ�
    public string failCause; // ���޹��� ��� ����

    private bool isShot; // ���� �߻��ߴ��� üũ
    private bool isHit; // ���� �������� üũ
    private bool canHit; // �÷��̾ ������ �� �ִ� �������� üũ

    GameObject bossSwordHitFX; // �� �浹 �ִϸ��̼��� ����� ������Ʈ

    public Transform headTransform; // ���� �Ӹ� �κ� ��ġ

    RaycastHit2D hitInfo; // ���� �浹 ����

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        BossSwordRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ���� �߻��� ��ü�� ��ġ ���
        float dis = Vector2.Distance(shooterPos.position, transform.position);

        // ���� ��ġ�� �߻��� ��ü ��ġ�� ��Ÿ����� �� �־����� �� ���� �������� �ʾҴٸ�
        if (dis > distance && !isHit)
        {
            ExplosionBossSword(); // �� ����
        }

        AimPlayer(); // �÷��̾� ����

        Debug.DrawRay(bulletPos.position, (headTransform.position - transform.position) * 10f, Color.red);
    }

    private void OnEnable()
    {
        // �� �߻����� ���� üũ
        isShot = false;

        // �� ������ ���� üũ
        isHit = false;

        // �� �⺻ �ִϸ��̼�
        anim.SetBool("isShot", false);
    }

    // �� ��ġ ���� �Լ�
    public void Setting()
    {
        // �� ��ġ ����
        transform.position = bulletPos.position;
    }

    public void Shot()
    {
        // �� �߻�
        if (shooterPos.localScale.x > 0)
        {
            BossSwordRB.velocity = aimPos.up * speed; // �� �̵�
        }
        else if (shooterPos.localScale.x < 0)
        {
            BossSwordRB.velocity = aimPos.up * -speed; // �� �̵�
        }

        isShot = true; // �߻� üũ
        canHit = true; // �߻縦 �����Ƿ� ���� ����
    }

    // Ʈ���ſ� ����� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� �÷��̾ ���� �ʾҰ� ���� ������ �ʾҰ� ���� ������ ��
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDie && !isHit && canHit)
        {
            // �ǰ�
            GameManager.instance.failCause = failCause; // ��� ����

            canHit = false; // �ѹ� �ǰ� ���ϸ� ���� �ɷ� ���

            collision.GetComponent<PlayerManager>().TakeDamage(damage, transform, false); // ������ �μ� = ���ʹ� ũ��Ƽ�� ���� ����
        }
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷����� ��
        else if (collision.gameObject.CompareTag("Platform"))
        {
            ExplosionBossSword();
        }
    }

    // �� ���� �Լ�
    void ExplosionBossSword()
    {
        // �� �� �ִϸ��̼� ����
        anim.SetBool("isShot", false);

        // �� ����
        BossSwordRB.velocity = Vector2.zero;

        // �� ��Ʈ ����Ʈ �ִϸ��̼��� ����� ������Ʈ Ǯ���� �뿩
        bossSwordHitFX = ObjectPoolingManager.instance.GetObject("Effect_BossSwordHit");
        hitAnim = bossSwordHitFX.GetComponent<Animator>();

        // ���� �߻�
        hitInfo = Physics2D.Raycast(bulletPos.position, (headTransform.position - transform.position) * 10f);

        if (hitInfo)
        {
            // ��ġ ����
            bossSwordHitFX.transform.position = transform.position;

            // ���� ���� ( ���� �� �� ) ---------------------------------------------
            //bossSwordHitFX.transform.rotation = new Quaternion(0, 0, Quaternion.LookRotation(headTransform.position - transform.position, Vector3.right).z, bossSwordHitFX.transform.rotation.w);
            Debug.Log(Quaternion.LookRotation(hitInfo.normal, Vector2.right));
            Debug.Log(Quaternion.LookRotation(hitInfo.normal));
        }

        // �� ��Ʈ ����Ʈ �ִϸ��̼� ���
        hitAnim.SetTrigger("Hit");

        isHit = true; // �� ���� üũ

        // �� ��ȯ �Լ��� �̹� ȣ�� �Ǿ��ٸ� ����
        if (IsInvoking("RetrunBossSword"))
        {
            return;
        }

        Invoke("RetrunBossSword", 1f); // 1�� �� �� Ǯ�� ��ȯ
    }

    // �÷��̾� ���� �Լ�
    public void AimPlayer()
    {
        // ���� �߻��ߴٸ� ���� ����
        if (isShot)
        {
            return;
        }

        Vector3 dir = targetTransform.position - transform.position;

        // ���⿡ ���� �ٸ� ���� (��Ȯ�� ���� �Ʒ��� ������ �� ����� ���׸� ���� ����)
        if (transform.localScale.x > 0)
        {
            aimPos.transform.right = dir.normalized;
        }
        else if (transform.localScale.x < 0)
        {
            aimPos.transform.right = -dir.normalized;
        }

        // �� ���� ����
        if (shooterPos.localScale.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, aimPos.rotation.eulerAngles.z - 90);
        }
        else if (shooterPos.localScale.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, aimPos.rotation.eulerAngles.z + 90);
        }
    }

    void RetrunBossSword()
    {
        Debug.Log("�� ��ȯ!");
        gameObject.SetActive(false); // �� Ǯ�� ��ȯ
        bossSwordHitFX.SetActive(false); // �浹 ����Ʈ Ǯ�� ��ȯ
    }
}

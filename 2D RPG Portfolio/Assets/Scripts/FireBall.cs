using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Rigidbody2D fireBallRb; // ���̾ ������ٵ�
    Animator anim; // ���̾ �ִϸ�����

    public float speed; // ���̾ �ӵ�
    public float distance; // ���̾ ��Ÿ�

    public Transform shooterPos; // ���̾�� �߻��� ��ü�� ��ġ ��

    public Transform aimPos; // ���޹��� ���̾ �߻� ���� ��
    public Transform bulletPos; // ���޹��� ���̾ �߻� ��ġ

    public int damage; // ���޹��� ���ݷ�
    public string failCause; // ���޹��� ��� ����

    private bool isHit; // ���̾�� �������� üũ

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        fireBallRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ���̾�� �߻��� ��ü�� ��ġ ���
        float dis = Vector2.Distance(shooterPos.position, transform.position);

        // ���̾�� ��ġ�� �߻��� ��ü ��ġ�� ��Ÿ����� �� �־����� �� ���̾�� �������� �ʾҴٸ�
        if (dis > distance && !isHit)
        {
            ExplosionFireBall(); // ���̾ ����
        }
    }

    private void OnEnable()
    {
        // ���̾ ������ ���� üũ
        isHit = false;

        // ���̾ �⺻ �ִϸ��̼�
        anim.SetBool("isDefault", true);
    }

    // ���̾ ��ġ, ���� ���� �Լ�
    public void Setting()
    {
        // ���̾�� ��ġ ����
        transform.position = bulletPos.position;

        // ���̾�� ���� ����
        if (shooterPos.localScale.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, aimPos.rotation.eulerAngles.z - 90);
        }
        else if (shooterPos.localScale.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, aimPos.rotation.eulerAngles.z + 90);
        }
    }

    public void Shot()
    {
        // ���̾ �߻�
        if (shooterPos.localScale.x > 0)
        {
            fireBallRb.velocity = aimPos.right * speed; // ���̾ �̵�
        }
        else if (shooterPos.localScale.x < 0)
        {
            fireBallRb.velocity = aimPos.right * -speed; // ���̾ �̵�
        }
    }

    // Ʈ���ſ� ����� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� �÷��̾ ���� �ʾҰ� ���̾�� ������ �ʾ��� ��
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDie && !isHit)
        {
            // �ǰ�
            GameManager.instance.failCause = failCause; // ��� ����

            collision.GetComponent<PlayerManager>().TakeDamage(damage, transform, false); // ������ �μ� = ���ʹ� ũ��Ƽ�� ���� ����

            ExplosionFireBall(); // ���̾ ����
        }
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷����� ��
        else if (collision.gameObject.CompareTag("Platform"))
        {
            ExplosionFireBall(); // ���̾ ����
        }
    }

    // ���̾ ���� �Լ�
    void ExplosionFireBall()
    {
        // ���̾ �⺻ �ִϸ��̼� ����
        anim.SetBool("isDefault", false);

        // ���̾ ���� �ִϸ��̼�
        anim.SetTrigger("Hit");

        // ���̾ ����
        fireBallRb.velocity = Vector2.zero;

        isHit = true; // ���̾ ���� üũ

        // ���̾ ��ȯ �Լ��� �̹� ȣ�� �Ǿ��ٸ� ����
        if (IsInvoking("RetrunFireBall"))
        {
            return;
        }

        Invoke("RetrunFireBall", 1f); // 1�� �� ���̾ Ǯ�� ��ȯ
    }

    void RetrunFireBall()
    {
        Debug.Log("���̾ ��ȯ!");
        gameObject.SetActive(false); // ���̾ Ǯ�� ��ȯ
    }
}

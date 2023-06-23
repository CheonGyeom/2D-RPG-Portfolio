using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBall : MonoBehaviour
{
    Rigidbody2D ShadowBallRB; // �����캼 ������ٵ�
    Animator anim; // �����캼 �ִϸ�����

    public float speed; // �����캼 �ӵ�
    public float distance; // �����캼 ��Ÿ�

    public Transform shooterPos; // �����캼�� �߻��� ��ü�� ��ġ ��

    public Transform aimPos; // ���޹��� �����캼 �߻� ���� ��
    public Transform bulletPos; // ���޹��� �����캼 �߻� ��ġ

    public int damage; // ���޹��� ���ݷ�
    public string failCause; // ���޹��� ��� ����

    private bool isHit; // �����캼�� �������� üũ

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        ShadowBallRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // �����캼�� �߻��� ��ü�� ��ġ ���
        float dis = Vector2.Distance(shooterPos.position, transform.position);

        // �����캼�� ��ġ�� �߻��� ��ü ��ġ�� ��Ÿ����� �� �־����� �� �����캼�� �������� �ʾҴٸ�
        if (dis > distance && !isHit)
        {
            ExplosionShadowBall(); // �����캼 ����
        }
    }

    private void OnEnable()
    {
        // �����캼 ������ ���� üũ
        isHit = false;

        // �����캼 �⺻ �ִϸ��̼�
        anim.SetBool("isDefault", true);
    }

    // �����캼 ��ġ, ���� ���� �Լ�
    public void Setting()
    {
        // �����캼�� ��ġ ����
        transform.position = bulletPos.position;

        // �����캼�� ���� ����
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
        // �����캼 �߻�
        if (shooterPos.localScale.x > 0)
        {
            ShadowBallRB.velocity = aimPos.right * speed; // �����캼 �̵�
        }
        else if (shooterPos.localScale.x < 0)
        {
            ShadowBallRB.velocity = aimPos.right * -speed; // �����캼 �̵�
        }
    }

    // Ʈ���ſ� ����� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� �÷��̾ ���� �ʾҰ� �����캼�� ������ �ʾ��� ��
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDie && !isHit)
        {
            // �ǰ�
            GameManager.instance.failCause = failCause; // ��� ����

            collision.GetComponent<PlayerManager>().TakeDamage(damage, transform, false); // ������ �μ� = ���ʹ� ũ��Ƽ�� ���� ����

            ExplosionShadowBall(); // �����캼 ����
        }
    }

    // �����캼 ���� �Լ�
    void ExplosionShadowBall()
    {
        // �����캼 �⺻ �ִϸ��̼� ����
        anim.SetBool("isDefault", false);

        // �����캼 ���� �ִϸ��̼�
        anim.SetTrigger("Hit");

        // �����캼 ����
        ShadowBallRB.velocity = Vector2.zero;

        isHit = true; // �����캼 ���� üũ

        // �����캼 ��ȯ �Լ��� �̹� ȣ�� �Ǿ��ٸ� ����
        if (IsInvoking("RetrunShadowBall"))
        {
            return;
        }

        Invoke("RetrunShadowBall", 1f); // 1�� �� �����캼 Ǯ�� ��ȯ
    }

    void RetrunShadowBall()
    {
        Debug.Log("�����캼 ��ȯ!");
        gameObject.SetActive(false); // �����캼 Ǯ�� ��ȯ
    }
}

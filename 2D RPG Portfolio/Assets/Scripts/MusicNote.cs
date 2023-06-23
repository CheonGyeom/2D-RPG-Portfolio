using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNote : MonoBehaviour
{
    Rigidbody2D musicNoteRb; // ��ǥ ������ٵ�
    Animator anim; // ��ǥ �ִϸ�����

    public float speed; // ��ǥ �ӵ�
    public float distance; // ��ǥ ��Ÿ�

    public Transform shooterPos; // ��ǥ�� �߻��� ��ü�� ��ġ ��

    public Transform aimPos; // ���޹��� ��ǥ �߻� ���� ��
    public Transform bulletPos; // ���޹��� ��ǥ �߻� ��ġ

    public Transform shotRotation; // ������ �߻��� ����

    public int damage; // ���޹��� ���ݷ�
    public string failCause; // ���޹��� ��� ����

    private bool isHit; // ��ǥ�� �������� üũ

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        musicNoteRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ��ǥ�� �߻��� ��ü�� ��ġ ���
        float dis = Vector2.Distance(shooterPos.position, transform.position);

        // ��ǥ�� ��ġ�� �߻��� ��ü ��ġ�� ��Ÿ����� �� �־����� �� ��ǥ�� �������� �ʾҴٸ�
        if (dis > distance && !isHit)
        {
            ExplosionFireBall(); // ��ǥ ����
        }
    }

    private void OnEnable()
    {
        // ��ǥ ������ ���� üũ
        isHit = false;

        // ��ǥ �⺻ �ִϸ��̼�
        anim.SetBool("isDefault", true);
    }

    // ��ǥ ��ġ ���� �Լ�
    public void Setting()
    {
        // ��ǥ�� ��ġ ����
        transform.position = bulletPos.position;

        // ��ǥ�� ���� ����
        shotRotation.rotation = aimPos.rotation;
    }

    public void Shot()
    {
        // ��ǥ �߻�
        if (shooterPos.localScale.x > 0)
        {
            musicNoteRb.velocity = shotRotation.right * speed; // ��ǥ �̵�
        }
        else if (shooterPos.localScale.x < 0)
        {
            musicNoteRb.velocity = shotRotation.right * -speed; // ��ǥ �̵�
        }
    }

    // Ʈ���ſ� ����� ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ʈ���Ÿ� �ߵ���Ų ������Ʈ�� �±װ� �÷��̾��̰� �÷��̾ ���� �ʾҰ� ��ǥ�� ������ �ʾ��� ��
        if (collision.gameObject.CompareTag("Player") && !GameManager.instance.isDie && !isHit)
        {
            // �ǰ�
            GameManager.instance.failCause = failCause; // ��� ����

            collision.GetComponent<PlayerManager>().TakeDamage(damage, transform, false); // ������ �μ� = ���ʹ� ũ��Ƽ�� ���� ����

            ExplosionFireBall(); // ��ǥ ����
        }
    }

    // ��ǥ ���� �Լ�
    void ExplosionFireBall()
    {
        // ��ǥ �⺻ �ִϸ��̼� ����
        anim.SetBool("isDefault", false);

        // ��ǥ ���� �ִϸ��̼�
        anim.SetTrigger("Hit");

        // ��ǥ ����
        musicNoteRb.velocity = Vector2.zero;

        isHit = true; // ��ǥ ���� üũ

        // ��ǥ ��ȯ �Լ��� �̹� ȣ�� �Ǿ��ٸ� ����
        if (IsInvoking("RetrunMusicNote"))
        {
            return;
        }

        Invoke("RetrunMusicNote", 1f); // 1�� �� ��ǥ Ǯ�� ��ȯ
    }

    void RetrunMusicNote()
    {
        Debug.Log("��ǥ ��ȯ!");
        gameObject.SetActive(false); // ��ǥ Ǯ�� ��ȯ
    }
}

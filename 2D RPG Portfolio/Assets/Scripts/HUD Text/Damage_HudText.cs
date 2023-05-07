using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_HudText : MonoBehaviour
{
    public TextMesh hudText; // ����ؽ�Ʈ
    Color alpha; // ����

    public float moveSpeed; // ����ؽ�Ʈ�� �����̴� �ӵ�
    public float alphaSpeed; // ����ؽ�Ʈ�� ���������� �ӵ�
    public int damage; // �ؽ�Ʈ�� �ݿ��� �����

    void Awake()
    {
        hudText = GetComponent<TextMesh>();
        alpha = hudText.color;
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // �̵��ӵ� ��ŭ �̵�
        alpha.a = Mathf.Lerp(alpha.a, 0, alphaSpeed * Time.deltaTime); // ���������� �̿��ؼ� �ε巴�� ���� ����
        hudText.color = alpha; // ���� �ݿ�
    }
    private void OnEnable()
    {
        transform.parent.gameObject.transform.position = new Vector3(0, 0, 0); // �θ� ������Ʈ ��ġ �ʱ�ȭ
        transform.position = new Vector3(0, 0, 0); // �ؽ�Ʈ ��ġ �ʱ�ȭ
        alpha.a = 255; // ���� �ʱ�ȭ

        Invoke("DestroyText", 5f);
    }

    public void ShowDamageText(int damage)
    {
        hudText.text = damage.ToString(); // �ؽ�Ʈ ����� �ݿ�
    }

    public void DestroyText()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}

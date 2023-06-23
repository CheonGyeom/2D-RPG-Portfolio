using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHealth_HudText : MonoBehaviour
{
    TextMesh hudText; // ����ؽ�Ʈ
    Color alpha; // ����

    public float moveSpeed; // ����ؽ�Ʈ�� �����̴� �ӵ�
    public float alphaSpeed; // ����ؽ�Ʈ�� ���������� �ӵ�
    public int health; // �ؽ�Ʈ�� �ݿ��� HP ȹ�淮

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

    public void ShowGetHealthText(int health)
    {
        hudText.text = $"+{health}HP"; // �ؽ�Ʈ HP ȹ�淮 �ݿ�
    }

    public void DestroyText()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHealthHudText_Shadow : MonoBehaviour
{
    TextMesh hudText; // ����ؽ�Ʈ
    Color alpha; // ����

    public float alphaSpeed; // ����ؽ�Ʈ�� ���������� �ӵ�
    public int health; // �ؽ�Ʈ�� �ݿ��� HP ȹ�淮

    void Awake()
    {
        hudText = GetComponent<TextMesh>();
        alpha = hudText.color;
    }
    void Update()
    {
        alpha.a = Mathf.Lerp(alpha.a, 0, alphaSpeed * Time.deltaTime); // ���������� �̿��ؼ� �ε巴�� ���� ����
        hudText.color = alpha; // ���� �ݿ�
    }
    private void OnEnable()
    {
        alpha.a = 255; // ���� �ʱ�ȭ
    }

    public void ShowGetHealthText(int health)
    {
        hudText.text = $"+{health}HP"; // �ؽ�Ʈ HP ȹ�淮 �ݿ�
    }
}

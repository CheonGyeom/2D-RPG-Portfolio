using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHudText_Shadow : MonoBehaviour
{
    TextMesh hudText; // ����ؽ�Ʈ
    Color alpha; // ����

    public float alphaSpeed; // ����ؽ�Ʈ�� ���������� �ӵ�
    public int damage; // �ؽ�Ʈ�� �ݿ��� �����

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

    public void ShowDamageText(int damage)
    {
        hudText.text = damage.ToString(); // �ؽ�Ʈ ����� �ݿ�
    }
}

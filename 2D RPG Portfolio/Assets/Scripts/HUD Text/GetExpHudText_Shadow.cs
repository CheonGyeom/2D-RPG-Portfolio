using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetExpHudText_Shadow : MonoBehaviour
{
    TextMesh hudText; // ����ؽ�Ʈ
    Color alpha; // ����

    public float alphaSpeed; // ����ؽ�Ʈ�� ���������� �ӵ�
    public int exp; // �ؽ�Ʈ�� �ݿ��� ����ġ ȹ�淮

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

    public void ShowGetExpText(int exp)
    {
        hudText.text = $"+{exp.ToString()}xp"; // �ؽ�Ʈ ����ġ ȹ�淮 �ݿ�
    }
}


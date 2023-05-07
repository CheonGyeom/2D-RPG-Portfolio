using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGoldHudText_Shadow : MonoBehaviour
{
    TextMesh hudText; // ����ؽ�Ʈ
    Color alpha; // ����

    public float alphaSpeed; // ����ؽ�Ʈ�� ���������� �ӵ�
    public int gold; // �ؽ�Ʈ�� �ݿ��� ��� ȹ�淮

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

    public void ShowGetGoldText(int gold)
    {
        hudText.text = $"+{gold.ToString()}G"; // �ؽ�Ʈ ��� ȹ�淮 �ݿ�
    }
}

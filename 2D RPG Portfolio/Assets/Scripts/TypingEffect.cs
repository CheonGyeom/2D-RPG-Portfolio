using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public int typingPerSeconds; // �ʴ� Ÿ���εǴ� ���� ��
    public TMP_Text typingText; // �ؽ�Ʈ ������Ʈ
    public GameObject talkCursor; // ��ȭâ ���� Ŀ��
    public bool isTyping; // ���� Ÿ������ ����ǰ� �ִ��� üũ

    Animator talkCursorAnim; // ��ȭâ ���� Ŀ�� �ִϸ�����
    float intervar; // ���� Ÿ���α����� ����
    string targetMessage; // Ÿ���� �� ����
    int typingIndex; // Ÿ���� �ε���

    private void Awake()
    {
        // ������Ʈ �Ҵ�
        typingText = GetComponent<TMP_Text>();
        talkCursorAnim = talkCursor.GetComponent<Animator>();
    }

    // ���� �ʱ�ȭ �Լ�
    public void SetMessage(string msg)
    {
        // Ÿ���� �߿� �Է��� ���Դٸ�
        if (isTyping)
        {
            typingText.text = targetMessage; // Ÿ���� ��ŵ
            CancelInvoke("Effecting"); // Ÿ���� ����
            EffectEnd(); // Ÿ���� ����
        }
        else
        {
            targetMessage = msg; // Ÿ�� ���� ����
            EffectStart(); // Ÿ���� ����
        }
    }

    // Ÿ���� ���� �Լ�
    void EffectStart()
    {
        typingText.text = ""; // Ÿ������ ���� �ؽ�Ʈ ����
        typingIndex = 0; // Ÿ���� �ε��� �ʱ�ȭ

        intervar = 1.0f / typingPerSeconds; // ���� Ÿ���α����� ���� ���
        Invoke("Effecting", intervar); // Ÿ���� ����

        isTyping = true; // ���� Ÿ���� ���� ��

        talkCursorAnim.SetBool("isBlink", false); // Ÿ���� �߿� ���� Ŀ�� ������ �ִϸ��̼� ����
    }

    // Ÿ���� ���� �Լ�
    void Effecting()
    {
        // �ؽ�Ʈ�� Ÿ�� ���ڿ� ������ 
        if (typingText.text == targetMessage)
        {
            EffectEnd(); // Ÿ���� ����
            return;
        }

        typingText.text += targetMessage[typingIndex]; // �ؽ�Ʈ�� Ÿ���� �ε����� ���ڸ� ������

        // Ÿ���� �� ���ڰ� ���� �Ǵ� ��ħǥ�� ��쿣 ����ó��
        if (targetMessage[typingIndex] != ' ' || targetMessage[typingIndex] != '.')
        {
            SoundManager.instance.PlaySound("Typing"); // Ÿ���� ȿ���� ���
        }

        typingIndex++; // Ÿ���� �ε��� ����

        Invoke("Effecting", intervar); // ����Լ��� ���� Ÿ���� ����
    }

    // Ÿ���� ���� �Լ�
    void EffectEnd()
    {
        // ��ȭ ������ �ؾ��Ѵٸ�
        if (GameManager.instance.isChoiceTalk)
        {
            GameManager.instance.choicePanel.SetActive(true); // ��ȭ ����â Ȱ��ȭ
        }
        else
        {
            GameManager.instance.choicePanel.SetActive(false); // ��ȭ ����â ��Ȱ��ȭ
        }

        talkCursorAnim.SetBool("isBlink", true); // ���� Ŀ�� ������ �ִϸ��̼� ���

        isTyping = false; // Ÿ���� ���� ���� �ƴ�
    }
}

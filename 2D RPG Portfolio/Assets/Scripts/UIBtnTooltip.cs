using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIBtnTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text uiTooltipDescriptionText; // UI ���� ���� �ؽ�Ʈ

    public string uiTooltipDescription; // UI ���� ����


    // �����Ͱ� ���� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip(); // ���� Ȱ��ȭ
    }

    // �����Ͱ� ������ ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.instance.uiTooltipObject.SetActive(false); // ���� �г� �ݱ�
    }

    void ShowTooltip()
    {
        GameManager.instance.uiTooltipObject.SetActive(true); // ���� �г� Ȱ��ȭ

        GameManager.instance.uiTooltipObject.transform.position = transform.position; // ���� ��ġ ����
        uiTooltipDescriptionText.text = uiTooltipDescription; // ���� ���� ����
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DataSlot : MonoBehaviour
{
    public TMP_Text[] dataText; // ���� �� ������ ������ ��Ÿ���� �ؽ�Ʈ

    bool[] savefile = new bool[3]; // ���̺����� ���� ����

    private void OnEnable()
    {
        UpdateSlotDataText(); // ���� ������ �ؽ�Ʈ ����
    }

    // ������ ���� ���
    public void Slot(int number)
    {
        DataManager.instance.nowSlot = number; // �ش� ���� ����

        if (savefile[number]) // �����Ͱ� ������
        {
            // �ش� ���� ������ �ҷ�����
            DataManager.instance.LoadData();

            // ������ �����ϱ�
            Debug.Log("�����͸� �ҷ���");
            GameManager.instance.ApplyData();

            if (DataManager.instance.nowPlayer.tutorial_Complete) // Ʃ�丮���� �Ϸ��ߴٸ�
            {
                // ��������
                Debug.Log("��������");
                GameManager.instance.SetLoadScene("Town");
            }
            else // Ʃ�丮���� �Ϸ����� �ʾҴٸ�
            {
                // Ʃ�丮���
                Debug.Log("Ʃ�丮���");
                GameManager.instance.SetLoadScene("Tutorial");
            }
        }
        else // �����Ͱ� ������
        {
            // �� ���̺����� ����
            Debug.Log("���̺����� ����");
            DataManager.instance.nowPlayer = new PlayerData();
            DataManager.instance.SaveData();

            // ������ �����ϱ�
            Debug.Log("�����͸� �ҷ���");
            GameManager.instance.ApplyData();

            // Ʃ�丮���
            Debug.Log("Ʃ�丮���");
            GameManager.instance.SetLoadScene("Tutorial");
        }

    }

    // ���� ���̺� ���� ����
    public void DeleteSavefile(int number)
    {
        DataManager.instance.nowSlot = number; // �ش� ���� ����

        DataManager.instance.DeleteSavefile(); // ���̺� ���� ����

        UpdateSlotDataText(); // ���� ������ �ؽ�Ʈ ����
    }

    // ���� ������ �ؽ�Ʈ ����
    public void UpdateSlotDataText()
    {
        // ���Ժ��� ����� �����Ͱ� �ִ��� �˻�
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}")) // �����Ͱ� �ִ� ���
            {
                savefile[i] = true; // ������ ����
                DataManager.instance.nowSlot = i; // �ش� ���� ����
                DataManager.instance.LoadData(); // ������ ���� ������ �ҷ�����
                
                // ������ �� ���� 1�̻��̸�
                if (DataManager.instance.nowPlayer.highFloor > 0)
                {
                    dataText[i].text =
                    $"< �÷��� �ð� >\n{(DataManager.instance.nowPlayer.playTime / 3600).ToString("00") + "H " + ((DataManager.instance.nowPlayer.playTime % 3600) / 60).ToString("00") + "M"}\n\n< ������ �� >\n{DataManager.instance.nowPlayer.highFloor}F\n\n< ���� >\n{DataManager.instance.nowPlayer.level + 1}LV\n\n< ������ >\n{string.Format("{0:n0}G", DataManager.instance.nowPlayer.gold)}"; // ���� ������ ���� �ؽ�Ʈ ǥ��
                }
                else
                {
                    dataText[i].text =
                    $"< �÷��� �ð� >\n{(DataManager.instance.nowPlayer.playTime / 3600).ToString("00") + "H " + ((DataManager.instance.nowPlayer.playTime % 3600) / 60).ToString("00") + "M"}\n\n< ���� >\n{DataManager.instance.nowPlayer.level + 1}LV\n\n< ������ >\n{string.Format("{0:n0}G", DataManager.instance.nowPlayer.gold)}"; // ���� ������ ���� �ؽ�Ʈ ǥ��
                }
            }
            else // �����Ͱ� ���� ���
            {
                savefile[i] = false; // ������ ����
                dataText[i].text = "������ ����";
            }
        }
        // ���Կ� ������ ������ ǥ���ϱ� ���� �����Ϳ��� ������ ������ �ʱ�ȭ
        DataManager.instance.DataClear();
    }
}

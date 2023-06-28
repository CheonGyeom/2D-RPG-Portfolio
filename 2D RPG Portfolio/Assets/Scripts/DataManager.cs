using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// ������ ������ Ŭ����
public class PlayerData
{
    public int level; // �÷��̾� ����
    public int highFloor; // ������ ��
    public float playTime; // �÷��� �ð�
    public float gold; // ������
    public float exp; // ȹ���� ����ġ
    public Item[] item; // ������ ������
    public bool tutorial_Complete; // Ʃ�丮�� �Ϸ� ����
}

public class DataManager : MonoBehaviour
{
    // �̱��� ����
    public static DataManager instance;

    public PlayerData nowPlayer = new PlayerData(); // �÷��̾� ������ ����

    public string path; // ������ ���� ���
    public int nowSlot; // �� ���� ��ȣ


    // �̱��� ����
    private void Awake()
    {
        // �̱��� ������ ���������
        if (instance == null)
        {
            // �ڽ��� �Ҵ�
            instance = this;
        }
        // �̱��� ������ ������� ������
        else
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� DataManager�� �������� ����

        path = Application.persistentDataPath + "/Savefile"; // ��� ����
        Debug.Log(path);
    }

    // ������ ����
    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowPlayer); // ���� �÷��̾� �����͸� Json���� ��ȯ
        File.WriteAllText(path + nowSlot.ToString(), data); // Json�� ���� ��� ���Ͽ� ����
    }

    // ������ �ҷ�����
    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString()); // ���� ��� ���Ͽ��� Json�� �о��
        nowPlayer = JsonUtility.FromJson<PlayerData>(data); // �о�� Json�� PlayerData�������� ��ȯ�ؼ� ���� �÷��̾� �����Ϳ� ����
    }

    // ���� �ҷ��� ������ ����
    public void DataClear()
    {
        nowSlot = -1; // ���� ���� ����
        nowPlayer = new PlayerData(); // ������ �ʱ�ȭ
    }

    // ���̺� ���� �ʱ�ȭ
    public void DeleteSavefile()
    {
        File.Delete(path + nowSlot.ToString()); // ���� ��� ���� ����
    }
}

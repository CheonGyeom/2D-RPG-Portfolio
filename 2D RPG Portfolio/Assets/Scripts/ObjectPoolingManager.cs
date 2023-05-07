using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolingManager : MonoBehaviour
{
    // ������Ʈ Ǯ�� �Ŵ����� ���� ��ũ��Ʈ���� ȣ��Ǿ���ϱ� ������ �̱��� ���� ���
    public static ObjectPoolingManager instance; // �̱����� �Ҵ��� ��������

    // ������ ����
    public GameObject enemy_Skull_Prefab;

    public GameObject item_Coin_Prefab;
    public GameObject item_Bullion_Prefab;
    public GameObject item_Exp_Prefab;

    public GameObject hudText_GetGoldText_Prefab;
    public GameObject hudText_GetExpText_Prefab;
    public GameObject hudText_DamageText_Prefab;
    public GameObject hudText_CriticalDamText_Prefab;

    public GameObject equipItem_GraveSword_Prefab;
    public GameObject equipItem_InfernoBlade_Prefab;
    public GameObject equipItem_AmethystAbyssSword_Prefab;
    public GameObject equipItem_SpringShoes_Prefab;
    public GameObject equipItem_WhiteWing_Prefab;
    public GameObject equipItem_ClothArmor_Prefab;
    public GameObject equipItem_ChainArmor_Prefab;
    public GameObject equipItem_TitaniumBreastplate_Prefab;
    public GameObject equipItem_JetPack_Prefab;
    public GameObject equipItem_MetalShoes_Prefab;

    public GameObject bullet_Arrow_Prefab;

    public GameObject effect_ArrowHit_Prefab;

    public GameObject object_SkelHead_Prefab;
    public GameObject object_BoxPiece_Prefab;
    public GameObject object_BigBoxPiece_Prefab;

    // �迭 ����
    GameObject[] enemy_Skull; // ����

    GameObject[] item_Coin; // ����
    GameObject[] item_Bullion; // �ݱ�
    GameObject[] item_Exp; // ����ġ

    GameObject[] hudText_GetGoldText; // ��� ȹ�� ����ؽ�Ʈ
    GameObject[] hudText_GetExpText; // ����ġ ȹ�� ����ؽ�Ʈ
    GameObject[] hudText_DamageText; // ����� ����ؽ�Ʈ
    GameObject[] hudText_CriticalDamText; // ũ��Ƽ�� ����� ����ؽ�Ʈ

    GameObject[] equipItem_GraveSword; // �׷��̺� �ҵ� ������
    GameObject[] equipItem_InfernoBlade; // ���丣�� ���̵� ������
    GameObject[] equipItem_AmethystAbyssSword; // �ڼ��� �ɿ� �� ������
    GameObject[] equipItem_SpringShoes; // ������ �Ź� ������
    GameObject[] equipItem_WhiteWing; // �Ͼ� ���� ������
    GameObject[] equipItem_ClothArmor; // õ ���� ������
    GameObject[] equipItem_ChainArmor; // �罽 ���� ������
    GameObject[] equipItem_TitaniumBreastplate; // ƼŸ�� �䰩 ������
    GameObject[] equipItem_JetPack; // ��Ʈ�� ������
    GameObject[] equipItem_MetalShoes; // ��ö �Ź� ������

    GameObject[] bullet_Arrow; // ȭ�� �߻�ü

    GameObject[] effect_ArrowHit; // ȭ�� �浹 ����Ʈ

    GameObject[] object_SkelHead; // ���� �Ӹ� ������Ʈ
    GameObject[] object_BoxPiece; // �ڽ� ���� ������Ʈ
    GameObject[] object_BigBoxPiece; // ū �ڽ� ���� ������Ʈ

    GameObject[] targetPool; // ������Ʈ ���� ������ ���� ����

    private void Awake()
    {


        // �̱��� ����
        if (instance == null) // �̱��� ������ ���������
        {
            instance = this; // �ڽ��� �Ҵ�
        }
        else // �̱��� ������ ������� ������
        {
            Destroy(gameObject); // �ڽ��� �ı�
        }
        DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ObjectPoolingManager�� �������� ����

        // �̺�Ʈ ���
        SceneManager.sceneLoaded += LoadedsceneEvent;

        enemy_Skull = new GameObject[50];

        item_Coin = new GameObject[300];
        item_Bullion = new GameObject[100];
        item_Exp = new GameObject[300];

        hudText_GetGoldText = new GameObject[50];
        hudText_GetExpText = new GameObject[50];
        hudText_DamageText = new GameObject[50];
        hudText_CriticalDamText = new GameObject[50];

        equipItem_GraveSword = new GameObject[10];
        equipItem_InfernoBlade = new GameObject[10];
        equipItem_AmethystAbyssSword = new GameObject[10];
        equipItem_SpringShoes = new GameObject[10];
        equipItem_WhiteWing = new GameObject[10];
        equipItem_ClothArmor = new GameObject[10];
        equipItem_ChainArmor = new GameObject[10];
        equipItem_TitaniumBreastplate = new GameObject[10];
        equipItem_MetalShoes = new GameObject[10];
        equipItem_JetPack = new GameObject[10];

        bullet_Arrow = new GameObject[50];

        effect_ArrowHit = new GameObject[50];

        object_SkelHead = new GameObject[50];
        object_BoxPiece = new GameObject[50];
        object_BigBoxPiece = new GameObject[50];

        Generate(); // �ν��Ͻ� ����
    }

    // �ν��Ͻ� ���� �Լ�
    void Generate()
    {
        // �迭�� ���̸�ŭ ������Ʈ�� �̸� ����
        
        // ���� ����
        for (int i = 0; i < enemy_Skull.Length; i++)
        {
            enemy_Skull[i] = Instantiate(enemy_Skull_Prefab); // �ν��Ͻ� ����
            enemy_Skull[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(enemy_Skull[i]); // �� ���� �ÿ��� �������� ����
        }

        // ����
        for (int i = 0; i < item_Coin.Length; i++)
        {
            item_Coin[i] = Instantiate(item_Coin_Prefab); // �ν��Ͻ� ����
            item_Coin[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(item_Coin[i]); // �� ���� �ÿ��� �������� ����
        }

        // �ݱ�
        for (int i = 0; i < item_Bullion.Length; i++)
        {
            item_Bullion[i] = Instantiate(item_Bullion_Prefab); // �ν��Ͻ� ����
            item_Bullion[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(item_Bullion[i]); // �� ���� �ÿ��� �������� ����
        }

        // ����ġ
        for (int i = 0; i < item_Exp.Length; i++)
        {
            item_Exp[i] = Instantiate(item_Exp_Prefab); // �ν��Ͻ� ����
            item_Exp[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(item_Exp[i]); // �� ���� �ÿ��� �������� ����
        }

        // ��� ȹ�� ����ؽ�Ʈ
        for (int i = 0; i < hudText_GetGoldText.Length; i++)
        {
            hudText_GetGoldText[i] = Instantiate(hudText_GetGoldText_Prefab); // �ν��Ͻ� ����
            hudText_GetGoldText[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(hudText_GetGoldText[i]); // �� ���� �ÿ��� �������� ����
        }

        // ����ġ ȹ�� ����ؽ�Ʈ
        for (int i = 0; i < hudText_GetExpText.Length; i++)
        {
            hudText_GetExpText[i] = Instantiate(hudText_GetExpText_Prefab); // �ν��Ͻ� ����
            hudText_GetExpText[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(hudText_GetExpText[i]); // �� ���� �ÿ��� �������� ����
        }

        // ����� ����ؽ�Ʈ
        for (int i = 0; i < hudText_DamageText.Length; i++)
        {
            hudText_DamageText[i] = Instantiate(hudText_DamageText_Prefab); // �ν��Ͻ� ����
            hudText_DamageText[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(hudText_DamageText[i]); // �� ���� �ÿ��� �������� ����
        }

        // ũ��Ƽ�� ����� ����ؽ�Ʈ
        for (int i = 0; i < hudText_CriticalDamText.Length; i++)
        {
            hudText_CriticalDamText[i] = Instantiate(hudText_CriticalDamText_Prefab); // �ν��Ͻ� ����
            hudText_CriticalDamText[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(hudText_CriticalDamText[i]); // �� ���� �ÿ��� �������� ����
        }

        // �׷��̺� �ҵ� ������
        for (int i = 0; i < equipItem_GraveSword.Length; i++)
        {
            equipItem_GraveSword[i] = Instantiate(equipItem_GraveSword_Prefab); // �ν��Ͻ� ����
            equipItem_GraveSword[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_GraveSword[i]); // �� ���� �ÿ��� �������� ����
        }

        // ���丣�� ���̵� ������
        for (int i = 0; i < equipItem_InfernoBlade.Length; i++)
        {
            equipItem_InfernoBlade[i] = Instantiate(equipItem_InfernoBlade_Prefab); // �ν��Ͻ� ����
            equipItem_InfernoBlade[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_InfernoBlade[i]); // �� ���� �ÿ��� �������� ����
        }

        // �ڼ��� �ɿ� �� ������
        for (int i = 0; i < equipItem_AmethystAbyssSword.Length; i++)
        {
            equipItem_AmethystAbyssSword[i] = Instantiate(equipItem_AmethystAbyssSword_Prefab); // �ν��Ͻ� ����
            equipItem_AmethystAbyssSword[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_AmethystAbyssSword[i]); // �� ���� �ÿ��� �������� ����
        }

        // ������ �Ź� ������
        for (int i = 0; i < equipItem_SpringShoes.Length; i++)
        {
            equipItem_SpringShoes[i] = Instantiate(equipItem_SpringShoes_Prefab); // �ν��Ͻ� ����
            equipItem_SpringShoes[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_SpringShoes[i]); // �� ���� �ÿ��� �������� ����
        }

        // �Ͼ� ���� ������
        for (int i = 0; i < equipItem_WhiteWing.Length; i++)
        {
            equipItem_WhiteWing[i] = Instantiate(equipItem_WhiteWing_Prefab); // �ν��Ͻ� ����
            equipItem_WhiteWing[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_WhiteWing[i]); // �� ���� �ÿ��� �������� ����
        }

        // õ ���� ������
        for (int i = 0; i < equipItem_ClothArmor.Length; i++)
        {
            equipItem_ClothArmor[i] = Instantiate(equipItem_ClothArmor_Prefab); // �ν��Ͻ� ����
            equipItem_ClothArmor[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_ClothArmor[i]); // �� ���� �ÿ��� �������� ����
        }

        // �罽 ���� ������
        for (int i = 0; i < equipItem_ChainArmor.Length; i++)
        {
            equipItem_ChainArmor[i] = Instantiate(equipItem_ChainArmor_Prefab); // �ν��Ͻ� ����
            equipItem_ChainArmor[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_ChainArmor[i]); // �� ���� �ÿ��� �������� ����
        }

        // ƼŸ�� �䰩 ������
        for (int i = 0; i < equipItem_TitaniumBreastplate.Length; i++)
        {
            equipItem_TitaniumBreastplate[i] = Instantiate(equipItem_TitaniumBreastplate_Prefab); // �ν��Ͻ� ����
            equipItem_TitaniumBreastplate[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_TitaniumBreastplate[i]); // �� ���� �ÿ��� �������� ����
        }

        // ��Ʈ�� ������
        for (int i = 0; i < equipItem_JetPack.Length; i++)
        {
            equipItem_JetPack[i] = Instantiate(equipItem_JetPack_Prefab); // �ν��Ͻ� ����
            equipItem_JetPack[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_JetPack[i]); // �� ���� �ÿ��� �������� ����
        }

        // ��ö �Ź� ������
        for (int i = 0; i < equipItem_MetalShoes.Length; i++)
        {
            equipItem_MetalShoes[i] = Instantiate(equipItem_MetalShoes_Prefab); // �ν��Ͻ� ����
            equipItem_MetalShoes[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(equipItem_MetalShoes[i]); // �� ���� �ÿ��� �������� ����
        }

        // ȭ�� �߻�ü
        for (int i = 0; i < bullet_Arrow.Length; i++)
        {
            bullet_Arrow[i] = Instantiate(bullet_Arrow_Prefab); // �ν��Ͻ� ����
            bullet_Arrow[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(bullet_Arrow[i]); // �� ���� �ÿ��� �������� ����
        }

        // ȭ�� �浹 ����Ʈ
        for (int i = 0; i < effect_ArrowHit.Length; i++)
        {
            effect_ArrowHit[i] = Instantiate(effect_ArrowHit_Prefab); // �ν��Ͻ� ����
            effect_ArrowHit[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(effect_ArrowHit[i]); // �� ���� �ÿ��� �������� ����
        }

        // ���� �Ӹ� ������Ʈ
        for (int i = 0; i < object_SkelHead.Length; i++)
        {
            object_SkelHead[i] = Instantiate(object_SkelHead_Prefab); // �ν��Ͻ� ����
            object_SkelHead[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(object_SkelHead[i]); // �� ���� �ÿ��� �������� ����
        }

        // �ڽ� ���� ������Ʈ
        for (int i = 0; i < object_BoxPiece.Length; i++)
        {
            object_BoxPiece[i] = Instantiate(object_BoxPiece_Prefab); // �ν��Ͻ� ����
            object_BoxPiece[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(object_BoxPiece[i]); // �� ���� �ÿ��� �������� ����
        }

        // ū �ڽ� ���� ������Ʈ
        for (int i = 0; i < object_BigBoxPiece.Length; i++)
        {
            object_BigBoxPiece[i] = Instantiate(object_BigBoxPiece_Prefab); // �ν��Ͻ� ����
            object_BigBoxPiece[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(object_BigBoxPiece[i]); // �� ���� �ÿ��� �������� ����
        }
    }

    public GameObject GetObject(string type)
    {
        // ����ġ ���� ���� ���ϴ� Ÿ���� ������Ʈ Ǯ�� ����
        switch (type)
        {
            case "Enemy_Skull":
                targetPool = enemy_Skull;
                break;
            case "Item_Coin":
                targetPool = item_Coin;
                break;
            case "Item_Bullion":
                targetPool = item_Bullion;
                break;
            case "Item_Exp":
                targetPool = item_Exp;
                break;
            case "HudText_GetGold":
                targetPool = hudText_GetGoldText;
                break;
            case "HudText_GetExp":
                targetPool = hudText_GetExpText;
                break;
            case "HudText_Damage":
                targetPool = hudText_DamageText;
                break;
            case "HudText_CriticalDam":
                targetPool = hudText_CriticalDamText;
                break;
            case "EquipItem_GraveSword":
                targetPool = equipItem_GraveSword;
                break;
            case "EquipItem_InfernoBlade":
                targetPool = equipItem_InfernoBlade;
                break;
            case "EquipItem_AmethystAbyssSword":
                targetPool = equipItem_AmethystAbyssSword;
                break;
            case "EquipItem_SpringShoes":
                targetPool = equipItem_SpringShoes;
                break;
            case "EquipItem_WhiteWing":
                targetPool = equipItem_WhiteWing;
                break;
            case "EquipItem_ClothArmor":
                targetPool = equipItem_ClothArmor;
                break;
            case "EquipItem_ChainArmor":
                targetPool = equipItem_ChainArmor;
                break;
            case "EquipItem_TitaniumBreastplate":
                targetPool = equipItem_TitaniumBreastplate;
                break;
            case "EquipItem_JetPack":
                targetPool = equipItem_JetPack;
                break;
            case "EquipItem_MetalShoes":
                targetPool = equipItem_MetalShoes;
                break;
            case "Bullet_Arrow":
                targetPool = bullet_Arrow;
                break;
            case "Effect_ArrowHit":
                targetPool = effect_ArrowHit;
                break;
            case "Object_SkelHead":
                targetPool = object_SkelHead;
                break;
            case "Object_BoxPiece":
                targetPool = object_BoxPiece;
                break;
            case "Object_BigBoxPiece":
                targetPool = object_BigBoxPiece;
                break;
        }

        // ������ ������Ʈ Ǯ�� ������ ��Ȱ��ȭ ��(��� ������ ����) ������Ʈ�� ã��
        for (int i = 0; i < targetPool.Length; i++)
        {
            // ��Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true); // ������Ʈ Ȱ��ȭ
                return targetPool[i]; // ������Ʈ �뿩
            }
        }

        // ������ ������ ������Ʈ�� ã�� ���ߴٸ�
        return null; // �ƹ��͵� ��ȯ���� ����
    }

    // �� ���� �� �ߵ� �̺�Ʈ
    private void LoadedsceneEvent(Scene arg0, LoadSceneMode arg1)
    {
        // ��� ������Ʈ Ǯ�� ������ Ȱ��ȭ �� ������Ʈ�� ã��

        // ����
        for (int i = 0; i < item_Coin.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (item_Coin[i].activeSelf)
            {
                item_Coin[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ����ġ
        for (int i = 0; i < item_Exp.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (item_Exp[i].activeSelf)
            {
                item_Exp[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �ݱ�
        for (int i = 0; i < item_Bullion.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (item_Bullion[i].activeSelf)
            {
                item_Bullion[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �׷��̺� �ҵ�
        for (int i = 0; i < equipItem_GraveSword.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_GraveSword[i].activeSelf)
            {
                equipItem_GraveSword[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ���丣�� ���̵�
        for (int i = 0; i < equipItem_InfernoBlade.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_InfernoBlade[i].activeSelf)
            {
                equipItem_InfernoBlade[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �ڼ��� �ɿ� ��
        for (int i = 0; i < equipItem_AmethystAbyssSword.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_AmethystAbyssSword[i].activeSelf)
            {
                equipItem_AmethystAbyssSword[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ������ �Ź�
        for (int i = 0; i < equipItem_SpringShoes.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_SpringShoes[i].activeSelf)
            {
                equipItem_SpringShoes[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �Ͼ� ����
        for (int i = 0; i < equipItem_WhiteWing.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_WhiteWing[i].activeSelf)
            {
                equipItem_WhiteWing[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // õ ����
        for (int i = 0; i < equipItem_ClothArmor.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_ClothArmor[i].activeSelf)
            {
                equipItem_ClothArmor[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �罽 ����
        for (int i = 0; i < equipItem_ChainArmor.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_ChainArmor[i].activeSelf)
            {
                equipItem_ChainArmor[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ƼŸ�� �䰩
        for (int i = 0; i < equipItem_TitaniumBreastplate.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_TitaniumBreastplate[i].activeSelf)
            {
                equipItem_TitaniumBreastplate[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ��Ʈ��
        for (int i = 0; i < equipItem_JetPack.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_JetPack[i].activeSelf)
            {
                equipItem_JetPack[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ��ö �Ź�
        for (int i = 0; i < equipItem_MetalShoes.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (equipItem_MetalShoes[i].activeSelf)
            {
                equipItem_MetalShoes[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ȭ�� �߻�ü
        for (int i = 0; i < bullet_Arrow.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (bullet_Arrow[i].activeSelf)
            {
                bullet_Arrow[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ���� �Ӹ� ������Ʈ
        for (int i = 0; i < object_SkelHead.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (object_SkelHead[i].activeSelf)
            {
                object_SkelHead[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �ڽ� ���� ������Ʈ
        for (int i = 0; i < object_BigBoxPiece.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (object_BigBoxPiece[i].activeSelf)
            {
                object_BigBoxPiece[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ū �ڽ� ���� ������Ʈ
        for (int i = 0; i < object_BigBoxPiece.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (object_BigBoxPiece[i].activeSelf)
            {
                object_BigBoxPiece[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }
    }
}

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
    public GameObject item_FairyS_Prefab;
    public GameObject item_FairyM_Prefab;
    public GameObject item_FairyL_Prefab;
    public GameObject item_FairyXL_Prefab;

    public GameObject hudText_GetGoldText_Prefab;
    public GameObject hudText_GetExpText_Prefab;
    public GameObject hudText_DamageText_Prefab;
    public GameObject hudText_CriticalDamText_Prefab;
    public GameObject hudText_GetHpText_Prefab;

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
    public GameObject bullet_FireBall_Prefab;
    public GameObject bullet_MusicNote_Prefab;
    public GameObject bullet_ShadowBall_Prefab;
    public GameObject bullet_BossSword_Prefab;

    public GameObject effect_ArrowHit_Prefab;
    public GameObject effect_BossSwordHit_Prefab;

    public GameObject object_SkelHead_Prefab;
    public GameObject object_BoxPiece_Prefab;
    public GameObject object_BigBoxPiece_Prefab;

    // �迭 ����
    GameObject[] enemy_Skull; // ����

    GameObject[] item_Coin; // ����
    GameObject[] item_Bullion; // �ݱ�
    GameObject[] item_Exp; // ����ġ
    GameObject[] item_FairyS; // �� S
    GameObject[] item_FairyM; // �� M
    GameObject[] item_FairyL; // �� L
    GameObject[] item_FairyXL; // �� XL

    GameObject[] hudText_GetGoldText; // ��� ȹ�� ����ؽ�Ʈ
    GameObject[] hudText_GetExpText; // ����ġ ȹ�� ����ؽ�Ʈ
    GameObject[] hudText_DamageText; // ����� ����ؽ�Ʈ
    GameObject[] hudText_CriticalDamText; // ũ��Ƽ�� ����� ����ؽ�Ʈ
    GameObject[] hudText_GetHpText; // HP ȹ�� ����ؽ�Ʈ

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
    GameObject[] bullet_FireBall; // ���̾ �߻�ü
    GameObject[] bullet_MusicNote; // ��ǥ �߻�ü
    GameObject[] bullet_ShadowBall; // �����캼 �߻�ü
    GameObject[] bullet_BossSword; // ���� �� �߻�ü

    GameObject[] effect_ArrowHit; // ȭ�� �浹 ����Ʈ
    GameObject[] effect_BossSwordHit; // ���� �� �浹 ����Ʈ

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
        item_Bullion = new GameObject[150];
        item_Exp = new GameObject[300];
        item_FairyS = new GameObject[50];
        item_FairyM = new GameObject[10];
        item_FairyL = new GameObject[10];
        item_FairyXL = new GameObject[10];

        hudText_GetGoldText = new GameObject[150];
        hudText_GetExpText = new GameObject[150];
        hudText_DamageText = new GameObject[150];
        hudText_CriticalDamText = new GameObject[150];
        hudText_GetHpText = new GameObject[50];

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
        bullet_FireBall = new GameObject[150];
        bullet_MusicNote = new GameObject[100];
        bullet_ShadowBall = new GameObject[300];
        bullet_BossSword = new GameObject[30];

        effect_ArrowHit = new GameObject[50];
        effect_BossSwordHit = new GameObject[30];

        object_SkelHead = new GameObject[50];
        object_BoxPiece = new GameObject[50];
        object_BigBoxPiece = new GameObject[50];

        Generate(); // �ν��Ͻ� ����
    }

    // �ν��Ͻ� ���� �Լ�
    void Generate()
    {
        // �迭�� ���̸�ŭ ������Ʈ�� �̸� ����

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

        // �� S
        for (int i = 0; i < item_FairyS.Length; i++)
        {
            item_FairyS[i] = Instantiate(item_FairyS_Prefab); // �ν��Ͻ� ����
            item_FairyS[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(item_FairyS[i]); // �� ���� �ÿ��� �������� ����
        }

        // �� M
        for (int i = 0; i < item_FairyM.Length; i++)
        {
            item_FairyM[i] = Instantiate(item_FairyM_Prefab); // �ν��Ͻ� ����
            item_FairyM[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(item_FairyM[i]); // �� ���� �ÿ��� �������� ����
        }

        // �� L
        for (int i = 0; i < item_FairyL.Length; i++)
        {
            item_FairyL[i] = Instantiate(item_FairyL_Prefab); // �ν��Ͻ� ����
            item_FairyL[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(item_FairyL[i]); // �� ���� �ÿ��� �������� ����
        }

        // �� XL
        for (int i = 0; i < item_FairyXL.Length; i++)
        {
            item_FairyXL[i] = Instantiate(item_FairyXL_Prefab); // �ν��Ͻ� ����
            item_FairyXL[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(item_FairyXL[i]); // �� ���� �ÿ��� �������� ����
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

        // HP ȹ�� ����ؽ�Ʈ
        for (int i = 0; i < hudText_GetHpText.Length; i++)
        {
            hudText_GetHpText[i] = Instantiate(hudText_GetHpText_Prefab); // �ν��Ͻ� ����
            hudText_GetHpText[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(hudText_GetHpText[i]); // �� ���� �ÿ��� �������� ����
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

        // ���̾ �߻�ü
        for (int i = 0; i < bullet_FireBall.Length; i++)
        {
            bullet_FireBall[i] = Instantiate(bullet_FireBall_Prefab); // �ν��Ͻ� ����
            bullet_FireBall[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(bullet_FireBall[i]); // �� ���� �ÿ��� �������� ����
        }

        // ��ǥ �߻�ü
        for (int i = 0; i < bullet_MusicNote.Length; i++)
        {
            bullet_MusicNote[i] = Instantiate(bullet_MusicNote_Prefab); // �ν��Ͻ� ����
            bullet_MusicNote[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(bullet_MusicNote[i]); // �� ���� �ÿ��� �������� ����
        }

        // �����캼 �߻�ü
        for (int i = 0; i < bullet_ShadowBall.Length; i++)
        {
            bullet_ShadowBall[i] = Instantiate(bullet_ShadowBall_Prefab); // �ν��Ͻ� ����
            bullet_ShadowBall[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(bullet_ShadowBall[i]); // �� ���� �ÿ��� �������� ����
        }

        // ���� �� �߻�ü
        for (int i = 0; i < bullet_BossSword.Length; i++)
        {
            bullet_BossSword[i] = Instantiate(bullet_BossSword_Prefab); // �ν��Ͻ� ����
            bullet_BossSword[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(bullet_BossSword[i]); // �� ���� �ÿ��� �������� ����
        }

        // ȭ�� �浹 ����Ʈ
        for (int i = 0; i < effect_ArrowHit.Length; i++)
        {
            effect_ArrowHit[i] = Instantiate(effect_ArrowHit_Prefab); // �ν��Ͻ� ����
            effect_ArrowHit[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(effect_ArrowHit[i]); // �� ���� �ÿ��� �������� ����
        }

        // ���� �� �浹 ����Ʈ
        for (int i = 0; i < effect_BossSwordHit.Length; i++)
        {
            effect_BossSwordHit[i] = Instantiate(effect_BossSwordHit_Prefab); // �ν��Ͻ� ����
            effect_BossSwordHit[i].SetActive(false); // ���� �� �ٷ� ��Ȱ��ȭ
            DontDestroyOnLoad(effect_BossSwordHit[i]); // �� ���� �ÿ��� �������� ����
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
            case "Item_FairyS":
                targetPool = item_FairyS;
                break;
            case "Item_FairyM":
                targetPool = item_FairyM;
                break;
            case "Item_FairyL":
                targetPool = item_FairyL;
                break;
            case "Item_FairyXL":
                targetPool = item_FairyXL;
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
            case "HudText_GetHp":
                targetPool = hudText_GetHpText;
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
            case "Bullet_FireBall":
                targetPool = bullet_FireBall;
                break;
            case "Bullet_MusicNote":
                targetPool = bullet_MusicNote;
                break;
            case "Bullet_ShadowBall":
                targetPool = bullet_ShadowBall;
                break;
            case "Bullet_BossSword":
                targetPool = bullet_BossSword;
                break;
            case "Effect_ArrowHit":
                targetPool = effect_ArrowHit;
                break;
            case "Effect_BossSwordHit":
                targetPool = effect_BossSwordHit;
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

        // �� S
        for (int i = 0; i < item_FairyS.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (item_FairyS[i].activeSelf)
            {
                item_FairyS[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �� M
        for (int i = 0; i < item_FairyM.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (item_FairyM[i].activeSelf)
            {
                item_FairyM[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �� L
        for (int i = 0; i < item_FairyL.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (item_FairyL[i].activeSelf)
            {
                item_FairyL[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �� XL
        for (int i = 0; i < item_FairyXL.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (item_FairyXL[i].activeSelf)
            {
                item_FairyXL[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
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

        // ���̾ �߻�ü
        for (int i = 0; i < bullet_FireBall.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (bullet_FireBall[i].activeSelf)
            {
                bullet_FireBall[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ��ǥ �߻�ü
        for (int i = 0; i < bullet_MusicNote.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (bullet_MusicNote[i].activeSelf)
            {
                bullet_MusicNote[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // �����캼 �߻�ü
        for (int i = 0; i < bullet_ShadowBall.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (bullet_ShadowBall[i].activeSelf)
            {
                bullet_ShadowBall[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
            }
        }

        // ���� �� �߻�ü
        for (int i = 0; i < bullet_BossSword.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (bullet_BossSword[i].activeSelf)
            {
                bullet_BossSword[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
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
        for (int i = 0; i < object_BoxPiece.Length; i++)
        {
            // Ȱ��ȭ �� ������Ʈ�� ã�Ҵٸ�
            if (object_BoxPiece[i].activeSelf)
            {
                object_BoxPiece[i].SetActive(false); // ������Ʈ ��Ȱ��ȭ
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

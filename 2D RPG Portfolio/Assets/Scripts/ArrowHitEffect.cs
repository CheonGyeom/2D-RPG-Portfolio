using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitEffect : MonoBehaviour
{
    public SpriteRenderer sr;
    private void OnEnable()
    {
        Invoke("DestroyEffect", 1f); // 1�� �� ����Ʈ ������Ʈ Ǯ�� ��ȯ
    }

    private void Update()
    {
        sr.color = Color.white;
    }

    void DestroyEffect()
    {
        gameObject.SetActive(false);
    }
}

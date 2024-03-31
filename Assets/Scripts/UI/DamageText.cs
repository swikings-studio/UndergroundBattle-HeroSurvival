using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamageText : MonoBehaviour
{
    private const float actionTime = 0.25f;
    public void Initialize(int damage)
    {
        Transform transform = this.transform;
        TMP_Text text = GetComponent<TMP_Text>();

        text.text = damage.ToString();
        transform.eulerAngles = Camera.main.transform.eulerAngles;

        transform.DOScale(Vector3.one, actionTime);
        transform.DOLocalMoveY(transform.localPosition.y + 1, actionTime * 2);
        text.DOFade(0, actionTime * 2);
        Destroy(gameObject, actionTime * 2);
    }
}
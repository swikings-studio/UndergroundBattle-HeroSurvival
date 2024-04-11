using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeCard : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText, descriptionText;
    [SerializeField] private Image iconImage, weaponIconImage;
    private UpgradeParametrs upgradeParametrs;

    public void SetParametrs(UpgradeParametrs upgradeParametrs)
    {

    }
}
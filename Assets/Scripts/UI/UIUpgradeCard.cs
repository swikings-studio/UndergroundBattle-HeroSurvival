using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIUpgradeCard : MonoBehaviour 
{
    [SerializeField] private TMP_Text titleText, descriptionText;
    [SerializeField] private GameObject weaponObject;
    [SerializeField] private Image upgradeIconImage, weaponIconImage, upgradeTypeIconImage, frontsideImage, frontsideBackgroundImage, backsideImage;
    [SerializeField] private Button choiceButton;
    public void SetParametrs<T>(T upgrade, LevelCardSprites levelCardSprites, UnityAction onSelect) where T : Upgrade
    {
        upgradeIconImage.sprite = upgrade.Icon;
        titleText.text = upgrade.Title;
        descriptionText.text = upgrade.Description;

        frontsideImage.sprite = levelCardSprites.ForegroundSprite;
        frontsideBackgroundImage.sprite = levelCardSprites.BackgroundSprite;
        backsideImage.sprite = levelCardSprites.ReverseSideSprite;
        
        choiceButton.onClick.RemoveAllListeners();
        choiceButton.onClick.AddListener(onSelect);
    }
}
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UILevelUp : MonoBehaviour
{
    [SerializeField] private TMP_Text newLevelNumberText;
    [SerializeField] private Transform cardUpgradesContainer;
    [SerializeField] private GameObject player;
    [SerializeField] private CardSpritesList cardSpritesList;
    [SerializeField] private UpgradesList upgrades;
    private int upgradesCount = 3;
    private UpgradesManager _upgradesManager;

    private void Start()
    {
        _upgradesManager = new UpgradesManager(player);
        ClearUpgrades();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        SetUpgradeCards();
    }

    private void SetUpgradeCards()
    {
        for (int i = 0; i < upgradesCount; i++)
        {
            var card = cardUpgradesContainer.GetChild(i).GetComponent<UIUpgradeCard>();
            var currentUpgrade = upgrades.List[i] as UpgradeSystem;
            int upgradeLevel = _upgradesManager.GetUpgrateLevel(currentUpgrade);
        
            card.SetParametrs(currentUpgrade, cardSpritesList.GetLevelCardSprites(upgradeLevel),() => _upgradesManager.ApplyUpgrade(currentUpgrade));
        }
    }
    
    public void Close()
    {
        ClearUpgrades();
        gameObject.SetActive(false);
    }

    public void SetUpgradesCount(int count)
    {
        int maxCount = cardUpgradesContainer.childCount;

        if (count > maxCount)
        {
            count = maxCount;
            Debug.LogWarning("Maximum upgrades count value exceeded. Set on " + maxCount);
        }

        upgradesCount = count;
    }
    private void ClearUpgrades()
    {
        for (int i = 0; i < cardUpgradesContainer.childCount; i++)
        {
            GameObject upgradeCard = cardUpgradesContainer.GetChild(i).gameObject;
            upgradeCard.SetActive(false);
        }
    }
}
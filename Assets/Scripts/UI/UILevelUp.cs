using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelUp : MonoBehaviour
{
    [SerializeField] private TMP_Text newLevelNumberText;
    [SerializeField] private Transform cardUpgradesContainer;
    [SerializeField] private GameObject player;
    [SerializeField] private CardSpritesList cardSpritesList;
    [SerializeField] private UpgradesList upgrades;
    private int upgradesCount = 3;
    private UpgradesManager _upgradesManager;

    private void Awake()
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
        List<int> randomNumbers = new List<int>();
        randomNumbers.GetRandomNumbersWithoutRepeat(upgradesCount, upgrades.List.Length);
        
        for (int i = 0; i < randomNumbers.Count; i++)
        {
            var card = cardUpgradesContainer.GetChild(i).GetComponent<UIUpgradeCard>();
            var randomUpgrade = upgrades.List[randomNumbers[i]];
            int upgradeLevel = _upgradesManager.GetUpgrateLevel(randomUpgrade);
        
            card.SetParametrs(randomUpgrade, cardSpritesList.GetSpritesByLevel(upgradeLevel), cardSpritesList.GetUpgradeIconSpriteByType(randomUpgrade), () =>
            {
                _upgradesManager.ApplyUpgrade(randomUpgrade);
                Close();
            });
            card.gameObject.SetActive(true);
        }
    }
    
    public void Close()
    {
        ClearUpgrades();
        gameObject.SetActive(false);
        GameTimeManager.SetTimeScaleSmoothly(0.5f, 1);
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
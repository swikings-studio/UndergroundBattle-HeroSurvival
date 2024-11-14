using UnityEngine;

[CreateAssetMenu(fileName = "Card Sprites List", menuName = "SwiKinGs Studio/List/Create Card Sprites List")]
public class CardSpritesList : ScriptableObject
{
    [SerializeField] private LevelCardSprites[] levelCards;
    [SerializeField] private Sprite systemUpgradeIconSprite, abilityIconSprite, weaponIconSprite;

    public Sprite GetUpgradeIconSpriteByType<T>(T upgrade) where T : Upgrade
    {
        return upgrade switch
        {
            UpgradeSystem => systemUpgradeIconSprite,
            Weapon => weaponIconSprite,
            Ability => abilityIconSprite,
            _ => null
        };
    }
    
    public LevelCardSprites GetSpritesByLevel(int level)
    {
        if (level < 0 || level >= levelCards.Length)
            throw new System.Exception("Needed level is not corrected");

        return levelCards[level];
    }
}

[System.Serializable]
public struct LevelCardSprites
{
    public string Name;
    public Sprite ReverseSideSprite, ForegroundSprite, BackgroundSprite;
}
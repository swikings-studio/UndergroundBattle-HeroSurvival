using UnityEngine;

[CreateAssetMenu(fileName = "Card Sprites List", menuName = "SwiKinGs Studio/List/Create Card Sprites List")]
public class CardSpritesList : ScriptableObject
{
    [SerializeField] private LevelCardSprites[] levelCards;
    public LevelCardSprites GetLevelCardSprites(int level)
    {
        if (level < 0 || level >= levelCards.Length)
            throw new System.Exception("Needed level is not corrected");

        return levelCards[level - 1];
    }
}

[System.Serializable]
public struct LevelCardSprites
{
    public string Name;
    public Sprite ReverseSideSprite, ForegroundSprite, BackgroundSprite;
}
using UnityEngine;

public class Upgrade : ScriptableObject
{
    public string Title;
    [Multiline] public string Description;
    public Sprite Icon;
    public float[] LevelValues = new float[7];

    public float Value => LevelValues[UpgradesManager.GetUpgradeLevel(this)];
    public int ValueInt => Mathf.RoundToInt(LevelValues[UpgradesManager.GetUpgradeLevel(this)]);
}
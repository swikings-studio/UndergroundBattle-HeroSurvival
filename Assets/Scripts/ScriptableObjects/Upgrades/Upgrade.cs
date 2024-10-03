using UnityEngine;

public class Upgrade : ScriptableObject
{
    public string Title;
    [Multiline] public string Description;
    public Sprite Icon;
    public float[] LevelValues = new float[7];
}
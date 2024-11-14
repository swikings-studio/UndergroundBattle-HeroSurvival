using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "SwiKinGs Studio/Create Ability", order = 0)]
public class Ability : AttackUpgrade
{
    public float[] applySecondsByLevel = new float[7];
    public NeededParametrsForOpen neededUpgradeAndLevelForOpen;
}
[Serializable]
public class NeededParametrsForOpen
{
    [SerializeField] private UpgradeAndLevel[] Items;

    public Dictionary<Upgrade, int> ToDictionary()
    {
        var result = new Dictionary<Upgrade, int>();
        foreach (var item in Items)
        {
            result.Add(item.Upgrade, item.Level);
        }

        return result;
    }
}

[Serializable]
public struct UpgradeAndLevel
{
    public Upgrade Upgrade;
    public int Level;
}
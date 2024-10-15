using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Ability", menuName = "SwiKinGs Studio/Create Ability", order = 0)]
public class Ability : Upgrade
{
    public AssetReferenceGameObject Reference;
    public float[] ApplySecondsByLevel = new float[7];
    public float[] CooldownSecondsByLevel = new float[7];
    public NeededParametrsForOpen NeededUpgradeAndLevelForOpen;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrades List", menuName = "SwiKinGs Studio/Create Upgrades List")]
public class UpgradesList : ScriptableObject
{
    public Upgrade[] Upgrades;
    public Upgrade GetUpgrade(PlayerSystem system)
    {
        foreach (Upgrade upgrade in Upgrades)
        {
            if (upgrade.System == system) return upgrade;
        }
        return Upgrades[0];
    }
}
[System.Serializable]
public struct Upgrade
{
    public PlayerSystem System;
    public UpgradeParametrs[] Parametrs;
}
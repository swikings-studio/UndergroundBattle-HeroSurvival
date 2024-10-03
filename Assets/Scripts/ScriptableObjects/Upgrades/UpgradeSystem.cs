using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Upgrade System", menuName = "SwiKinGs Studio/Create Upgrade System", order = 0)]
public class UpgradeSystem : Upgrade
{
    public PlayerSystem System;
}
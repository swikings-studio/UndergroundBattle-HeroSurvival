using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Weapon", menuName = "SwiKinGs Studio/Create Weapon", order = 0)]
public class Weapon : Upgrade
{
    public AssetReferenceGameObject Reference;
    public float[] cooldownSecondsByLevel;
}
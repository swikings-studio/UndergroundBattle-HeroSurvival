using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Ability", menuName = "SwiKinGs Studio/Create Ability", order = 0)]
public class Ability : Upgrade
{
    public AssetReferenceGameObject Reference;
    public float[] ApplySecondsByLevel = new float[7];
    public float[] CooldownSecondsByLevel = new float[7];
}

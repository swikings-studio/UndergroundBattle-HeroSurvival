using UnityEngine.AddressableAssets;

public class AttackUpgrade : Upgrade
{
    public AssetReference reference;
    public float[] cooldownSecondsByLevel = new float[7];
    public float[] radiusByLevel = new float[7];
    public float Reload => cooldownSecondsByLevel[UpgradesManager.GetUpgradeLevel(this)];
    public float Radius => radiusByLevel[UpgradesManager.GetUpgradeLevel(this)];
    
}
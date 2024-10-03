using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Upgrades List", menuName = "SwiKinGs Studio/List/Create Upgrades List")]
public class UpgradesList : ScriptableObject
{
    public Upgrade[] List;
}
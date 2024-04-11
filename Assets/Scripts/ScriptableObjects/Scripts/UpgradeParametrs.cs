using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Upgrade", menuName = "SwiKinGs Studio/Create Upgrade")]
public class UpgradeParametrs : ScriptableObject
{
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField, Multiline] public string Description { get; private set; }
    [HideInInspector] public Sprite Icon;
    [field: SerializeField] public PlayerSystem System { get; private set; }
    [field: SerializeField] public Operation OperationEnum { get; private set; }
    [field: SerializeField] public Type TypeEnum { get; private set; }
    [field: SerializeField, Range(0, 100)] public float OperationCount { get; private set; }
    [field: SerializeField] public AssetReferenceGameObject InstantiatePrefab { get; private set; }

    public enum Operation
    {
        Increase,
        Deacrease,
        New
    }
    public enum Type
    {
        Damage,
        Reload,
        Radius,
        Instantiate,
        Opportunity,
        Effect
    }
}

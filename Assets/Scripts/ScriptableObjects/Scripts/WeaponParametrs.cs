using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Weapon", menuName = "SwiKinGs Studio/Create Weapon")]
public class WeaponParametrs : ScriptableObject
{
    [field: SerializeField] public AssetReferenceGameObject PrefabReference;
    [HideInInspector] public Sprite Icon;
    [field: SerializeField, Multiline] public string Description { get; private set; }
    [field: SerializeField, Range(0, 100)] public int Damage { get; private set; }
    [field: SerializeField, Range(0, 30)] public float ReloadDuration { get; private set; }
    [field: SerializeField, Range(0, 30)] public float Radius { get; private set; }
    [field: SerializeField] public AttackType Type { get; private set; }
    [field: SerializeField] public AssetReferenceGameObject ProjectilePrefabReference { get; private set; }
    [field: SerializeField] public UpgradeParametrs[] Upgrades { get; private set; }
}
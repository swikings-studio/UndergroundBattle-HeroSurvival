using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Objects List", menuName = "SwiKinGs Studio/Create Objects List")]
public class ObjectsList : ScriptableObject
{
    public Parametrs[] Objects;
    [System.Serializable]
    public struct Parametrs
    {
        public AssetReferenceGameObject Reference;
        public Vector3 Offset;
        public int DropChance;
    }

    public Parametrs GetRandomObjectParametrs()
    {
        int summaryChance = 0;

        foreach (Parametrs objectParametrs in Objects)
            summaryChance += objectParametrs.DropChance;

        int randomValue = Random.Range(0, summaryChance);
        int previousValue = 0;
        foreach (Parametrs objectParametrs in Objects)
        {
            previousValue += objectParametrs.DropChance;

            if (randomValue < previousValue) return objectParametrs;
        }
        return Objects[Objects.Length - 1];
    }
}
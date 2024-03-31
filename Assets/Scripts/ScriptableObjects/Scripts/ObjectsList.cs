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
    }
}
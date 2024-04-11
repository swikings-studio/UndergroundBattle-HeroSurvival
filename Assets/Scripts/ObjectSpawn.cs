using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AddressableAssets;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject reference;
    [SerializeField] private Vector3 offset = Vector3.zero;
    public void Start()
    {
        reference.LoadAssetAsync();
    }
    public void Spawn()
    {
        reference.InstantiateAsync(transform.position + offset, Quaternion.identity);
        //Addressables.InstantiateAsync(reference, transform.position + offset, Quaternion.identity);
    }
    public void Spawn(string key)
    {
        Addressables.InstantiateAsync(key, transform.position + offset, Quaternion.identity);
    }
}
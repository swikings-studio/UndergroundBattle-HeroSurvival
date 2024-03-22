using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private Vector3 offset = Vector3.zero;

    public void Spawn()
    {
        Instantiate(objectPrefab, transform.position + offset, Quaternion.identity);
    }
    public void Spawn(Vector3 position)
    {
        Instantiate(objectPrefab, position + offset, Quaternion.identity);
    }
    public void Spawn(Transform transformPosition)
    {
        Instantiate(objectPrefab, transformPosition.position + offset, Quaternion.identity);
    }
}
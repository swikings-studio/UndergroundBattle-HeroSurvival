using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;

public class DestroyableObject : MonoBehaviour, IDamagable
{
    [SerializeField] private int healths;
    [SerializeField] private float spawnChance;
    [SerializeField] private ObjectsList spawnObjectsList;
    public UnityEvent OnGetHit, OnDestroy;

    private ObjectsList.Parametrs innerObjectParametrs;
    private bool isSpawnObject;

    private void Start()
    {
        OnDestroy.AddListener(Destroy);
        isSpawnObject = Random.Range(0, 101) < spawnChance;
        if (isSpawnObject)
        {
            innerObjectParametrs = spawnObjectsList.GetRandomObjectParametrs();
        }
    }
    public void GetHit(int damage)
    {
        if (healths - damage < 0) damage = healths;

        healths -= damage;
        DamageCountText.Create(this, damage);

        if (healths <= 0)
        {
            OnDestroy.Invoke();
        }
        else
        {
            OnGetHit.Invoke();
        }
    }

    public void Destroy()
    {
        if (TryGetComponent(out MeshRenderer meshRenderer)) meshRenderer.enabled = false;
        if (TryGetComponent(out Collider collider)) collider.enabled = false;

        if (isSpawnObject)
        {
            Addressables.InstantiateAsync(innerObjectParametrs.Reference, transform.position + innerObjectParametrs.Offset, Quaternion.identity);
        }
        Destroy(gameObject, 1f);
    }
}
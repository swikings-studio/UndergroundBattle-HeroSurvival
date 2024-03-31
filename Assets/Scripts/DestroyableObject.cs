using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;

public class DestroyableObject : MonoBehaviour, IDamagable, IDestroyable
{
    [SerializeField] private int healths;
    [SerializeField] private float spawnChance;
    [SerializeField] private ObjectsList spawnObjectsList;
    public UnityEvent OnGetHit, OnDestroy;

    private void Start()
    {
        OnDestroy.AddListener(Destroy);
    }

    public void GetHit(int damage)
    {
        if (healths - damage < 0) damage = healths;

        healths -= damage;

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

        if (Random.Range(0, 101) < spawnChance)
        {
            ObjectsList.Parametrs randomObjectParametrs = spawnObjectsList.Objects[Random.Range(0, spawnObjectsList.Objects.Length)];
            Addressables.InstantiateAsync(randomObjectParametrs.Reference, transform.position + randomObjectParametrs.Offset, Quaternion.identity);
        }
        Destroy(gameObject, 1f);
    }
}
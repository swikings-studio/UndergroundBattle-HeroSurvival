using UnityEngine;
using UnityEngine.AddressableAssets;

public class DestroyableObject : BaseHealthSystem
{
    [SerializeField] private float spawnChance;
    [SerializeField] private ObjectsList spawnObjectsList;

    private ObjectsList.Parametrs innerObjectParametrs;
    private bool isSpawnObject;

    protected override void Start()
    {
        base.Start();
        OnHealthsOver.AddListener(Destroy);
        isSpawnObject = Random.Range(0, 101) < spawnChance;
        if (isSpawnObject)
        {
            innerObjectParametrs = spawnObjectsList.GetRandomObjectParametrs();
            //innerObjectParametrs.Reference.LoadAssetAsync();
        }
    }
    public override void GetHit(int damage)
    {
        base.GetHit(damage);

        if (healths <= 0)
        {
            OnHealthsOver.Invoke();
        }
        else
        {
            OnGetHit.Invoke();
        }
    }

    private void Destroy()
    {
        if (TryGetComponent(out MeshRenderer meshRenderer)) meshRenderer.enabled = false;

        if (isSpawnObject)
        {
            Addressables.InstantiateAsync(innerObjectParametrs.Reference, transform.position + innerObjectParametrs.Offset, Quaternion.identity);
        }
        Destroy(gameObject, 1f);
    }
}
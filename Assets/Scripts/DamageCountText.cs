using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class DamageCountText
{
    private const string damageCountTextKey = "DamageCountText";
    private const float actionTime = 0.25f;

    public static void Create(MonoBehaviour monoBehaviour, int damage)
    {
        Vector3 neededPosition = monoBehaviour.transform.position + (Vector3.up * 0.1f);
        monoBehaviour.StartCoroutine(Initializing(neededPosition, damage));
    }

    private static IEnumerator Initializing(Vector3 position, int damage)
    {
        AsyncOperationHandle<GameObject> opHandle = Addressables.LoadAssetAsync<GameObject>(damageCountTextKey);
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var instantiate = Addressables.InstantiateAsync(damageCountTextKey, position, Quaternion.identity);

            if (instantiate.IsDone)
            {
                if (instantiate.Result.TryGetComponent(out TMP_Text text))
                {
                    Transform transform = text.transform;

                    text.text = damage.ToString();
                    transform.eulerAngles = Camera.main.transform.eulerAngles;
                    transform.localScale = Vector3.zero;

                    transform.DOScale(Vector3.one, actionTime);
                    transform.DOLocalMoveY(transform.localPosition.y + 1, actionTime * 2);
                    yield return text.DOFade(0, actionTime * 2).WaitForCompletion();

                    Addressables.Release(opHandle);
                    Object.Destroy(text.gameObject, actionTime * 2);
                }
            }
        }
        yield break;
    }
}
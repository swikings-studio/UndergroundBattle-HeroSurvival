using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class DamageCountText
{
    private const string damageCountTextKey = "DamageCountText";
    private const float actionTime = 0.25f;

    public static void Create(MonoBehaviour monoBehaviour, int damage, Type type = Type.Default)
    {
        Vector3 neededPosition = monoBehaviour.transform.position + (Vector3.up * 0.1f);
        monoBehaviour.StartCoroutine(Initializing(neededPosition, damage, type));
    }

    private static IEnumerator Initializing(Vector3 position, int damage, Type type)
    {
        var instantiate = Addressables.InstantiateAsync(damageCountTextKey, position, Quaternion.identity);
        yield return instantiate;

        if (instantiate.IsDone)
        {
            if (instantiate.Result.TryGetComponent(out TMP_Text text))
            {
                Transform transform = text.transform;

                text.text = damage.ToString();
                if (type != Type.Default) text.color = GetColorByType(type);

                transform.eulerAngles = Camera.main.transform.eulerAngles;
                transform.localScale = Vector3.zero;

                transform.DOScale(Vector3.one, actionTime);
                transform.DOLocalMoveY(transform.localPosition.y + 1, actionTime * 2);
                yield return text.DOFade(0, actionTime * 2).WaitForCompletion();
            }
            Addressables.ReleaseInstance(instantiate.Result);
        }
    }

    private static Color GetColorByType(Type type)
    {
        switch (type)
        {
            case Type.Default: return Color.white;
            case Type.Critical: return Color.red;
            default: return Color.white;
        }
    }

    public enum Type
    {
        Default,
        Critical
    }
}
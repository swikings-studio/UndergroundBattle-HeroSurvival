using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using SwiKinGsStudio;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CollectableItem : MonoBehaviour
{
    [Range(1, 10), SerializeField] private int points = 1;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private AnimationType idleAnimationType;
    [SerializeField] private Light _light;

    private MoveManager moveManager;
    private bool isTriggered = false;
    public bool IsTriggered => isTriggered;
    private const float actionTime = 0.3f, soaringRange = 0.1f;
    private Tween idleAnimation;

    private void Awake()
    {
        moveManager = new MoveManager(GetComponent<Rigidbody>(), moveSpeed);
    }
    private void Start()
    {
        StartCoroutine(Appearance());
    }
    private IEnumerator Appearance()
    {
        Collider collider = GetComponent<Collider>();
        Vector3 objectPrefabScale = transform.localScale;

        collider.enabled = false;
        float startPositionY = transform.position.y;

        transform.localScale = Vector3.zero;
        transform.DOMoveY(startPositionY, actionTime).From(0);
        transform.DOScale(objectPrefabScale, actionTime);

        yield return new WaitForSeconds(actionTime);

        collider.enabled = true;
        yield return new WaitForEndOfFrame();

        if (isTriggered) yield break;

        idleAnimation = GetIdleAnimation();
    }
    private Tween GetIdleAnimation()
    {
        return idleAnimationType switch
        {
            AnimationType.Soaring => AnimationsManager.Soaring(transform, soaringRange, actionTime * 4),
            AnimationType.Rotating => AnimationsManager.Rotating(transform, new Vector3(0, 180, 0), actionTime * 4),
            _ => null,
        };
    }
    public void MoveTo(Transform target, UnityAction<int> callbackOnComplete)
    {
        if (isTriggered) return;

        idleAnimation?.Kill();
        GetComponent<Collider>().enabled = false;
        StartCoroutine(Moving(target, callbackOnComplete));
    }
    private IEnumerator Moving(Transform target, UnityAction<int> callbackOnComplete)
    {
        isTriggered = true;
        Vector3 direction = target.position - transform.position;

        while (moveManager.IsMoving(direction))
        {
            moveManager.MoveKinematic(direction);
            moveManager.UpdateMoveRange(0.04f + transform.position.y);
            direction = target.position - transform.position;
            yield return new WaitForFixedUpdate();
        }

        callbackOnComplete.Invoke(points);
        if (_light) _light.DOIntensity(0, actionTime);
        yield return transform.DOScale(0, actionTime).WaitForCompletion();

        Addressables.ReleaseInstance(gameObject);
        yield break;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CollectableItem : MonoBehaviour
{
    [Range(1, 10), SerializeField] private int points = 1;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private Light _light;
    private MoveManager moveManager;
    private bool isTriggered;
    public bool IsTriggered => isTriggered;
    private Sequence soaring;
    private const float actionTime = 0.3f, soaringRange = 0.1f;
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
        soaring = DOTween.Sequence();
        soaring.Append(transform.DOLocalMoveY(transform.localPosition.y + soaringRange, actionTime * 5));
        soaring.Append(transform.DOLocalMoveY(transform.localPosition.y - soaringRange, actionTime * 6));
        soaring.Append(transform.DOLocalMoveY(transform.localPosition.y, actionTime * 4).SetEase(Ease.Linear));
        soaring.SetLoops(-1, LoopType.Restart);

        yield break;
    }
    public void MoveTo(Transform target, UnityAction<int> callbackOnComplete)
    {
        if (isTriggered) return;

        soaring.Kill();
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
        transform.DOScale(0, actionTime).WaitForCompletion();
        if (_light != null) _light.DOIntensity(0, actionTime).WaitForCompletion();
        yield return new WaitForSeconds(actionTime);

        Addressables.ReleaseInstance(gameObject);
        yield break;
    }
}
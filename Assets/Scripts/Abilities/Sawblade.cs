using System.Collections;
using UnityEngine;

public class Sawblade : BaseAbility
{
    [SerializeField] private Transform target;

    private void Start()
    {
        Apply(target);
    }

    protected override IEnumerator Applying(Transform target)
    {
        float applyTyme = ApplyTime;
        float degreesPerSecond = 360 / applyTyme;
        Transform body = transform.GetChild(0);

        while (true)
        {
            applyTyme -= Time.deltaTime;
            transform.position = target.position;
            body.RotateAround(transform.position, Vector3.up, degreesPerSecond * Time.deltaTime);
            body.Rotate(Vector3.back, degreesPerSecond * Time.deltaTime * 5f, Space.Self);
            yield return null;
        }

        OnComplete?.Invoke();
    }
}
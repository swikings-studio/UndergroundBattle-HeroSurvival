using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{
    [SerializeField] private LayerMask excludeLayersOnThrow;
    [SerializeField] private UnityEvent onDestroy;

    private State state = State.Wait;
    private MoveManager moveManager;
    private Vector3 endPosition;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private float throwPower;

    private bool isThrowed => state == State.Throwed;
    public bool CanCollected => state == State.Wait;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Collected()
    {
        _collider.enabled = false;
        state = State.Collected;
    }
    public void Throwed(Vector3 endPosition, float throwPower)
    {
        moveManager = new MoveManager(_rigidbody, throwPower, 0.1f);

        this.throwPower = throwPower;
        _collider.excludeLayers = excludeLayersOnThrow;
        _collider.enabled = true;
        this.endPosition = endPosition;
        state = State.Throwed;
    }
    private void FixedUpdate()
    {
        if (isThrowed)
        {
            moveManager.MoveKinematic(endPosition - transform.position);
            moveManager.Rotate(_rigidbody.rotation * Quaternion.Euler(360 * Time.fixedDeltaTime * Vector3.right));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isThrowed)
        {
            if (other.TryGetComponent(out IDamagable damagable))
            {
                int damage = (int)throwPower;
                DamageCountText.Create(other.GetComponent<MonoBehaviour>(), damage);
                damagable.GetHit(damage);
            }
            StartCoroutine(Destroying());
        }
    }
    private IEnumerator Destroying()
    {
        state = State.Destroying;
        GetComponent<MeshRenderer>().enabled = false;
        _collider.enabled = false;
        onDestroy.Invoke();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
        yield break;
    }
    private enum State
    {
        Wait,
        Collected,
        Throwed,
        Destroying
    }
}
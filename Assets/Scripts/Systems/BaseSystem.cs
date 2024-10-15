using UnityEngine;

public abstract class BaseSystem : MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected Animator _animator;

    public virtual void Awake()
    {
        if (_rigidbody == null && TryGetComponent(out Rigidbody rigidbody))
        {
            Debug.Log("Get Rigidbody");
            _rigidbody = rigidbody;
        }

        if (_animator == null && TryGetComponent(out Animator animator))
        {
            Debug.Log("Get Animator");
            _animator = animator;
        }
    }

    public abstract void Upgrade(float value);
}
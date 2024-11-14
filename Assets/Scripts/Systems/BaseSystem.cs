using UnityEngine;

public abstract class BaseSystem : MonoBehaviour
{
    protected const string AnimatorHitTriggerName = "Hit";
    protected const string AnimatorHitVariationFloatName = "HitVariation";
    
    protected Rigidbody _rigidbody;
    protected Animator _animator;

    public virtual void Awake()
    {
        if (_rigidbody == null && TryGetComponent(out Rigidbody rigidbody))
        {
            _rigidbody = rigidbody;
        }

        if (_animator == null && TryGetComponent(out Animator animator))
        {
            _animator = animator;
        }
    }
}
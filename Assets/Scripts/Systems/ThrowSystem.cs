using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSystem : Unit, ILockayable
{
    [Range(0, 2), SerializeField] private float pickUpRadius;
    [Range(0, 10), SerializeField] private float throwRange;
    [Range(0, 10), SerializeField] private int throwPower;
    [SerializeField] private LayerMask neededLayerMask;
    [SerializeField] private Transform itemsContainer;
    private const string pickUpAnimatorParametrName = "PickUp", throwAnimatorParametrsName = "Throw";
    private PickableObject collectedItem;

    public bool IsLocked { get; set; }

    private void Update()
    {
        if (Input.GetButtonDown("Use"))
        {
            CollectAndThrow();
        }
    }
    public void CollectAndThrow()
    {
        if (IsLocked) return;

        if (collectedItem == null)
        {
            if (TryPickUp(out PickableObject item))
            {
                Collect(item);
            }
        }
        else
        {
            _animator.SetTrigger(throwAnimatorParametrsName);
        }
    }
    private void Throw()
    {
        if (collectedItem == null) return;

        Vector3 direction = transform.position + transform.forward * throwRange;

        collectedItem.transform.localPosition = new Vector3(0, 0.25f, 0);//0.2f);
        collectedItem.transform.SetParent(null);
        Debug.Log(direction);
        collectedItem.Throwed(direction, throwPower);
        collectedItem = null;
    }
    private void Collect(PickableObject item)
    {
        if (collectedItem != null) return;

        _animator.SetTrigger(pickUpAnimatorParametrName);
        item.Collected();
        collectedItem = item;
        collectedItem.transform.SetParent(itemsContainer);
        collectedItem.transform.localPosition = Vector3.zero;
        collectedItem.transform.eulerAngles = new Vector3(-90, 0, 0);
    }
    private bool TryPickUp(out PickableObject item)
    {
        item = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickUpRadius, neededLayerMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out PickableObject foundedItem))
            {
                if (foundedItem.CanCollected == false) continue;

                item = foundedItem;
                return true;
            }    
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, Vector3.forward * throwRange);
    }
}
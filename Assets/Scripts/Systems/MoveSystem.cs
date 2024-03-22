using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSystem : Unit, ILockayable
{
    [SerializeField, Range(0f, 20f)] private float _movementSpeed;
    private MoveManager moveManager;

    public bool IsLocked { get; set; }
    private Vector3 moveDirection;

    private void Start()
    {
        moveManager = new MoveManager(_rigidbody, _movementSpeed);
    }

    private void Update()
    {
        if (IsLocked) return;

        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontalMove, 0, verticalMove);
        _animator.SetBool(moveManager.AnimatorNameParametr, moveManager.IsMoving(moveDirection));
    }   
    private void FixedUpdate()
    {
        if (IsLocked) return;

        moveManager.Move(moveDirection);
    }
    public void Lock()
    { 
        IsLocked = true;
        _animator.SetBool(moveManager.AnimatorNameParametr, false);
    }
}
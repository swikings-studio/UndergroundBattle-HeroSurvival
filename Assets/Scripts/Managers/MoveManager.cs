using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager
{
    public readonly string AnimatorNameParametr = "isMove";
    private readonly Rigidbody _rigidbody;
    private bool isStoped;
    private float moveRange = 0;
    private readonly float speed;

    public bool IsMoving(Vector3 direction)
    {
        if (isStoped) return false;

        float distance = direction.magnitude;

        if (moveRange != 0) return distance > moveRange;
        else return distance > 0;
    }

    public void UpdateMoveRange(float newMoveRange) => moveRange = newMoveRange;

    public MoveManager(Rigidbody rigidbody, float speed)
    {
        _rigidbody = rigidbody;
        this.speed = speed;
    }
    public MoveManager(Rigidbody rigidbody, float speed, float moveRange)
    {
        this.moveRange = moveRange;
        this.speed = speed;
        _rigidbody = rigidbody;
    }

    public void Move(Vector3 direction)
    {
        if (IsMoving(direction) == false) return;

        _rigidbody.MovePosition(_rigidbody.position + speed * Time.fixedDeltaTime * direction.normalized);
    }
    public void Rotate(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        _rigidbody.MoveRotation(Quaternion.LookRotation(direction.normalized));
    }
    public void Rotate(Quaternion quaternion)
    {
        _rigidbody.MoveRotation(quaternion);
    }
    public void MoveAndRotate(Vector3 direction)
    {
        if (IsMoving(direction) == false) return;

        _rigidbody.Move(_rigidbody.position + speed * Time.fixedDeltaTime * direction.normalized, Quaternion.LookRotation(direction));
    }
    public void MoveKinematic(Vector3 direction)
    {
        if (IsMoving(direction) == false) return;

        _rigidbody.MovePosition(_rigidbody.position + speed * Time.deltaTime * direction.normalized);
    }
}
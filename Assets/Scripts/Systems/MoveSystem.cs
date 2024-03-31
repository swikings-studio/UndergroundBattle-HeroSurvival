using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MoveSystem : Unit, ILockayable
{
    [SerializeField, Range(0f, 20f)] private float _movementSpeed;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private GameObject mobileUI;
    private MoveManager moveManager;

    public bool IsLocked { get; set; }
    private Vector3 moveDirection;

    private void Start()
    {
        moveManager = new MoveManager(_rigidbody, _movementSpeed);
        mobileUI.SetActive(YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet);
    }

    private void Update()
    {
        if (IsLocked) return;

        if (YandexGame.EnvironmentData.isDesktop)
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            float verticalMove = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(horizontalMove, 0, verticalMove);
        }
        else
        {
            moveDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        }
        _animator.SetBool(moveManager.AnimatorNameParametr, moveManager.IsMoving(moveDirection));
    }   
    private void FixedUpdate()
    {
        if (IsLocked) return;

        moveManager.MoveAndRotate(moveDirection);
    }
    public void Lock()
    { 
        IsLocked = true;
        _animator.SetBool(moveManager.AnimatorNameParametr, false);
    }
}
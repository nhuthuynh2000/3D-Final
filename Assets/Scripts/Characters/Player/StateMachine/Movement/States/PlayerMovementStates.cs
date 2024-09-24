using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementStates : IState
{
    protected PlayerMovementStateMachine stateMachine;

    protected PlayerGroundedData movementData;

    protected bool shouldWalk;
    public PlayerMovementStates(PlayerMovementStateMachine playerMovementStateMachine)
    {
        stateMachine = playerMovementStateMachine;
        movementData = stateMachine.Player.Data.GroundedData;
        InitializeData();
    }

    private void InitializeData()
    {
        stateMachine.reusableData.TimeToReachTargetRotation = movementData.rotationData.targetRoationReachTime;
    }

    public virtual void Enter()
    {
        AddInputActionCallBack();
    }



    public virtual void Exit()
    {
        RemoveInputActionCallBack();
    }



    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public void PhysicsUpdate()
    {
        Move();
    }


    public virtual void Update()
    {

    }

    #region Main Methods
    private void ReadMovementInput()
    {
        stateMachine.reusableData.movementInput = stateMachine.Player.playerInput.playerActions.Movement.ReadValue<Vector2>();
    }
    private void Move()
    {
        if (stateMachine.reusableData.movementInput == Vector2.zero || stateMachine.reusableData.movementSpeedModifier == 0f)
        {
            return;
        }
        Vector3 movementDirection = GetMovementInputDirection();
        float targetRotationYAngle = Rotate(movementDirection);
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
        float movementSpeed = GetMoveSpeed();
        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.Player.myRigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
    }



    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);
        RotateTowardsTargetRotation();
        return directionAngle;
    }



    private float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.Player.mainCameraTransform.eulerAngles.y;
        if (angle > 360f)
        {
            angle -= 360;
        }

        return angle;
    }

    private static float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        stateMachine.reusableData.CurrentTargetRotation.y = targetAngle;
        stateMachine.reusableData.DampedTargetRotationPassedTime.y = 0f;
    }
    #endregion
    #region Reuasble Methods
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.reusableData.movementInput.x, 0f, stateMachine.reusableData.movementInput.y);
    }
    protected float GetMoveSpeed()
    {
        return movementData.baseSpeed * stateMachine.reusableData.movementSpeedModifier;
    }
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = stateMachine.Player.myRigidbody.velocity;
        playerHorizontalVelocity.y = 0f;
        return playerHorizontalVelocity;
    }
    private void RotateTowardsTargetRotation()
    {
        float currentYAngle = stateMachine.Player.myRigidbody.rotation.eulerAngles.y;
        if (currentYAngle == stateMachine.reusableData.CurrentTargetRotation.y)
        {
            return;
        }
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.reusableData.CurrentTargetRotation.y, ref stateMachine.reusableData.DampedTargetRotationCurrentVelocity.y, stateMachine.reusableData.TimeToReachTargetRotation.y - stateMachine.reusableData.DampedTargetRotationPassedTime.y);
        stateMachine.reusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
        stateMachine.Player.myRigidbody.MoveRotation(targetRotation);
    }
    protected float UpdateTargetRotation(Vector3 direction, bool shouConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);
        if (shouConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }
        directionAngle = AddCameraRotationToAngle(directionAngle);
        if (directionAngle != stateMachine.reusableData.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }
    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }
    protected void ResetVelocity()
    {
        stateMachine.Player.myRigidbody.velocity = Vector3.zero;
    }
    protected virtual void AddInputActionCallBack()
    {
        stateMachine.Player.playerInput.playerActions.WalkToggle.started += OnWalkToggleStarted;
    }



    protected virtual void RemoveInputActionCallBack()
    {
        stateMachine.Player.playerInput.playerActions.WalkToggle.started -= OnWalkToggleStarted;
    }
    #endregion


    #region Input Methods
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        shouldWalk = !shouldWalk;
    }
    #endregion
}

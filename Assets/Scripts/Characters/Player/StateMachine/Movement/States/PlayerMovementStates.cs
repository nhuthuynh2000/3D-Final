using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovementStates : IState
{
    protected PlayerMovementStateMachine stateMachine;
    protected Vector2 movementInput;
    protected float baseSpeed = 5f;
    protected float speedModifier = 1f;

    protected Vector3 currentTargetRotation;
    protected Vector3 timeToReachTargetRotation;
    protected Vector3 dampedTargetRotationCurrentVelocity;
    protected Vector3 dampedTargetRotationPassedTime;
    public PlayerMovementStates(PlayerMovementStateMachine playerMovementStateMachine)
    {
        stateMachine = playerMovementStateMachine;
        InitializeData();
    }

    private void InitializeData()
    {
        timeToReachTargetRotation.y = 0.14f;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void HandleInput()
    {
        ReadMovementInput();
    }

    public void PhysicsUpdate()
    {
        Move();
    }


    public void Update()
    {

    }

    #region Main Methods
    private void ReadMovementInput()
    {
        movementInput = stateMachine.Player.playerInput.playerActions.Movement.ReadValue<Vector2>();
    }
    private void Move()
    {
        if (movementInput == Vector2.zero || speedModifier == 0f)
        {
            return;
        }
        Vector3 movementDirection = GetMovementInputDirection();
        float targetRotationYAngle = Rotate(movementDirection);
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
        float movementSpeed = GetMoveSpeed();
        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.Player.rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
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
        float directionAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        currentTargetRotation.y = targetAngle;
        dampedTargetRotationPassedTime.y = 0f;
    }
    #endregion
    #region Reuasble Methods
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(movementInput.x, 0f, movementInput.y);
    }
    protected float GetMoveSpeed()
    {
        return baseSpeed * speedModifier;
    }
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = stateMachine.Player.rigidbody.velocity;
        playerHorizontalVelocity.y = 0f;
        return playerHorizontalVelocity;
    }
    private void RotateTowardsTargetRotation()
    {
        float currentYAngle = stateMachine.Player.rigidbody.rotation.eulerAngles.y;
        if (currentYAngle == currentTargetRotation.y)
        {
            return;
        }
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, ref dampedTargetRotationCurrentVelocity.y, timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y);
        dampedTargetRotationPassedTime.y += Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);
        stateMachine.Player.rigidbody.MoveRotation(targetRotation);
    }
    protected float UpdateTargetRotation(Vector3 direction, bool shouConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);
        if (shouConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }
        directionAngle = AddCameraRotationToAngle(directionAngle);
        if (directionAngle != currentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }
    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }
    #endregion

}

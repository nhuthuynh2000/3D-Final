using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementStates : IState
{
    protected PlayerMovementStateMachine stateMachine;

    protected PlayerGroundedData movementData;

    protected PlayerAirborneData airborneData;


    public PlayerMovementStates(PlayerMovementStateMachine playerMovementStateMachine)
    {
        stateMachine = playerMovementStateMachine;
        movementData = stateMachine.Player.Data.GroundedData;
        airborneData = stateMachine.Player.Data.AirborneData;
        InitializeData();
    }

    private void InitializeData()
    {
        SetBaseRotationData();

    }


    #region IState Methods
    public virtual void Enter()
    {
        Debug.Log("State: " + GetType().Name);
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

    public virtual void PhysicsUpdate()
    {
        Move();
    }


    public virtual void Update()
    {

    }
    public virtual void OnAnimationEnterEvent()
    {

    }

    public virtual void OnAnimationExitEvent()
    {

    }

    public virtual void OnAnimationTransitionEvent()
    {

    }
    public virtual void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.Player.layerData.isGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGround(collider);
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (stateMachine.Player.layerData.isGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGroundExited(collider);
            return;
        }
    }




    #endregion
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
        return movementData.baseSpeed * stateMachine.reusableData.movementSpeedModifier * stateMachine.reusableData.movementOnSlopeSpeedModifier;
    }
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = stateMachine.Player.myRigidbody.velocity;
        playerHorizontalVelocity.y = 0f;
        return playerHorizontalVelocity;
    }
    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, stateMachine.Player.myRigidbody.velocity.y, 0f);
    }
    protected void RotateTowardsTargetRotation()
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
    protected void ResetVerticalVelocity()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.Player.myRigidbody.velocity = playerHorizontalVelocity;
    }
    protected virtual void AddInputActionCallBack()
    {
        stateMachine.Player.playerInput.playerActions.WalkToggle.started += OnWalkToggleStarted;
    }



    protected virtual void RemoveInputActionCallBack()
    {
        stateMachine.Player.playerInput.playerActions.WalkToggle.started -= OnWalkToggleStarted;
    }
    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.Player.myRigidbody.AddForce(-playerHorizontalVelocity * stateMachine.reusableData.movementDecelerationForce, ForceMode.Acceleration);
    }
    protected void DecelerateVertically()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
        stateMachine.Player.myRigidbody.AddForce(-playerVerticalVelocity * stateMachine.reusableData.movementDecelerationForce, ForceMode.Acceleration);
    }
    protected bool isMovingHorizontally(float minimumMagnitude = 0.1f)
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);

        return playerHorizontalMovement.magnitude > minimumMagnitude;
    }
    protected void SetBaseRotationData()
    {
        stateMachine.reusableData.rotationData = movementData.rotationData;
        stateMachine.reusableData.TimeToReachTargetRotation = stateMachine.reusableData.rotationData.targetRoationReachTime;
    }

    protected bool isMovingUp(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y > minimumVelocity;
    }
    protected bool isMovingDown(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y < -minimumVelocity;
    }
    protected virtual void OnContactWithGround(Collider collider)
    {

    }
    protected virtual void OnContactWithGroundExited(Collider collider)
    {

    }
    #endregion


    #region Input Methods
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        stateMachine.reusableData.shoudWalk = !stateMachine.reusableData.shoudWalk;
    }

    #endregion
}

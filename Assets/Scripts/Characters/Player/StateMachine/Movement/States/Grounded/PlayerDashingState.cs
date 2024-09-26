using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerGroundedState
{
    private PlayerDashData dashData;
    private float startTime;
    private int consecutiveDashesUsed;
    private bool shouldKeepRotating;
    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        dashData = movementData.dashData;
    }
    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = dashData.speedModifier;
        base.Enter();

        stateMachine.reusableData.rotationData = dashData.RotationData;
        stateMachine.reusableData.currentJumpForce = airborneData.jumpData.strongForce;

        Dash();
        shouldKeepRotating = stateMachine.reusableData.movementInput != Vector2.zero;
        UpdateConsecutiveDashes();

        startTime = Time.time;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!shouldKeepRotating)
        {
            return;
        }
        RotateTowardsTargetRotation();
    }
    public override void Exit()
    {
        base.Exit();
        SetBaseRotationData();
    }
    public override void OnAnimationTransitionEvent()
    {
        if (stateMachine.reusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.hardStoppingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.sprintingState);
    }

    #endregion
    #region Main Methods
    private void Dash()
    {
        Vector3 dashDirection = stateMachine.Player.transform.forward;
        dashDirection.y = 0f;

        UpdateTargetRotation(dashDirection, false);
        if (stateMachine.reusableData.movementInput != Vector2.zero)
        {
            UpdateTargetRotation(GetMovementInputDirection());
            dashDirection = GetTargetRotationDirection(stateMachine.reusableData.CurrentTargetRotation.y);
        }


        stateMachine.Player.myRigidbody.velocity = dashDirection * GetMovementSpeed(false);

    }

    private void UpdateConsecutiveDashes()
    {
        if (!IsConsecutive())
        {
            consecutiveDashesUsed = 0;
        }
        ++consecutiveDashesUsed;
        if (consecutiveDashesUsed == dashData.consecutiveDashesLimitAmount)
        {
            consecutiveDashesUsed = 0;
            stateMachine.Player.playerInput.DisableActionFor(stateMachine.Player.playerInput.playerActions.Dash, dashData.dashLimitReachCooldown);

        }
    }

    private bool IsConsecutive()
    {
        return Time.time < startTime + dashData.timeToBeConsideredConsecutive;
    }
    #endregion

    #region Reusable Methods
    protected override void AddInputActionCallBack()
    {
        base.AddInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Movement.performed += OnMovementPerformed;
    }
    protected override void RemoveInputActionCallBack()
    {
        base.RemoveInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Movement.performed -= OnMovementPerformed;

    }


    #endregion

    #region Input Methods
    protected override void OnDashStarted(InputAction.CallbackContext context)
    {

    }
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        shouldKeepRotating = true;
    }
    #endregion
}

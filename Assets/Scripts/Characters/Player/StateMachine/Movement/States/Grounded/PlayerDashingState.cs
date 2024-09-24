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
    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        dashData = movementData.dashData;
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();

        stateMachine.reusableData.movementSpeedModifier = dashData.speedModifier;

        AddForceOnTransitionFromStationeryState();
        UpdateConsecutiveDashes();

        startTime = Time.time;
    }
    public override void OnAnimationTransitionEvent()
    {
        base.OnAnimationTransitionEvent();
        if (stateMachine.reusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.idlingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.sprintingState);
    }

    #endregion
    #region Main Methods
    private void AddForceOnTransitionFromStationeryState()
    {
        if (stateMachine.reusableData.movementInput != Vector2.zero)
        {
            return;
        }
        Vector3 characterRotationDirection = stateMachine.Player.transform.forward;
        characterRotationDirection.y = 0f;

        stateMachine.Player.myRigidbody.velocity = characterRotationDirection * GetMoveSpeed();

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
    #region Input Methods
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }
    protected override void OnDashStarted(InputAction.CallbackContext context)
    {

    }
    #endregion
}

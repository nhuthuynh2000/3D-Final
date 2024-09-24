using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

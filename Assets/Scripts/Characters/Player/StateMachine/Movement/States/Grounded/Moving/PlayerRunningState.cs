using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerMovingState
{
    private PlayerSprintData sprintData;
    private float startTime;
    public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        sprintData = movementData.sprintData;
    }
    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = movementData.runData.speedModifier;
        base.Enter();

        stateMachine.reusableData.currentJumpForce = airborneData.jumpData.mediumForce;

        startTime = Time.time;
    }
    public override void Update()
    {
        base.Update();
        if (!stateMachine.reusableData.shoudWalk)
        {
            return;
        }
        if (Time.time < startTime + sprintData.runToWalkTime)
        {
            return;
        }
        StopRunning();
    }


    #endregion

    #region Main Methods
    private void StopRunning()
    {
        if (stateMachine.reusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.idlingState);
            return;
        }
        stateMachine.ChangeState(stateMachine.walkingState);
    }
    #endregion

    #region Input Methods
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.mediumStoppingState);
        base.OnMovementCanceled(context);
    }
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        stateMachine.ChangeState(stateMachine.walkingState);
    }
    #endregion
}

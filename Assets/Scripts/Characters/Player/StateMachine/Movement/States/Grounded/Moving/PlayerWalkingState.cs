using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerMovingState
{
    private PlayerWalkData walkData;
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        walkData = movementData.walkData;
    }
    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = walkData.speedModifier;
        stateMachine.reusableData.backwardsCameraRecenteringData = walkData.backwardsCameraRecenteringData;
        base.Enter();
        StartAnimation(stateMachine.Player.animationsData.walkParameterHash);
        stateMachine.reusableData.currentJumpForce = airborneData.jumpData.weakForce;

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationsData.walkParameterHash);
        SetBaseCameraRecenteringData();
    }
    #endregion

    #region Input Methods
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.lightStoppingState);
        base.OnMovementCanceled(context);
    }
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        stateMachine.ChangeState(stateMachine.runningState);
    }
    #endregion
}

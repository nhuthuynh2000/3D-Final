using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
    private PlayerIdleData idleData;
    public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        idleData = movementData.idleData;
    }
    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = 0f;
        stateMachine.reusableData.backwardsCameraRecenteringData = idleData.backwardsCameraRecenteringData;
        base.Enter();
        StartAnimation(stateMachine.Player.animationsData.idleParameterHash);
        stateMachine.reusableData.currentJumpForce = airborneData.jumpData.stationeryForce;
        ResetVelocity();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationsData.idleParameterHash);

    }
    public override void Update()
    {
        base.Update();
        if (stateMachine.reusableData.movementInput == Vector2.zero)
        {
            return;
        }
        OnMove();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!isMovingHorizontally())
        {
            return;
        }
        ResetVelocity();
    }
    #endregion
}

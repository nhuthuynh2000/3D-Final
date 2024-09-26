using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAirborneState
{
    private PlayerFallData fallData;
    private Vector3 playerPositionOnEnter;
    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        fallData = airborneData.fallData;
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.animationsData.fallParameterHash);
        playerPositionOnEnter = stateMachine.Player.transform.position;
        stateMachine.reusableData.movementSpeedModifier = 0f;
        ResetVerticalVelocity();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationsData.fallParameterHash);

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        LimitVerticalVelocity();
    }
    #endregion

    #region Reusable Methods
    protected override void ResetSprintState()
    {

    }

    protected override void OnContactWithGround(Collider collider)
    {
        float fallDistance = playerPositionOnEnter.y - stateMachine.Player.transform.position.y;

        if (fallDistance < fallData.minimumDistanceToBeConsideredHardFall)
        {
            stateMachine.ChangeState(stateMachine.lightLandingState);
            return;
        }
        if (stateMachine.reusableData.shoudWalk && !stateMachine.reusableData.shouldSprint || stateMachine.reusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.hardLandingState);
            return;
        }
        stateMachine.ChangeState(stateMachine.rollingState);
    }
    #endregion

    #region Main Methods
    private void LimitVerticalVelocity()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
        if (stateMachine.Player.myRigidbody.velocity.y >= -fallData.fallSpeedLimit)
        {
            return;
        }
        Vector3 limitedVelocity = new Vector3(0f, -fallData.fallSpeedLimit - playerVerticalVelocity.y, 0f);
        stateMachine.Player.myRigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
    }
    #endregion
}

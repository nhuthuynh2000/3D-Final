using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAirborneState
{
    private PlayerFallData fallData;
    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        fallData = airborneData.fallData;
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        stateMachine.reusableData.movementSpeedModifier = 0f;
        ResetVerticalVelocity();
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

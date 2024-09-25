using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerAirborneState
{
    private PlayerJumpData jumpData;
    private bool shouldKeepRotating;
    private bool canStartFalling;
    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        jumpData = airborneData.jumpData;
    }
    #region IState Method
    public override void Enter()
    {
        base.Enter();
        stateMachine.reusableData.movementSpeedModifier = 0f;
        stateMachine.reusableData.movementDecelerationForce = jumpData.DecelerationForce;
        shouldKeepRotating = stateMachine.reusableData.movementInput != Vector2.zero;
        Jump();
    }
    public override void Exit()
    {
        base.Exit();
        SetBaseRotationData();
        canStartFalling = false;
    }
    public override void Update()
    {
        base.Update();
        if (!canStartFalling && isMovingUp(0f))
        {
            canStartFalling = true;
        }
        if (!canStartFalling || GetPlayerVerticalVelocity().y > 0)
        {
            return;
        }
        stateMachine.ChangeState(stateMachine.fallingState);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (shouldKeepRotating)
        {
            RotateTowardsTargetRotation();
        }
        if (isMovingUp())
        {
            DecelerateVertically();
        }
    }

    #endregion

    #region Main Methods
    private void Jump()
    {
        Vector3 jumpForce = stateMachine.reusableData.currentJumpForce;

        Vector3 jumpDirection = stateMachine.Player.transform.forward;

        if (shouldKeepRotating)
        {
            jumpDirection = GetTargetRotationDirection(stateMachine.reusableData.CurrentTargetRotation.y);
        }

        jumpForce.x *= jumpDirection.x;
        jumpForce.z *= jumpDirection.z;

        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.colliderUtility.capsuleColliderData.collider.bounds.center;
        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, jumpData.jumptToGroundRayDistance, stateMachine.Player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);
            if (isMovingUp())
            {
                float forceModifier = jumpData.jumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);
                jumpForce.x *= forceModifier;
                jumpForce.z *= forceModifier;
            }
            if (isMovingDown())
            {
                float forceModifier = jumpData.jumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);
                jumpForce.y *= forceModifier;
            }
        }

        ResetVelocity();

        stateMachine.Player.myRigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
    }
    #endregion

    #region Resuable Methods
    protected override void ResetSprintState()
    {

    }
    #endregion
}

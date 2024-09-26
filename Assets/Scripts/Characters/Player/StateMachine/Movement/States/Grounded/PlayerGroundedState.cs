using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerGroundedState : PlayerMovementStates
{
    private SlopeData slopeData;
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        slopeData = stateMachine.Player.colliderUtility.slopeData;
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        UpdateShouldSprintState();

        UpdateCameraRecenteringState(stateMachine.reusableData.movementInput);
    }



    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Float();
    }
    #endregion
    #region Main Methods
    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.colliderUtility.capsuleColliderData.collider.bounds.center;
        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.stepReachForce, stateMachine.Player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);
            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);
            if (slopeSpeedModifier == 0f)
            {
                return;
            }
            float distanceToFloatingPoint = stateMachine.Player.colliderUtility.capsuleColliderData.colliderCenterInLocalSpace.y * stateMachine.Player.transform.localScale.y - hit.distance;
            if (distanceToFloatingPoint == 0f)
            {
                return;
            }
            float amountToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
            stateMachine.Player.myRigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = movementData.slopeSpeedAngle.Evaluate(angle);
        if (stateMachine.reusableData.movementOnSlopeSpeedModifier != slopeSpeedModifier)
        {
            stateMachine.reusableData.movementOnSlopeSpeedModifier = slopeSpeedModifier;
            UpdateCameraRecenteringState(stateMachine.reusableData.movementInput);

        }

        stateMachine.reusableData.movementOnSlopeSpeedModifier = slopeSpeedModifier;

        return slopeSpeedModifier;
    }
    private void UpdateShouldSprintState()
    {
        if (!stateMachine.reusableData.shouldSprint)
        {
            return;
        }
        if (stateMachine.reusableData.movementInput != Vector2.zero)
        {
            return;
        }
        stateMachine.reusableData.shouldSprint = false;
    }
    private bool IsThereGroundUnderneath()
    {
        BoxCollider groundCheckCollider = stateMachine.Player.colliderUtility.triggerColliderData.groundCheckCollider;
        Vector3 groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;
        Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, stateMachine.Player.colliderUtility.triggerColliderData.groundCheckColliderExtents, groundCheckCollider.transform.rotation, stateMachine.Player.layerData.groundLayer, QueryTriggerInteraction.Ignore);
        return overlappedGroundColliders.Length > 0;
    }
    #endregion
    #region Reusable Methods
    protected override void AddInputActionCallBack()
    {
        base.AddInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Dash.started += OnDashStarted;
        stateMachine.Player.playerInput.playerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionCallBack()
    {
        base.RemoveInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Dash.started -= OnDashStarted;
        stateMachine.Player.playerInput.playerActions.Jump.started -= OnJumpStarted;

    }


    protected virtual void OnMove()
    {
        if (stateMachine.reusableData.shouldSprint)
        {
            stateMachine.ChangeState(stateMachine.sprintingState);
            return;
        }
        if (stateMachine.reusableData.shoudWalk)
        {
            stateMachine.ChangeState(stateMachine.walkingState);
            return;
        }
        stateMachine.ChangeState(stateMachine.runningState);
    }
    protected override void OnContactWithGroundExited(Collider collider)
    {
        base.OnContactWithGroundExited(collider);
        if (IsThereGroundUnderneath())
        {
            return;
        }
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.colliderUtility.capsuleColliderData.collider.bounds.center;
        Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine.Player.colliderUtility.capsuleColliderData.colliderVerticalExtents, Vector3.down);
        if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, movementData.groundToFallRayDistance, stateMachine.Player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            OnFall();
        }

    }



    protected virtual void OnFall()
    {
        stateMachine.ChangeState(stateMachine.fallingState);
    }
    #endregion

    #region Input Methods
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        stateMachine.ChangeState(stateMachine.runningState);
    }
    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.dashingState);
    }
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.jumpingState);
    }
    #endregion
}

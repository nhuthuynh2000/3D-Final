using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = 0f;
        SetBaseCameraRecenteringData();
        base.Enter();

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        RotateTowardsTargetRotation();
        if (!isMovingHorizontally())
        {
            return;
        }
        DecelerateHorizontally();
    }
    public override void OnAnimationTransitionEvent()
    {

        stateMachine.ChangeState(stateMachine.idlingState);
    }
    #endregion
    #region Reusable Methods
    protected override void AddInputActionCallBack()
    {
        base.AddInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Movement.started += OnMovementStarted;
    }
    protected override void RemoveInputActionCallBack()
    {
        base.RemoveInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Movement.started += OnMovementStarted;

    }
    #endregion

    #region Input Methods
    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHardLandingState : PlayerLandingState
{
    public PlayerHardLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = 0f;
        base.Enter();
        stateMachine.Player.playerInput.playerActions.Movement.Disable();

        ResetVelocity();
    }
    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.playerInput.playerActions.Movement.Enable();
    }
    public override void OnAnimationExitEvent()
    {
        stateMachine.Player.playerInput.playerActions.Movement.Enable();
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



    protected override void OnMove()
    {
        if (stateMachine.reusableData.shoudWalk)
        {
            return;
        }
        stateMachine.ChangeState(stateMachine.runningState);
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

    #region Input Methods
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {

    }
    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }
    #endregion
}

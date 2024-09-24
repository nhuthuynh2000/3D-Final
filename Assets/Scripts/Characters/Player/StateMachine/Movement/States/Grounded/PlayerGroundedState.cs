using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementStates
{
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region Reusable Methods
    protected override void AddInputActionCallBack()
    {
        base.AddInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Movement.canceled += OnMovementCanceled;
    }

    protected override void RemoveInputActionCallBack()
    {
        base.RemoveInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Movement.canceled -= OnMovementCanceled;
    }
    protected virtual void OnMove()
    {
        if (shouldWalk)
        {
            stateMachine.ChangeState(stateMachine.walkingState);
            return;
        }
        stateMachine.ChangeState(stateMachine.runningState);
    }
    #endregion

    #region Input Methods
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);
        stateMachine.ChangeState(stateMachine.runningState);
    }
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.idlingState);
    }

    #endregion
}

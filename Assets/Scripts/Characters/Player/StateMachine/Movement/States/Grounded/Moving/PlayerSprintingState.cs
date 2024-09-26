using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerMovingState
{
    private PlayerSprintData sprintData;
    private float startTime;
    private bool keepSprinting;
    private bool shouldResetSprintState;
    public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        sprintData = movementData.sprintData;
    }
    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = sprintData.speedModifier;
        base.Enter();


        stateMachine.reusableData.currentJumpForce = airborneData.jumpData.strongForce;
        shouldResetSprintState = true;

        startTime = Time.time;
    }
    public override void Exit()
    {
        base.Exit();
        if (shouldResetSprintState)
        {
            keepSprinting = false;
            stateMachine.reusableData.shouldSprint = false;
        }

    }
    public override void Update()
    {
        base.Update();
        if (keepSprinting)
        {
            return;
        }

        if (Time.time < startTime + sprintData.sprintToRunTime)
        {
            return;
        }

        StopSprinting();
    }


    #endregion

    #region Main Methods
    private void StopSprinting()
    {
        if (stateMachine.reusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.idlingState);
            return;
        }
        stateMachine.ChangeState(stateMachine.runningState);
    }
    #endregion

    #region Reusable Methods
    protected override void AddInputActionCallBack()
    {
        base.AddInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Sprint.performed += OnSprintPerformed;
    }
    protected override void RemoveInputActionCallBack()
    {
        base.RemoveInputActionCallBack();
        stateMachine.Player.playerInput.playerActions.Sprint.performed -= OnSprintPerformed;

    }
    protected override void OnFall()
    {
        shouldResetSprintState = false;

        base.OnFall();
    }
    #endregion

    #region Input Methods
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.hardStoppingState);
        base.OnMovementCanceled(context);
    }
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        shouldResetSprintState = false;
        base.OnJumpStarted(context);
    }
    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        keepSprinting = true;
        stateMachine.reusableData.shouldSprint = true;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRollingState : PlayerLandingState
{
    private PlayerRollData rollData;
    public PlayerRollingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        rollData = movementData.rollData;
    }

    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = rollData.speedModifier;
        base.Enter();
        StartAnimation(stateMachine.Player.animationsData.rollParameterHash);
        stateMachine.reusableData.shouldSprint = false;
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationsData.rollParameterHash);

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (stateMachine.reusableData.movementInput != Vector2.zero)
        {
            return;
        }
        RotateTowardsTargetRotation();
    }

    public override void OnAnimationTransitionEvent()
    {
        if (stateMachine.reusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.mediumStoppingState);
            return;
        }
        OnMove();
    }
    #endregion

    #region Input Methods
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {

    }
    #endregion
}

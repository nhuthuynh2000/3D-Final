using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightLandingState : PlayerLandingState
{
    public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        stateMachine.reusableData.movementSpeedModifier = 0f;
        base.Enter();

        stateMachine.reusableData.currentJumpForce = airborneData.jumpData.stationeryForce;
        ResetVelocity();
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

    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.idlingState);
    }
    #endregion
}

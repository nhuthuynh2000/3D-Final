using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerMovementStates
{
    public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.animationsData.airborneParameterHash);
        ResetSprintState();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationsData.airborneParameterHash);
    }
    #endregion
    #region Reusable Methods
    protected override void OnContactWithGround(Collider collider)
    {
        base.OnContactWithGround(collider);
        stateMachine.ChangeState(stateMachine.lightLandingState);
    }
    protected virtual void ResetSprintState()
    {
        stateMachine.reusableData.shouldSprint = false;
    }
    #endregion
}

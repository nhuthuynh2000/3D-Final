using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLandingState : PlayerGroundedState
{
    public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.animationsData.landingParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationsData.landingParameterHash);
    }
    #endregion
    #region Input Methods
    #endregion
}

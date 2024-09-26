using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMediumStoppingState : PlayerStoppingState
{
    public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.animationsData.mediumStopParameterHash);
        stateMachine.reusableData.movementDecelerationForce = movementData.stopData.mediumDecelerationForce;
        stateMachine.reusableData.currentJumpForce = airborneData.jumpData.mediumForce;

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationsData.mediumStopParameterHash);

    }
    #endregion
}

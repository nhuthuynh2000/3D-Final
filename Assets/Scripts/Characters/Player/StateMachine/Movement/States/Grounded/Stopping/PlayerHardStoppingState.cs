using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardStoppingState : PlayerStoppingState
{
    public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        stateMachine.reusableData.movementDecelerationForce = movementData.stopData.hardDecelerationForce;
        stateMachine.reusableData.currentJumpForce = airborneData.jumpData.strongForce;

    }
    #endregion

    #region Reusable Methods
    protected override void OnMove()
    {
        if (stateMachine.reusableData.shoudWalk)
        {
            return;
        }
        stateMachine.ChangeState(stateMachine.runningState);
    }
    #endregion
}

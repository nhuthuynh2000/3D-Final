using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
    public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();
        stateMachine.reusableData.movementSpeedModifier = 0f;
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
    #endregion
}

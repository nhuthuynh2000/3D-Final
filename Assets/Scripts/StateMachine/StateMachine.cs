using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine
{
    protected IState curState;

    public void ChangeState(IState newState)
    {
        if (curState != null)
        {
            curState.Exit();
        }
        curState = newState;
        curState.Enter();
    }
    public void HandleInput()
    {
        curState?.HandleInput();
    }
    public void Update()
    {
        curState?.Update();
    }
    public void PhysicsUpdate()
    {
        curState?.PhysicsUpdate();
    }
}

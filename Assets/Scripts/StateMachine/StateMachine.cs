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
    public void OnAnimationEnterEvent()
    {
        curState?.OnAnimationEnterEvent();
    }
    public void OnAnimationExitEvent()
    {
        curState?.OnAnimationExitEvent();
    }
    public void OnAnimationTransitionEvent()
    {
        curState?.OnAnimationTransitionEvent();
    }
    public void OnTriggerEnter(Collider collider)
    {
        curState.OnTriggerEnter(collider);
    }
    public void OnTriggerExit(Collider collider)
    {
        curState.OnTriggerExit(collider);
    }
}

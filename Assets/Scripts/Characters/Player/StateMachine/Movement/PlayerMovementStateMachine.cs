using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    public Player Player { get; }
    public PlayerStateReusableData reusableData { get; }
    public PlayerIdlingState idlingState { get; }
    public PlayerWalkingState walkingState { get; }
    public PlayerRunningState runningState { get; }
    public PlayerSprintingState sprintingState { get; }

    public PlayerMovementStateMachine(Player player)
    {
        Player = player;
        reusableData = new PlayerStateReusableData();
        idlingState = new PlayerIdlingState(this);
        walkingState = new PlayerWalkingState(this);
        runningState = new PlayerRunningState(this);
        sprintingState = new PlayerSprintingState(this);
    }
}

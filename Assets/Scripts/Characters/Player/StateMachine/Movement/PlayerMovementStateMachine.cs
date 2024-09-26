using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    public Player Player { get; }
    public PlayerStateReusableData reusableData { get; }
    public PlayerIdlingState idlingState { get; }
    public PlayerDashingState dashingState { get; }

    public PlayerWalkingState walkingState { get; }
    public PlayerRunningState runningState { get; }
    public PlayerSprintingState sprintingState { get; }
    public PlayerLightStoppingState lightStoppingState { get; }
    public PlayerMediumStoppingState mediumStoppingState { get; }
    public PlayerHardStoppingState hardStoppingState { get; }
    public PlayerJumpingState jumpingState { get; }
    public PlayerFallingState fallingState { get; }
    public PlayerLightLandingState lightLandingState { get; }
    public PlayerHardLandingState hardLandingState { get; }
    public PlayerRollingState rollingState { get; }

    public PlayerMovementStateMachine(Player player)
    {
        Player = player;
        reusableData = new PlayerStateReusableData();
        idlingState = new PlayerIdlingState(this);
        dashingState = new PlayerDashingState(this);
        walkingState = new PlayerWalkingState(this);
        runningState = new PlayerRunningState(this);
        sprintingState = new PlayerSprintingState(this);

        lightStoppingState = new PlayerLightStoppingState(this);
        mediumStoppingState = new PlayerMediumStoppingState(this);
        hardStoppingState = new PlayerHardStoppingState(this);

        jumpingState = new PlayerJumpingState(this);
        fallingState = new PlayerFallingState(this);

        lightLandingState = new PlayerLightLandingState(this);
        hardLandingState = new PlayerHardLandingState(this);
        rollingState = new PlayerRollingState(this);
    }
}

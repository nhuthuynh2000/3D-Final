using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReusableData
{
    public Vector2 movementInput { get; set; }
    public float movementSpeedModifier { get; set; } = 1f;
    public float movementOnSlopeSpeedModifier { get; set; } = 1f;
    public float movementDecelerationForce { get; set; } = 1f;
    public List<PlayerCameraRecenteringData> backwardsCameraRecenteringData { get; set; }
    public List<PlayerCameraRecenteringData> sidewaysCameraRecenteringData { get; set; }

    public bool shoudWalk { get; set; }
    public bool shouldSprint { get; set; }

    private Vector3 currentTargetRotation;
    private Vector3 timeToReachTargetRotation;
    private Vector3 dampedTargetRotationCurrentVelocity;
    private Vector3 dampedTargetRotationPassedTime;
    public ref Vector3 CurrentTargetRotation
    {
        get
        {
            return ref currentTargetRotation;
        }
    }
    public ref Vector3 TimeToReachTargetRotation
    {
        get
        {
            return ref timeToReachTargetRotation;
        }
    }
    public ref Vector3 DampedTargetRotationCurrentVelocity
    {
        get
        {
            return ref dampedTargetRotationCurrentVelocity;
        }
    }
    public ref Vector3 DampedTargetRotationPassedTime
    {
        get
        {
            return ref dampedTargetRotationPassedTime;
        }
    }
    public Vector3 currentJumpForce { get; set; }

    public PlayerRotationData rotationData { get; set; }
}

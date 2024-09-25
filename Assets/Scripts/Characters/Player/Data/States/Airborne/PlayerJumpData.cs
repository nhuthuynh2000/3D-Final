using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerJumpData
{
    [field: SerializeField] public PlayerRotationData rotationData { get; private set; }
    [field: SerializeField][field: Range(0f, 5f)] public float jumptToGroundRayDistance { get; private set; } = 2f;
    [field: SerializeField] public AnimationCurve jumpForceModifierOnSlopeUpwards { get; private set; }
    [field: SerializeField] public AnimationCurve jumpForceModifierOnSlopeDownwards { get; private set; }

    [field: SerializeField] public Vector3 stationeryForce { get; private set; }
    [field: SerializeField] public Vector3 weakForce { get; private set; }
    [field: SerializeField] public Vector3 mediumForce { get; private set; }
    [field: SerializeField] public Vector3 strongForce { get; private set; }
    [field: SerializeField][field: Range(0f, 10f)] public float DecelerationForce { get; private set; } = 1.5f;

}

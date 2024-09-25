using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerAirborneData
{
    [field: SerializeField] public PlayerJumpData jumpData { get; private set; }
    [field: SerializeField] public PlayerFallData fallData { get; private set; }
}

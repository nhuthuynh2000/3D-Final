using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerIdleData
{
    [field: SerializeField] public List<PlayerCameraRecenteringData> backwardsCameraRecenteringData { get; private set; }
}

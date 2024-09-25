using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerFallData : MonoBehaviour
{
    [field: SerializeField][field: Range(1f, 15f)] public float fallSpeedLimit { get; private set; } = 15f;
}

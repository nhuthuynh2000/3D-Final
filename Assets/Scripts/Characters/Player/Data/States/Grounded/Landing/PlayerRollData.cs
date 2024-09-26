using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerRollData
{
    [field: SerializeField][field: Range(0f, 3f)] public float speedModifier { get; private set; } = 1f;
}

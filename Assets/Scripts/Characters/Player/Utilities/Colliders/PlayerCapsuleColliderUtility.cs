using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCapsuleColliderUtility : CapsuleColliderUtility
{
    [field: SerializeField] public PlayerTriggerColliderData triggerColliderData { get; private set; }
    protected override void OnInitialize()
    {
        base.OnInitialize();
        triggerColliderData.Initialize();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputActions { get; private set; }
    public PlayerInputActions.PlayerActions playerActions { get; private set; }
    private void Awake()
    {
        inputActions = new PlayerInputActions();

        playerActions = inputActions.Player;
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
}

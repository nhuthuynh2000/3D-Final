using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Transform mainCameraTransform { get; private set; }
    public PlayerInput playerInput { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        mainCameraTransform = Camera.main.transform;
        movementStateMachine = new PlayerMovementStateMachine(this);
    }
    private void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.idlingState);
    }
    private void Update()
    {
        movementStateMachine.HandleInput();
        movementStateMachine.Update();
    }
    private void FixedUpdate()
    {
        movementStateMachine.PhysicsUpdate();
    }
}

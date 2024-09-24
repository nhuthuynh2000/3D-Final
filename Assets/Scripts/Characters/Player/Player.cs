using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: Header("References:")]
    [field: SerializeField] public PlayerSO Data { get; private set; }
    [field: Header("Collisions")]
    [field: SerializeField] public CapsuleColliderUtility colliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData layerData { get; private set; }
    public Rigidbody myRigidbody;
    public Transform mainCameraTransform { get; private set; }
    public PlayerInput playerInput { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        colliderUtility.Initialize(gameObject);
        colliderUtility.CalculateCapsuleColliderDimensions();

        mainCameraTransform = Camera.main.transform;
        movementStateMachine = new PlayerMovementStateMachine(this);
    }
    private void OnValidate()
    {
        colliderUtility.Initialize(gameObject);
        colliderUtility.CalculateCapsuleColliderDimensions();
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

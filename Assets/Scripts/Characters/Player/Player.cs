using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: Header("References:")]
    [field: SerializeField] public PlayerSO Data { get; private set; }
    [field: Header("Collisions")]
    [field: SerializeField] public PlayerCapsuleColliderUtility colliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData layerData { get; private set; }
    [field: Header("Cameras")]
    [field: SerializeField] public PlayerCameraUtility cameraUtility { get; private set; }
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationsData animationsData { get; private set; }
    public Rigidbody myRigidbody { get; private set; }
    public Animator animator { get; private set; }
    public Transform mainCameraTransform { get; private set; }
    public PlayerInput playerInput { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();

        colliderUtility.Initialize(gameObject);
        colliderUtility.CalculateCapsuleColliderDimensions();
        cameraUtility.Initialize();
        animationsData.Initialize();

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
    private void OnTriggerEnter(Collider collider)
    {
        movementStateMachine.OnTriggerEnter(collider);
    }
    private void OnTriggerExit(Collider collider)
    {
        movementStateMachine.OnTriggerExit(collider);
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
    public void OnMovementStateAnimationEnterEvent()
    {
        movementStateMachine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        movementStateMachine.OnAnimationExitEvent();
    }
    public void OnMovementStateAnimationTransitionEvent()
    {
        movementStateMachine.OnAnimationTransitionEvent();
    }
}

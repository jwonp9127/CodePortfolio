using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [Range(0, 10)] public float movementSpeed;
    [SerializeField] private bool ignorePlayerMove;

    [Header("Rotation")]
    [Range(0, 30)] public float rotationDamping = 15;

    [Header("Jump")]
    [Range(0, 10)] public float jumpForce;
    [Range(0, 3)] public float doubleJump = 1.5f;
    
    [Header("Cam")]
    public CamChange camChange;
    
    
    public Rigidbody Rigidbody { get; private set; }
    public Collider Collider { get; private set; }
    public PlayerInput Input { get; private set; }
    public Vector2 MovementInput { get; protected set; }
    public Transform CameraTransform { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }

    public RunSFX runSFX;


    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        Input = GetComponent<PlayerInput>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Animator = GetComponentInChildren<Animator>();

        if (Camera.main != null)
        {
            CameraTransform = Camera.main.transform;
        }
    }

    protected virtual void Start()
    {
        Input.PlayerActions.Jump.started += OnJumpStarted;
    }

    private void Update()
    {
        ReadMovementInput();
        Move();
    }

    protected virtual void ReadMovementInput()
    {
        MovementInput = Input.PlayerActions.Movement.ReadValue<Vector2>();
        Animator.SetBool("Run", MovementInput != Vector2.zero);

        if(ForceReceiver.isGrounded == true)
        {
            if (MovementInput != Vector2.zero)
            {
                runSFX._isRunning = true;
                runSFX.OnRunSFX();
            }
            else
            {
                runSFX._isRunning = false;
            }
        }
    }

    private void Move()
    {
        Vector3 movementDirection = ignorePlayerMove ? Vector3.zero : GetMovementDirection();
        Rotate(movementDirection);
        Move(movementDirection);
        AdditionalMove();
    }

    private void Move(Vector3 movementDirection)
    {
        movementDirection *= movementSpeed;
        movementDirection.y = Rigidbody.velocity.y;
        Rigidbody.velocity = movementDirection;
    }

    private void AdditionalMove()
    {
        Rigidbody.velocity += ForceReceiver.additionalVelocity;
        if (ForceReceiver.additionalVelocity != Vector3.zero)
        {
            ForceReceiver.DampAdditionalVelocity();
        }
    }
    
    protected virtual Vector3 GetMovementDirection()
    {
        Vector3 forward = CameraTransform.forward;
        Vector3 right = CameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();
        
        return forward * MovementInput.y + right * MovementInput.x;
    }

    private void Rotate(Vector3 movementDirection)
    {
        if (movementDirection == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationDamping * Time.deltaTime);
    }

    private void OnJumpStarted(InputAction.CallbackContext obj)
    {
        ForceReceiver.Jump(jumpForce, doubleJump);
    }

    public void FreezeZPosition()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX |
                                RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void UnFreezeZPosition()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
                                RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }


    public void OnTriggerEnter(Collider other)

    {
        if (other.CompareTag("ChangePoint1"))
        {
            camChange.SetCamPoint(0);
        }
        else if (other.CompareTag("ChangePoint2"))
        {
            camChange.SetCamPoint(1);
        }
        else if (other.CompareTag("ChangePoint3"))
        {
            camChange.SetCamPoint(2);
        }
        else if (other.CompareTag("ChangePoint4"))
        {
            camChange.SetCamPoint(3);
        }
    }
}
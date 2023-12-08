using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public enum Gimmicks
{
    AddForce,
    AddVelocity,
    AddPosition
}
public class ForceReceiver : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }

    [SerializeField] [Range(0, 5)] private int canJumpCount = 2;
    [SerializeField] [Range(0, 5)] private int currentJumpCount = 0;
    [SerializeField] [Range(0, 1)] private float maxDistance = 0.5f;
    [SerializeField] [Range(-20, 0)] private float additionalGravity = -9.81f;
    [SerializeField] [Range(0, 10f)] private float defaultDrag = 0.6f;

    //[SerializeField] private GameObject _jumpEffectPrefab;

    public AudioSource JumpSFX;
    public RunSFX runSFX;

    private float _drag;
    public bool isGrounded;
    private bool _isFalling;
    public Vector3 additionalVelocity;
    private Vector3 _dampingVelocity;
    public bool ignorePlayerStatus;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();

        JumpSFX = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (ignorePlayerStatus)
        {
            return;
        }
        CheckIsGrounded();
        CheckIsFalling();
    }

    public void Jump(float jumpForce, float doubleJump)
    {
        if (currentJumpCount < canJumpCount)
        {
            SetVelocity(y: currentJumpCount == 0 ? jumpForce : jumpForce * doubleJump);
            
            Animator.SetTrigger("Jump");
            EnterAir();
            
            currentJumpCount++;
            Animator.SetInteger("CurrentJumpCount", currentJumpCount);

            JumpSFX.volume = 0.05f;
            JumpSFX.Play();
            //OnJumpEffect();
        }
        else
        {
        }
    }

    private void CheckIsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + transform.forward * 0.25f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position - transform.forward * 0.25f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + transform.right * 0.25f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position - transform.right * 0.25f + Vector3.up * 0.01f, Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], maxDistance, LayerMask.GetMask("Ground")))
            {
                // OnGround
                if (!isGrounded)
                {
                    EnterGround();
                }
                return;
            }
        }
        // NotOnGround
        if (isGrounded)
        {
            EnterAir();
        }
    }
    
    public void EnterGround()
    {
        Animator.SetBool("OnAir", false);
        Animator.SetBool("IsGrounded", true);
        
        _isFalling = false;
        Animator.SetBool("IsFalling", false);
        
        currentJumpCount = 0;
        Animator.SetInteger("CurrentJumpCount", currentJumpCount);

        isGrounded = true;
    }

    public void EnterAir()
    {
        Animator.SetBool("OnAir", true);
        Animator.SetBool("IsGrounded", false);

        isGrounded = false;
    }
    
    private void CheckIsFalling()
    {
        if (!_isFalling && Rigidbody.velocity.y < 5f && !isGrounded)
        {
            // EnterFalling
            _isFalling = true;
            Animator.SetBool("IsFalling", true);
        }
        else if (_isFalling && isGrounded)
        {
            // ExitFalling
            _isFalling = false;
            Animator.SetBool("IsFalling", false);
        }
        else if (_isFalling)
        {
            // OnFalling
            OnFalling();
        }
    }

    private void OnFalling()
    {
        AddVelocity(y: additionalGravity * Time.deltaTime);
    }
    
    private void OnDrawGizmos()
    {
        Transform playerTransform = transform;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerTransform.position + playerTransform.forward * 0.25f + Vector3.up * 0.01f, Vector3.down * maxDistance);
        Gizmos.DrawRay(playerTransform.position - playerTransform.forward * 0.25f + Vector3.up * 0.01f, Vector3.down * maxDistance);
        Gizmos.DrawRay(playerTransform.position + playerTransform.right * 0.25f + Vector3.up * 0.01f, Vector3.down * maxDistance);
        Gizmos.DrawRay(playerTransform.position - playerTransform.right * 0.25f + Vector3.up * 0.01f, Vector3.down * maxDistance);
    }

    public void AddVelocity(float x = 0f, float y = 0f, float z = 0f, float drag = float.MaxValue)
    {
        additionalVelocity += new Vector3(x, y, z);
        drag = drag >= float.MaxValue ? defaultDrag : drag;
        _drag = drag > _drag ? drag : _drag;
    }
    
    public void AddVelocity(Vector3 velocity, float drag = float.MaxValue)
    {
        additionalVelocity += velocity;
        drag = drag >= float.MaxValue ? defaultDrag : drag;
        _drag = drag > _drag ? drag : _drag;
    }
    
    public void AddLocalVelocity(float forward = 0f, float up = 0f, float right = 0f, float drag = float.MaxValue)
    {
        Transform playerTransform = transform;
        additionalVelocity += playerTransform.forward * forward + playerTransform.up * up + playerTransform.right * right;
        drag = drag >= float.MaxValue ? defaultDrag : drag;
        _drag = drag > _drag ? drag : _drag;
    }
    
    public void DampAdditionalVelocity()
    {
        Vector3 velocity = Vector3.SmoothDamp(additionalVelocity, Vector3.zero, ref _dampingVelocity, defaultDrag);
        velocity.y = 0;
        additionalVelocity = velocity;
    }
    
    public void SetVelocity(float x = float.MaxValue, float y = float.MaxValue, float z = float.MaxValue)
    {
        Vector3 velocity = Rigidbody.velocity;
        velocity.x = x >= float.MaxValue ? velocity.x : x;
        velocity.y = y >= float.MaxValue ? velocity.y : y;
        velocity.z = z >= float.MaxValue ? velocity.z : z;
        Rigidbody.velocity = velocity;
    }
    
    public void StartGimmick(Gimmicks gimmick, Rigidbody rb, float x, float y, float z, float force)
    {
        switch (gimmick)
        {
            case Gimmicks.AddForce:
                AddForce(rb, x, y, z, force);
                break;
            case Gimmicks.AddVelocity:
                AddVelocity(x, y, z);
                break;
            case Gimmicks.AddPosition:
                AddPosition(rb, x, y, z);
                break;
        }
    }
    private void AddForce(Rigidbody rb, float x, float y, float z, float force)
    {
        rb.AddForce((new Vector3(x, y, z)) * force);
    }

    private void AddPosition(Rigidbody rb, float x, float y, float z)
    {
        rb.transform.position += new Vector3(x, y, z);
    }

    public void OnJumpEffect()
    {
        var DoJumpEffect = ObjectPoolJump.GetObject();
        var direction = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
        DoJumpEffect.transform.position = direction.normalized;
        DoJumpEffect.OnJumpEffect(direction);
    }
}

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovePlayer1 : MonoBehaviour
{
    [Header("Movimiento y Rotación")]
    public float speedplayer = 5.0f;
    public float speedRotation = 10.0f;

    [Header("Físicas de Salto")]
    public float jumpForce = 5.0f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Mecánica de Escalar")]
    public float climbSpeed = 3.0f;
    public float climbCheckDistance = 1.2f;
    public LayerMask climbableLayer;
    private bool isClimbing = false;
    private bool canStartClimb = false;
    public string climbButton = "Climb";

    // Variables de control interno
    private float x;
    private float y;
    private Vector2 movementInput;

    private Animator animator;
    private Rigidbody rb;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Update()
    {
        CheckSurroundings();

        if (Input.GetButtonDown(climbButton))
        {
            ToggleClimb();
        }

        UpdateAnimator();
    }

    public void FixedUpdate()
    {
        if (!isClimbing)
        {
            PerformNormalMovement();
        }
        else
        {
            PerformClimbMovement();
        }
    }

    private void CheckSurroundings()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = false;
        }

        Vector3 rayOrigin = transform.position + Vector3.up * 1.0f;
        canStartClimb = Physics.Raycast(rayOrigin, transform.forward, climbCheckDistance, climbableLayer);

        Debug.DrawRay(rayOrigin, transform.forward * climbCheckDistance, canStartClimb ? Color.green : Color.red);
    }

    private void PerformNormalMovement()
    {
        x = movementInput.x;
        y = movementInput.y;

        Vector3 moveDirection = new Vector3(x, 0, y).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 velocity = moveDirection * speedplayer;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    private void PerformClimbMovement()
    {
        if (!canStartClimb)
        {
            StopClimbing();
            return;
        }

        float verticalClimb = movementInput.y * climbSpeed;
        rb.linearVelocity = new Vector3(0, verticalClimb, 0);
    }

    private void ToggleClimb()
    {
        if (!isClimbing && canStartClimb)
        {
            isClimbing = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
        }
        else if (isClimbing)
        {
            StopClimbing();
        }
    }

    private void StopClimbing()
    {
        isClimbing = false;
        rb.useGravity = true;
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);

        animator.SetFloat("Blend", isClimbing ? 0 : movementInput.magnitude);

        animator.SetBool("Climb", isClimbing);

        if (isGrounded)
        {
            animator.SetBool("Jump", false);
            animator.ResetTrigger("Jump");
        }
    }

    // NUEVA FUNCIÓN: Ejecuta el rebote físico hacia arriba cuando tocas la lava
    public void AplicarImpulsoLava(float fuerza)
    {
        if (rb != null)
        {
            // Reseteamos velocidad vertical actual para un rebote limpio
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * fuerza, ForceMode.Impulse);
        }

        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && !isClimbing)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
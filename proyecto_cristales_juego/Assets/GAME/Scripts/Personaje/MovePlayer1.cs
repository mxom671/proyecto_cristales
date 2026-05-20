using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovePlayer1 : MonoBehaviour
{
    [Header("Movimiento y Rotación")]
    public float speedplayer = 5.0f;
    public float speedRotation = 200f;

    [Header("Físicas de Salto")]
    public float jumpForce = 5.0f;
    public Transform groundCheck; // <-- ¡AQUÍ ESTÁ! Ahora sí aparecerá en el Inspector
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("Mecánica de Escalar")]
    public float climbSpeed = 3.0f;
    public float climbCheckDistance = 0.7f;
    public LayerMask climbableLayer;
    private bool isClimbing = false;
    private bool canStartClimb = false;
    public string climbButton = "Climb"; // Tecla del Input Manager clásico (ej: 'C')

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

        // Congelamos SOLO la rotación física para que el personaje no se caiga como un tronco, 
        // pero permitimos que las posiciones (X, Y, Z) se muevan libremente por código.
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Update()
    {
        // 1. Detectar Suelo y Árboles
        CheckSurroundings();

        // 2. Detectar tecla de escalado (Input Manager clásico)
        if (Input.GetButtonDown(climbButton))
        {
            ToggleClimb();
        }

        // 3. Controlar Animaciones
        UpdateAnimator();
    }

    public void FixedUpdate()
    {
        // El movimiento físico SIEMPRE es mejor hacerlo en FixedUpdate para evitar tirones
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
        // Chequeo de suelo usando el objeto GroundCheck que vas a asignar
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = false;
        }

        // Chequeo de árboles (Rayo hacia adelante desde la mitad del cuerpo)
        Vector3 rayOrigin = transform.position + Vector3.up * 1.0f;
        canStartClimb = Physics.Raycast(rayOrigin, transform.forward, climbCheckDistance, climbableLayer);

        // Dibujar rayos en la vista de Escena para que veas si están tocando algo (Verde = toca, Rojo = no toca)
        Debug.DrawRay(rayOrigin, transform.forward * climbCheckDistance, canStartClimb ? Color.green : Color.red);
    }

    private void PerformNormalMovement()
    {
        x = movementInput.x;
        y = movementInput.y;

        // Rotación por Transform (sigue funcionando igual)
        transform.Rotate(0, x * speedRotation * Time.deltaTime, 0);

        // MOVIMIENTO CORREGIDO: Movemos el Rigidbody hacia adelante/atrás usando su propia dirección
        Vector3 moveDirection = transform.forward * y * speedplayer;

        // Mantenemos la velocidad vertical actual del rigidbody (para que caiga por gravedad si no está en el suelo)
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }

    private void PerformClimbMovement()
    {
        // Si estás escalando y te alejas del árbol, te caes automáticamente
        if (!canStartClimb)
        {
            StopClimbing();
            return;
        }

        // Movimiento vertical usando la W y S (eje Y del movementInput)
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
    }

    // Métodos del NUEVO INPUT SYSTEM
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Solo saltar si se presiona Espacio, está en el suelo y NO está escalando
        if (context.started && isGrounded && !isClimbing)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    // Dibujar la esferita del GroundCheck en el editor para poder verla
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
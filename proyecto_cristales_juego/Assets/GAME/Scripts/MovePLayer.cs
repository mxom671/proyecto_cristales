using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    public float speedplayer = 10.0f;

    private Vector2 movementInput;
    private Animator animator;
    private CharacterController controller;

    public void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        // 1. Calculamos las direcciones relativas al personaje
        // transform.forward es "hacia adelante"
        // transform.right es "hacia la derecha"
        Vector3 moveForward = transform.forward * movementInput.y;
        Vector3 moveSide = transform.right * movementInput.x;

        // 2. Combinamos ambas direcciones en un solo vector de movimiento
        Vector3 direction = moveForward + moveSide;

        // 3. Movemos al personaje
        // SimpleMove ya aplica gravedad automáticamente
        controller.SimpleMove(direction * speedplayer);

        // 4. Animaciones
        if (animator != null)
        {
            // Enviamos los valores al Animator para que sepa si vamos de lado o frente
            animator.SetFloat("VelX", movementInput.x);
            animator.SetFloat("VelY", movementInput.y);
            animator.SetFloat("Blend", movementInput.magnitude);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
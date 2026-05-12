using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    public float speedplayer = 5.0f;
    public float speedRotation = 200f;

    private float x;
    private float y;

    private Vector2 movementInput;
    private Animator animator;
    private CharacterController controller; // NUEVO: Para usar el componente que agregamos

    public void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>(); // Buscamos el componente al empezar
    }

    public void Update()
    {
        x = movementInput.x;
        y = movementInput.y;

        // 1. Rotación (Girar a la muñeca)
        transform.Rotate(0, x * speedRotation * Time.deltaTime, 0);

        // 2. Movimiento con Character Controller (Esto evita que atraviese el suelo)
        // Creamos un vector hacia adelante relativo a donde mira la muñeca
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speedplayer * y;

        // SimpleMove aplica gravedad automáticamente y mueve al personaje
        controller.SimpleMove(forward * curSpeed);

        // 3. Animaciones
        if (animator != null)
        {
            animator.SetFloat("VelX", x);
            animator.SetFloat("VelY", y);
            animator.SetFloat("Blend", movementInput.magnitude);
        }
    }

    // El Player Input llama a esta función automáticamente
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
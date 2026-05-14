using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    public float speedplayer = 10.0f;
    public float fuerzaSalto = 6.0f;
    public float gravedad = -20.0f;
    public float velocidadGiro = 10.0f; // Para controlar qué tan rápido gira el cuerpo

    private Vector2 movementInput;
    private Vector3 velocidadVertical;
    private Animator animator;
    private CharacterController controller;

    public void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        // 1. Detectar si está en el suelo
        bool estaEnElSuelo = controller.isGrounded;

        if (estaEnElSuelo && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }

        // 2. Movimiento Horizontal (Calculado respecto al mundo para que gire bien)
        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y);

        if (direction.magnitude > 0.1f)
        {
            // Mover al personaje
            controller.Move(direction * speedplayer * Time.deltaTime);

            // --- ESTO HACE QUE EL X BOT GIRE EL CUERPO ---
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * velocidadGiro);
        }

        // 3. SALTO MANUAL
        if (Keyboard.current.spaceKey.wasPressedThisFrame && estaEnElSuelo)
        {
            velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);

            // Avisar al Animator para que salte
            if (animator != null) animator.SetTrigger("Salto");
        }

        // 4. Aplicar Gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);

        // 5. Animaciones
        if (animator != null)
        {
            // Enviamos la magnitud (Blend) para Idle/Walk/Run
            animator.SetFloat("Blend", movementInput.magnitude);
            // Enviamos si está en el suelo para salir de la animación de salto
            animator.SetBool("EstaEnElSuelo", estaEnElSuelo);
        }
    }

    public void AplicarImpulsoLava(float fuerza)
    {
        velocidadVertical.y = fuerza;
        // También activamos la animación si toca la lava
        if (animator != null) animator.SetTrigger("Salto");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
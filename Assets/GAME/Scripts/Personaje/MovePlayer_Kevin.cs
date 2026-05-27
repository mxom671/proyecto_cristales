using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer_Kevin : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float speedplayer = 10.0f;
    public float fuerzaSalto = 6.0f;
    public float gravedad = -20.0f;
    public float velocidadGiro = 10.0f;

    [Header("Configuración de Respawn")]
    public float limiteCaida = -15.0f; // Altura a la que el personaje "muere"
    public Vector3 puntoDeInicio;    // Posición de reaparición

    private Vector2 movementInput;
    private Vector3 velocidadVertical;
    private Animator animator;
    private CharacterController controller;

    public void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // Al iniciar, guardamos la posición actual como el punto de inicio
        puntoDeInicio = transform.position;
    }

    public void Update()
    {
        // 1. Detectar si está en el suelo
        bool estaEnElSuelo = controller.isGrounded;

        if (estaEnElSuelo)
        {
            if (velocidadVertical.y < 0)
            {
                velocidadVertical.y = -2f;
            }

            // Limpiamos el Trigger de salto al tocar el suelo para evitar dobles saltos raros
            if (animator != null) animator.ResetTrigger("Salto");
        }

        // 2. SISTEMA DE RESPAWN (Si cae al vacío)
        if (transform.position.y < limiteCaida)
        {
            EjecutarRespawn();
        }

        // 3. Movimiento Horizontal
        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.y);

        if (direction.magnitude > 0.1f)
        {
            controller.Move(direction * speedplayer * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * velocidadGiro);
        }

        // 4. SALTO MANUAL
        if (Keyboard.current.spaceKey.wasPressedThisFrame && estaEnElSuelo)
        {
            velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
            if (animator != null) animator.SetTrigger("Salto");
        }

        // 5. Aplicar Gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);

        // 6. Animaciones
        if (animator != null)
        {
            animator.SetFloat("Blend", movementInput.magnitude);
            animator.SetBool("EstaEnElSuelo", estaEnElSuelo);
        }
    }

    public void EjecutarRespawn()
    {
        // Para teletransportar un CharacterController, debemos apagarlo y prenderlo
        controller.enabled = false;
        transform.position = puntoDeInicio;
        velocidadVertical = Vector3.zero; // Reseteamos la velocidad de caída
        controller.enabled = true;

        Debug.Log("¡X Bot ha vuelto al inicio!");
    }

    public void AplicarImpulsoLava(float fuerza)
    {
        velocidadVertical.y = fuerza;
        if (animator != null) animator.SetTrigger("Salto");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
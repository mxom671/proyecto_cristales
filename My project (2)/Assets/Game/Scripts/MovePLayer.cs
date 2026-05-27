using UnityEngine;
using UnityEngine.InputSystem; // Obligatorio para usar la nueva lectura del sistema

public class MovePlayer : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float speedplayer = 10.0f;
    public float fuerzaSalto = 6.0f;
    public float gravedad = -20.0f;
    public float velocidadGiro = 10.0f;

    [Header("Configuración de Respawn")]
    public float limiteCaida = -15.0f;
    public Vector3 puntoDeInicio;

    [Header("R1: Sistema de Raycast (Quiz)")]
    [Tooltip("Rango de detección entre 3 y 5 unidades")]
    public float raycastRange = 4.0f;
    public LayerMask interactuableLayer; // Capa "Interactuables" para optimizar (R1)
    public Transform raycastOrigin;

    private Vector3 velocidadVertical;
    private Animator animator;
    private CharacterController controller;

    public void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        puntoDeInicio = transform.position;
    }

    public void Update()
    {
        bool estaEnElSuelo = controller.isGrounded;

        if (estaEnElSuelo && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
            if (animator != null) animator.ResetTrigger("Salto");
        }

        // Sistema de Respawn si cae al vacío
        if (transform.position.y < limiteCaida)
        {
            EjecutarRespawn();
        }

        // --- MOVIMIENTO CON NUEVO INPUT SYSTEM (POR HARDWARE DIRECTO) ---
        float moverHorizontal = 0f;
        float moverVertical = 0f;

        // Validamos que el teclado esté conectado y leemos las teclas físicas directamente
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moverVertical = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moverVertical = -1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moverHorizontal = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moverHorizontal = 1f;
        }

        Vector3 direction = new Vector3(moverHorizontal, 0, moverVertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            controller.Move(direction * speedplayer * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * velocidadGiro);
        }

        // --- INTERACCIÓN CON LA TECLA E (NUEVO INPUT SYSTEM) ---
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            EjecutarRaycastInteraccion();
        }

        // --- SALTO CON ESPACIO (NUEVO INPUT SYSTEM) ---
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && estaEnElSuelo)
        {
            velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
            if (animator != null) animator.SetTrigger("Salto");
        }

        // Aplicar Gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);

        // Control de Animaciones
        if (animator != null)
        {
            animator.SetFloat("Blend", direction.magnitude);
            animator.SetBool("EstaEnElSuelo", estaEnElSuelo);
        }
    }

    private void EjecutarRaycastInteraccion()
    {
        Vector3 origen = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 1f;
        Vector3 direccion = transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(origen, direccion, out hit, raycastRange, interactuableLayer))
        {
            Debug.Log($"[Raycast] Golpeó a: {hit.collider.name}");

            ElementController elemento = hit.collider.GetComponent<ElementController>();
            if (elemento != null)
            {
                elemento.Interactuar();
            }
        }
        else
        {
            Debug.Log("[Raycast] Intento fallido: No hay ningún interactuable al frente.");
        }
    }

    public void EjecutarRespawn()
    {
        controller.enabled = false;
        transform.position = puntoDeInicio;
        velocidadVertical = Vector3.zero;
        controller.enabled = true;
        Debug.Log("¡Personaje ha vuelto al inicio!");
    }

    private void OnDrawGizmos()
    {
        Vector3 origen = raycastOrigin != null ? raycastOrigin.position : transform.position + Vector3.up * 1f;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(origen, transform.forward * raycastRange);
    }
}
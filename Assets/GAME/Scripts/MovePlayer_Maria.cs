using UnityEngine;

public class MovePlayer_Maria : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float speedplayer = 10.0f;
    public float fuerzaSalto = 8.0f; // Lo subí a 8 para que tenga más energía al despegar
    public float gravedad = -25.0f;  // Gravedad un poco más pesada para que caiga con gracia
    public float velocidadGiro = 10.0f;

    [Header("Configuración de Respawn")]
    public float limiteCaida = -15.0f;
    public Vector3 puntoDeInicio;

    private Vector3 velocidadVertical;
    private Animator animator;
    private CharacterController controller;

    // 🎥 NUEVO: Guardamos la referencia de la cámara principal
    private Transform camaraPrincipal;

    public void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        puntoDeInicio = transform.position;

        // Buscamos automáticamente la cámara del juego
        if (Camera.main != null)
        {
            camaraPrincipal = Camera.main.transform;
        }
    }

    public void Update()
    {
        bool estaEnElSuelo = controller.isGrounded;

        if (estaEnElSuelo)
        {
            if (velocidadVertical.y < 0)
            {
                velocidadVertical.y = -2f; // Mantiene pegado al personaje al suelo sin acumular gravedad
            }
            if (animator != null) animator.ResetTrigger("Salto");
        }

        if (transform.position.y < limiteCaida)
        {
            EjecutarRespawn();
        }

        // --- SISTEMA DE TECLADO ---
        float moverHorizontal = 0f;
        float moverVertical = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moverVertical = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moverVertical = -1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moverHorizontal = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moverHorizontal = 1f;

        // Dirección cruda del teclado
        Vector3 entradaTeclado = new Vector3(moverHorizontal, 0f, moverVertical);

        // DIRECCIÓN RELATIVA A LA CÁMARA (Para evitar que camine de espaldas)
        Vector3 direccionFinal = Vector3.zero;

        if (entradaTeclado.magnitude > 0.1f && camaraPrincipal != null)
        {
            // Conseguimos las direcciones de la cámara proyectadas en el suelo plano
            Vector3 camaraAdelante = Vector3.Scale(camaraPrincipal.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camaraDerecha = Vector3.Scale(camaraPrincipal.right, new Vector3(1, 0, 1)).normalized;

            // Combinamos las teclas con la orientación de la toma de la cámara
            direccionFinal = (entradaTeclado.z * camaraAdelante + entradaTeclado.x * camaraDerecha).normalized;
        }

        // Enviamos la velocidad real al Animator para las animaciones
        if (animator != null)
        {
            animator.SetFloat("Blend", direccionFinal.magnitude);
        }

        // Ejecutamos el movimiento y la rotación hacia donde mira la cámara
        if (direccionFinal.magnitude > 0.1f)
        {
            controller.Move(direccionFinal * speedplayer * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direccionFinal);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * velocidadGiro);
        }

        // --- SALTO ÚNICO MEJORADO ---
        if (Input.GetKeyDown(KeyCode.Space) && estaEnElSuelo)
        {
            velocidadVertical.y = fuerzaSalto;
            if (animator != null) animator.SetTrigger("Salto");
        }

        // Aplicar Gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;

        // Mover verticalmente (Salto y Gravedad)
        controller.Move(velocidadVertical * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("EstaEnElSuelo", estaEnElSuelo);
        }
    }

    public void EjecutarRespawn()
    {
        controller.enabled = false;
        transform.position = puntoDeInicio;
        velocidadVertical = Vector3.zero;
        controller.enabled = true;
        Debug.Log("¡X Bot ha vuelto al inicio!");
    }

    public void AplicarImpulsoLava(float fuerza)
    {
        velocidadVertical.y = fuerza;
        if (animator != null) animator.SetTrigger("Salto");
    }
}
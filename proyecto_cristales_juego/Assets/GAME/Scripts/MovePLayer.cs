using UnityEngine;

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
        // Evaluamos si está firmemente en el suelo de Unity
        bool estaEnElSuelo = controller.isGrounded;

        if (estaEnElSuelo)
        {
            // Si está en el suelo y cayendo, frenamos la fuerza de gravedad acumulada
            if (velocidadVertical.y < 0)
            {
                velocidadVertical.y = -2f;
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

        Vector3 direction = new Vector3(moverHorizontal, 0f, moverVertical).normalized;

        if (animator != null)
        {
            animator.SetFloat("Blend", direction.magnitude);
        }

        if (direction.magnitude > 0.1f)
        {
            controller.Move(direction * speedplayer * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * velocidadGiro);
        }

        // --- SALTO ÚNICO CALIBRADO ---
        // Solo permite saltar si isGrounded es estrictamente verdadero
        if (Input.GetKeyDown(KeyCode.Space) && estaEnElSuelo)
        {
            velocidadVertical.y = fuerzaSalto;
            if (animator != null) animator.SetTrigger("Salto");
        }

        // Aplicar Gravedad (Siempre va hacia abajo)
        velocidadVertical.y += gravedad * Time.deltaTime;

        // Movemos verticalmente al personaje
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
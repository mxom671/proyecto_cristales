using UnityEngine;

public class AranaController : MonoBehaviour
{
    [Header("Movimiento y Persecucion")]
    public float velocidad = 4f;
    public float rangoDeteccion = 10f;
    public float rangoAtaque = 2f;

    [Header("Cooldown de Ataque")]
    public float tiempoEntreAtaques = 1.5f;
    private float cronometroAtaque;

    private Transform jugador;
    private Animator miAnimator;
    private bool persiguiendo = false;

    void Start()
    {
        miAnimator = GetComponent<Animator>();

        GameObject objetoJugador = GameObject.FindGameObjectWithTag("Player");
        if (objetoJugador != null)
        {
            jugador = objetoJugador.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        cronometroAtaque += Time.deltaTime;
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= rangoDeteccion)
        {
            persiguiendo = true;
        }
        else
        {
            persiguiendo = false;
            if (miAnimator != null) miAnimator.SetBool("isWalking", false);
        }

        if (persiguiendo)
        {
            if (distancia <= rangoAtaque)
            {
                if (miAnimator != null) miAnimator.SetBool("isWalking", false);

                if (cronometroAtaque >= tiempoEntreAtaques)
                {
                    AtacarAlJugador();
                }
            }
            else
            {
                PerseguirAlJugador();
            }
        }
    }

    void PerseguirAlJugador()
    {
        if (miAnimator != null) miAnimator.SetBool("isWalking", true);

        Vector3 direccionTarget = jugador.position - transform.position;
        direccionTarget.y = 0;

        if (direccionTarget != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 5f);
        }

        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
    }

    void AtacarAlJugador()
    {
        cronometroAtaque = 0f;

        if (miAnimator != null)
        {
            miAnimator.SetTrigger("Attack");
        }

        Debug.Log("¡La araña atacó!");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
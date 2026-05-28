using UnityEngine;

public class EnemigoInteligente : MonoBehaviour
{
    [Header("Rangos y Velocidad")]
    public float rangoDeteccion = 10f;
    public float velocidad = 4f;
    public float tiempoMaxPersecucion = 8f;

    [Header("Configuración de Ataque")]
    public int dañoAlJugador = 1;

    private Transform jugador;
    private Vector3 posicionInicial;
    private bool persiguiendo = false;
    private float cronometroPersecucion = 0f;

    private Animator animator;
    private string animacionActual = "";

    void Start()
    {
        posicionInicial = transform.position;

        animator = GetComponentInChildren<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (!persiguiendo)
        {
            if (distanciaAlJugador <= rangoDeteccion)
            {
                persiguiendo = true;
                cronometroPersecucion = tiempoMaxPersecucion;
            }
            else
            {
                if (Vector3.Distance(transform.position, posicionInicial) > 0.5f)
                {
                    RegresarAlOrigen();
                }
                else
                {
                    CambiarAnimacionState("Armature|Rest");
                }
            }
        }
        else
        {
            PerseguirJugador();

            if (distanciaAlJugador > rangoDeteccion)
            {
                cronometroPersecucion -= Time.deltaTime;
                if (cronometroPersecucion <= 0)
                {
                    persiguiendo = false; 
                }
            }
            else
            {
                cronometroPersecucion = tiempoMaxPersecucion;
            }
        }
    }

    void PerseguirJugador()
    {
        Vector3 direccionMirada = new Vector3(jugador.position.x, transform.position.y, jugador.position.z);
        transform.LookAt(direccionMirada);

        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);

        CambiarAnimacionState("Armature|Walk_Cycle_1");
    }

    void RegresarAlOrigen()
    {
        Vector3 direccionMirada = new Vector3(posicionInicial.x, transform.position.y, posicionInicial.z);
        transform.LookAt(direccionMirada);

        transform.position = Vector3.MoveTowards(transform.position, posicionInicial, velocidad * Time.deltaTime);

        CambiarAnimacionState("Armature|Walk_Cycle_1");
    }

    void CambiarAnimacionState(string nuevaAnimacion)
    {
        if (animator == null) return;
        if (animacionActual == nuevaAnimacion) return; 

        animator.Play(nuevaAnimacion);
        animacionActual = nuevaAnimacion;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats stats = collision.gameObject.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.RecibirDaño(dañoAlJugador);

                if (animator != null)
                {
                    animator.SetTrigger("Attack_1");
                }
                Debug.Log("¡El cangrejo te mordió y te quitó una vida!");
            }

            persiguiendo = false;
            cronometroPersecucion = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
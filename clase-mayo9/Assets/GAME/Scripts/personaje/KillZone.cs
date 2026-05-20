using UnityEngine;

public class KillZone : MonoBehaviour
{
    [Header("Configuración de Respawn")]
    public Transform puntoDeRetorno;
    public float distanciaMaximaAlSuelo = 5f; // Altura permitida
    public LayerMask capaSuelo; // La capa del piso

    void Update()
    {
        // 1. Definimos la dirección hacia abajo (0, -1, 0)
        Vector3 direccionAbajo = new Vector3(0, -1, 0);

        // 2. Lanzamos el Raycast
        // Si NO toca el suelo a la distancia marcada, activamos el regreso
        if (!Physics.Raycast(transform.position, direccionAbajo, out RaycastHit hit, distanciaMaximaAlSuelo, capaSuelo))
        {
            EjecutarRespawn();
        }
    }

    void EjecutarRespawn()
    {
        // Detenemos la física para que no aparezca con fuerza de caída
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Movemos al punto seguro
        if (puntoDeRetorno != null)
        {
            transform.position = puntoDeRetorno.position;
        }
        else
        {
            transform.position = new Vector3(0, 5, 0);
        }

        Debug.Log("Raycast detectó vacío: " + gameObject.name + " reubicado.");
    }
}
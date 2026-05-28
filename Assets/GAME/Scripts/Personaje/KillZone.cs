using UnityEngine;

public class KillZone : MonoBehaviour
{
    public int daño = 1;
    public float fuerzaDeRebote = 10f;
    public float fuerzaEmpujeAtras = 8f; // <-- NUEVO: Qué tan fuerte te avienta hacia atrás

    [Header("Configuración de Respawn")]
    public Transform puntoDeSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Quitamos vida
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.RecibirDaño(daño);
            }

            // 2. Buscamos el script de movimiento
            MovePlayer_Maria motor = other.GetComponent<MovePlayer_Maria>();

            // SI ES LAVA: Hace el rebote con empujón hacia atrás
            if (gameObject.CompareTag("Lava"))
            {
                if (motor != null)
                {
                    // Aplicamos el salto vertical que ya tenías
                    motor.AplicarImpulsoLava(fuerzaDeRebote);

                    // 💥 NUEVO: Calculamos la dirección de empujón (dirección contraria a la que mira la muñeca)
                    Vector3 direccionEmpuje = -other.transform.forward;
                    direccionEmpuje.y = 0; // Nos aseguramos de que el empujón sea horizontal
                    direccionEmpuje.Normalize();

                    // Buscamos el CharacterController para moverla a la fuerza hacia atrás
                    CharacterController controller = other.GetComponent<CharacterController>();
                    if (controller != null)
                    {
                        // La mandamos volando hacia atrás un breve instante
                        controller.Move(direccionEmpuje * fuerzaEmpujeAtras);
                    }
                }
                Debug.Log("¡Boing! Rebote ardiente con empujón hacia atrás.");
            }
            // SI ES CAÍDA AL VACÍO: Teletransporte directo al checkpoint
            else
            {
                if (puntoDeSpawn != null)
                {
                    other.transform.position = puntoDeSpawn.position;

                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;
                    }
                }
                Debug.Log("¡Caída libre! Teletransportado al inicio.");
            }
        }
    }
}
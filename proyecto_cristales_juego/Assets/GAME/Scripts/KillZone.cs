using UnityEngine;

public class KillZone : MonoBehaviour
{
    public int daño = 1;
    public float fuerzaDeRebote = 15f;

    [Header("Configuración de Respawn")]
    public Transform puntoDeSpawn; // <-- Arrastra aquí tu CheckPoint_Inicio

    private void OnTriggerEnter(Collider other)
    {
        // Si lo que tocó este detector es el jugador
        if (other.CompareTag("Player"))
        {
            // 1. Buscamos sus estadísticas para quitarle vida
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.RecibirDaño(daño);
            }

            // 2. Buscamos su script de movimiento
            MovePlayer1 motor = other.GetComponent<MovePlayer1>();

            // SI ESTE SCRIPT ESTÁ EN LA LAVA: hace el rebote
            if (gameObject.CompareTag("Lava"))
            {
                if (motor != null)
                {
                    motor.AplicarImpulsoLava(fuerzaDeRebote);
                }
                Debug.Log("¡Boing! Rebote en la lava");
            }
            // SI ESTE SCRIPT ESTÁ EN LA ZONA DE MUERTE (CAÍDA AL VACÍO): reaparece
            else
            {
                if (puntoDeSpawn != null)
                {
                    // Teletransportamos al jugador a la posición del checkpoint
                    other.transform.position = puntoDeSpawn.position;

                    // Si el jugador tiene físicas activas, frenamos su caída para que no aparezca cayendo rápido
                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;
                    }
                }
                Debug.Log("¡Caída libre! Teletransportado al inicio");
            }
        }
    }
}
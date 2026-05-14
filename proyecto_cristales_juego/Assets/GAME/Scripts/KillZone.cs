using UnityEngine;

public class KillZone : MonoBehaviour
{
    public int daño = 1;
    public float fuerzaDeRebote = 15f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava"))
        {
            // Quitar vida
            PlayerStats stats = GetComponent<PlayerStats>();
            if (stats != null) stats.RecibirDaño(daño);

            // Hacer el rebote
            MovePlayer motor = GetComponent<MovePlayer>();
            if (motor != null) motor.AplicarImpulsoLava(fuerzaDeRebote);

            Debug.Log("¡Boing! Rebote en lava");
        }
    }
}
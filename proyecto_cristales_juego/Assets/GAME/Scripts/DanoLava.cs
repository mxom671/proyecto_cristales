using UnityEngine;

public class DanoLava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Buscamos el script de las vidas en la muñeca
        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (stats != null)
        {
            Debug.Log("¡QUEMA! Quitándole vida al jugador...");
            stats.RecibirDaño(1); // Le quita 1 vida
        }
    }
}
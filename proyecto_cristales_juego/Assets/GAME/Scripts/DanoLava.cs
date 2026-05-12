using UnityEngine;
// No necesitamos SceneManagement aquí si el jugador es quien decide cuándo morir
using UnityEngine.SceneManagement; 

public class DañoLava : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public int dañoCausado = 1;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Detectamos si es el jugador el que entró en la lava
        // Comprobamos el Tag o el nombre como tenías en tu código
        if (other.CompareTag("Player") || other.name.Contains("mixamorig"))
        {
            Debug.Log("🔥 ¡El personaje tocó la lava!");

            // 2. Buscamos el script que maneja las vidas en el jugador
            // Nota: El script del jugador debe llamarse 'PlayerStats' (o el nombre que le pongas)
            PlayerStats estadisticas = other.GetComponent<PlayerStats>();

            // Si el script no está en el objeto que colisionó, lo busca en los padres 
            // (Útil si el collider está en un hueso del personaje)
            if (estadisticas == null)
            {
                estadisticas = other.GetComponentInParent<PlayerStats>();
            }

            // 3. Si encontramos el script, le restamos vida
            if (estadisticas != null)
            {
                estadisticas.RecibirDaño(dañoCausado);
            }
        }
    }
}
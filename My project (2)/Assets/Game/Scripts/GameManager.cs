using UnityEngine;
using UnityEngine.SceneManagement; // Requerido para cambiar de escenas

public class GameManager : MonoBehaviour
{
    // Instancia estática para el patrón Singleton (POO Avanzado)
    public static GameManager Instance { get; private set; }

    [Header("Control de Progreso Global (R165)")]
    public int elementosRecolectados = 0;
    public int totalElementosEnEscena = 20; // Meta del examen (R23, R27, R68)

    private void Awake()
    {
        // Implementación estricta del Singleton (R163)
        if (Instance == null)
        {
            Instance = this;
            // Hace que este GameObject persista entre cargas de escenas (R166)
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe otra instancia en una nueva escena, destruimos el duplicado
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Suma puntos al contador global desde cualquier controlador (R165)
    /// </summary>
    public void SumarElemento(int cantidad)
    {
        elementosRecolectados += cantidad;
        Debug.Log($"[GameManager] Progreso global actualizado: {elementosRecolectados}/{totalElementosEnEscena}");
    }

    /// <summary>
    /// Devuelve si el jugador ya completó la recolección
    /// </summary>
    public bool HaGanado()
    {
        return elementosRecolectados >= totalElementosEnEscena;
    }

    // --- CONTROL DE TRANSICIONES (R164) ---
    // (Nota: Se eliminó el [Header] de aquí porque no es válido encima de funciones)

    /// <summary>
    /// Método para el botón de "Iniciar Juego" en el MainMenu (R160)
    /// </summary>
    public void IniciarJuego()
    {
        elementosRecolectados = 0; // Reiniciamos el contador al empezar de nuevo
        SceneManager.LoadScene("Quiz3"); // Asegúrate de nombrar tu escena exactamente así (R167)
    }

    /// <summary>
    /// Método para el botón de "Salir" (R161)
    /// </summary>
    public void SalirDelJuego()
    {
        Debug.Log("[GameManager] Saliendo de la aplicación...");
        Application.Quit(); // Cierra el juego compilado
    }
}
using UnityEngine;

public class ElementController : MonoBehaviour
{
    [Header("Configuración del Elemento (R2)")]
    [Tooltip("Puntos o valor que otorga este elemento al ser recolectado")]
    public int valorPuntos = 1;

    [Header("Efectos Visuales (R3)")]
    [SerializeField] private ParticleSystem particulasActivacion; // Se activan al interactuar (R78)

    [Header("Sistema de Audio Espacial (R4)")]
    [SerializeField] private AudioSource audioSource; // De aquí tomaremos el clip y el volumen

    // Referencia al controlador de la escena para notificar el progreso (R66)
    private SceneController sceneController;
    private bool yaInteractuado = false;

    private void Start()
    {
        // Buscamos el SceneController en la escena para reportar la recolección
        sceneController = Object.FindFirstObjectByType<SceneController>();

        if (sceneController == null)
        {
            Debug.LogWarning($"[ElementController] No se encontró un SceneController en la escena para {gameObject.name}.");
        }
    }

    /// <summary>
    /// Método público invocado por el Raycast del Player al presionar 'E' (R19, R62)
    /// </summary>
    public void Interactuar()
    {
        // Candado lógico para evitar que el jugador recolecte el mismo objeto múltiples veces seguidas
        if (yaInteractuado) return;
        yaInteractuado = true;

        Debug.Log($"[Interacción] {gameObject.name} ha sido activado correctamente.");

        // 1. Disparar efectos visuales (R63, R79)
        if (particulasActivacion != null)
        {
            // Desparentamos las partículas para que no se destruyan/desactiven con el objeto padre
            particulasActivacion.transform.SetParent(null);
            particulasActivacion.Play();

            // Destruimos el objeto de partículas automáticamente después de su duración
            Destroy(particulasActivacion.gameObject, particulasActivacion.main.duration);
        }

        // 2. Reproducir audio espacial posicional (R64, R102, R107) - MÉTODO INFALIBLE
        if (audioSource != null && audioSource.clip != null)
        {
            // Crea un emisor temporal de sonido 3D en la posición del cristal.
            // Se reproduce por hardware y se borra solo al terminar el sonido sin depender del objeto.
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position, audioSource.volume);
        }
        else
        {
            Debug.LogWarning($"[ElementController] {gameObject.name} no pudo sonar porque el AudioSource o su clip están vacíos.");
        }

        // 3. Notificar al SceneController/GameManager para actualizar el Canvas (R66, R144)
        if (sceneController != null)
        {
            sceneController.ElementoRecolectado(valorPuntos);
        }

        // 4. Desactivar el elemento interactivo inmediatamente (R47, R65)
        gameObject.SetActive(false);
    }
}
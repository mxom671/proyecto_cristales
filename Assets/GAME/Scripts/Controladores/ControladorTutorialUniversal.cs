using UnityEngine;
using UnityEngine.UI;

public class ControladorTutorialUniversal : MonoBehaviour
{
    [Header("Panel de Instrucciones de esta Escena")]
    public GameObject panelTutorial;

    [Header("Botón Flotante a Ocultar (Opcional)")]
    public Button botonAQuitar;

    void Start()
    {
        // 1. Congelamos el tiempo apenas inicia la escena actual
        Time.timeScale = 0f;

        // 2. Activamos el panel de instrucciones si lo asignaste
        if (panelTutorial != null)
        {
            panelTutorial.SetActive(true);
        }

        // 3. Activamos el cursor del mouse para poder interactuar con la UI
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Este método lo asignas al botón de "Cerrar" o "¡LISTO!" de cualquier panel
    /// </summary>
    public void EmpezarJuego()
    {
        // 1. Ocultamos el panel de instrucciones
        if (panelTutorial != null)
        {
            panelTutorial.SetActive(false);
        }

        // 2. Ocultamos el botón flotante si es que pusiste uno en esta escena
        if (botonAQuitar != null)
        {
            botonAQuitar.gameObject.SetActive(false);
        }

        // 3. Devolvemos el tiempo a la normalidad para que empiece la acción
        Time.timeScale = 1f;

        Debug.Log("Tutorial de la escena cerrado. ¡Tiempo reanudado!");
    }
}
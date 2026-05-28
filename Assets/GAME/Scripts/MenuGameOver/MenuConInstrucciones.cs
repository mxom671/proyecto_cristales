using UnityEngine;
using UnityEngine.UI; // Obligatorio para poder apagar botones por código

public class TutorialEscenaKevin : MonoBehaviour
{
    [Header("Panel de Instrucciones")]
    public GameObject panelMaderaTutorial; // Aquí arrastras el objeto "Instrucciones1"

    [Header("Botón de la Pantalla a Ocultar")]
    public Button botonTutorialQuit; // Aquí arrastras el botón azul "X"

    void Start()
    {
        // 1. Apenas inicia la escena de Kevin, pausamos todo
        Time.timeScale = 0f;

        // 2. Nos aseguramos de que el cartel de madera se vea
        if (panelMaderaTutorial != null)
        {
            panelMaderaTutorial.SetActive(true);
        }

        // 3. Activamos el mouse para poder hacer clic
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Este es el método que se activa al pulsar "¡LISTO!" o la "X"
    public void EmpezarJuego()
    {
        // 1. Escondemos el cartel de madera
        if (panelMaderaTutorial != null)
        {
            panelMaderaTutorial.SetActive(false);
        }

        // 2. Apagamos el botón flotante para que no estorbe en la pantalla
        if (botonTutorialQuit != null)
        {
            botonTutorialQuit.gameObject.SetActive(false);
        }

        // 3. Devolvemos el tiempo a la normalidad para empezar a jugar
        Time.timeScale = 1f;

        Debug.Log("Tutorial cerrado y botón quitado con éxito.");
    }
}
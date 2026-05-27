using System;
using System.IO;
using UnityEngine;
using TMPro;

public class Scene2Controller : MonoBehaviour
{
    [Header("Componentes UI TextMeshPro")]
    public TextMeshProUGUI txtObjetos;
    public TextMeshProUGUI txtTiempo;
    public TextMeshProUGUI txtEscena;
    public TextMeshProUGUI txtResultado;
    public TextMeshProUGUI txtFinalizado;
    public TextMeshProUGUI txtMensajeCierre;

    private void Start()
    {
        CargarProgresoJSON();
    }

    private void CargarProgresoJSON()
    {
        string rutaArchivo = Path.Combine(Application.streamingAssetsPath, "progreso_jugador.json");

        if (File.Exists(rutaArchivo))
        {
            try
            {
                string jsonText = File.ReadAllText(rutaArchivo);
                ProgresoJugador progreso = JsonUtility.FromJson<ProgresoJugador>(jsonText);

                txtObjetos.text = "Gemas Recolectadas: " + progreso.objetosRecolectados;
                txtTiempo.text = "Tiempo Total: " + progreso.tiempoTotal.ToString("F1") + " s";
                txtEscena.text = "Escena: " + progreso.escenaCompletada;
                txtResultado.text = "Resultado: " + progreso.resultado;
                txtFinalizado.text = "Actividad Finalizada: " + (progreso.actividadFinalizada ? "Si" : "No");

                txtMensajeCierre.text = "¡Mision completada! Descansas en paz... o casi.";
            }
            catch (Exception e)
            {
                Debug.LogError("Error al leer el archivo JSON: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("No se encontro el archivo JSON en: " + rutaArchivo);
        }
    }
}
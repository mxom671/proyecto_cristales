using System;
using System.IO;
using UnityEngine;

[Serializable]
public class ProgresoJugador
{
    public int objetosRecolectados;
    public float tiempoTotal;
    public string escenaCompletada;
    public string resultado;
    public bool actividadFinalizada;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Datos Globales")]
    public int gemasRecolectadas = 0;
    public float tiempoTranscurrido = 0f;
    public bool juegoTerminado = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegistrarRecoleccion()
    {
        if (!juegoTerminado)
        {
            gemasRecolectadas++;
        }
    }

    public void GuardarProgreso()
    {
        juegoTerminado = true;

        ProgresoJugador progreso = new ProgresoJugador
        {
            objetosRecolectados = gemasRecolectadas,
            tiempoTotal = Mathf.Round(tiempoTranscurrido * 10f) / 10f,
            escenaCompletada = "Zona de exploración",
            resultado = "Completado",
            actividadFinalizada = true
        };

        string jsonText = JsonUtility.ToJson(progreso, true);
        string rutaDirectorio = Application.streamingAssetsPath;
        string rutaArchivo = Path.Combine(rutaDirectorio, "progreso_jugador.json");

        try
        {
            if (!Directory.Exists(rutaDirectorio))
            {
                Directory.CreateDirectory(rutaDirectorio);
            }

            File.WriteAllText(rutaArchivo, jsonText);
            Debug.Log("Datos exportados en: " + rutaArchivo);
        }
        catch (Exception e)
        {
            Debug.LogError("Error de guardado: " + e.Message);
        }
    }
}
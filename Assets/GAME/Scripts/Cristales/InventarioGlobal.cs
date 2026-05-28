using UnityEngine;
using System.IO;
using System.Collections.Generic;

// 📄 ESTA ES LA ESTRUCTURA DEL ARCHIVO JSON (Aquí añadimos los datos nuevos sin dañar los cristales)
[System.Serializable]
public class DatosGuardado
{
    public List<string> cristalesGuardados = new List<string>();
    public int vidasActuales = 3;            // 🔴 NUEVO: Guarda cuántas vidas le quedan
    public float tiempoTranscurrido = 0f;    // ⏱️ NUEVO: Guarda los segundos jugados
    public string ultimaEscenaPasada = "";    // 🎬 NUEVO: Guarda el nombre del último nivel completado
}

public class InventarioGlobal : MonoBehaviour
{
    public static InventarioGlobal instancia;

    [Header("Datos en Tiempo Real")]
    public List<string> cristalesGuardados = new List<string>();
    public int vidasActuales = 3;
    public float tiempoActual = 0f;
    public string ultimaEscena = "";

    private string rutaArchivo;

    void Awake()
    {
        // Sistema para que no se destruya al cambiar de escena
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            rutaArchivo = Path.Combine(Application.persistentDataPath, "progreso_juego.json");
            CargarProgreso();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // ⏱️ LÓGICA DEL TIEMPO: Si el juego no está en pausa (Time.timeScale > 0), sumamos los segundos
        if (Time.timeScale > 0f)
        {
            tiempoActual += Time.deltaTime;
        }
    }

    // 💾 FUNCIÓN MODIFICADA: Ahora guarda TODO en el JSON
    public void GuardarProgreso()
    {
        DatosGuardado datos = new DatosGuardado();
        datos.cristalesGuardados = new List<string>(cristalesGuardados);
        datos.vidasActuales = vidasActuales;
        datos.tiempoTranscurrido = tiempoActual;
        datos.ultimaEscenaPasada = ultimaEscena;

        string textoJSON = JsonUtility.ToJson(datos, true);
        File.WriteAllText(rutaArchivo, textoJSON);
        Debug.Log("💾 ¡JSON Actualizado con éxito en!: " + rutaArchivo);
    }

    // 📂 FUNCIÓN MODIFICADA: Al iniciar el juego, recupera todo del JSON si existe
    public void CargarProgreso()
    {
        if (File.Exists(rutaArchivo))
        {
            string textoJSON = File.ReadAllText(rutaArchivo);
            DatosGuardado datos = JsonUtility.FromJson<DatosGuardado>(textoJSON);

            cristalesGuardados = datos.cristalesGuardados;
            vidasActuales = datos.vidasActuales;
            tiempoActual = datos.tiempoTranscurrido;
            ultimaEscena = datos.ultimaEscenaPasada;

            Debug.Log("📂 Datos del JSON cargados correctamente.");
        }
        else
        {
            Debug.Log("✨ No hay archivo de guardado previo. Creando partida limpia.");
            vidasActuales = 3;
            tiempoActual = 0f;
            ultimaEscena = "Ninguna";
            GuardarProgreso();
        }
    }
}
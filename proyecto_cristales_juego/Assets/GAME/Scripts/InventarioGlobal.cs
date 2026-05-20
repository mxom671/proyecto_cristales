using UnityEngine;
using System.Collections.Generic;
using System.IO; // Necesario para manejar archivos

public class InventarioGlobal : MonoBehaviour
{
    public static InventarioGlobal instancia;
    public List<string> cristalesGuardados = new List<string>();

    private string rutaArchivo;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            // Definimos la ruta donde se guardará el JSON
            rutaArchivo = Application.persistentDataPath + "/progreso.json";
            CargarProgreso(); // Intentamos cargar al iniciar
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- REQUISITO: ESCRITURA DE JSON ---
    public void GuardarProgreso()
    {
        // Convertimos la lista a un formato que JSON entienda (una clase contenedora)
        DatosProgreso datos = new DatosProgreso();
        datos.cristales = cristalesGuardados;

        string json = JsonUtility.ToJson(datos, true);
        File.WriteAllText(rutaArchivo, json);

        Debug.Log("Juego Guardado en: " + rutaArchivo);
    }

    // --- REQUISITO: LECTURA DE JSON ---
    public void CargarProgreso()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            DatosProgreso datos = JsonUtility.FromJson<DatosProgreso>(json);

            cristalesGuardados = datos.cristales;
            Debug.Log("Progreso cargado del JSON.");
        }
    }
}

// Clase auxiliar necesaria para que Unity pueda serializar la lista a JSON
[System.Serializable]
public class DatosProgreso
{
    public List<string> cristales;
}
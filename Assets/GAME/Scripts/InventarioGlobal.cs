using UnityEngine;
using System.IO;
using System.Collections.Generic; // Obligatorio para usar List<>

public class InventarioGlobal : MonoBehaviour
{
    public static InventarioGlobal instancia;

    [Header("Datos del Inventario")]
    public List<string> cristalesGuardados = new List<string>();

    private string rutaArchivo;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);

            // 📂 Definimos la ruta apuntando a StreamingAssets
            rutaArchivo = Path.Combine(Application.streamingAssetsPath, "progreso.json");

            // 🛡️ SEGURO DE VIDA: Si la carpeta StreamingAssets no existe, la crea automáticamente
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }

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
        // Convertimos la lista a un formato que JSON entienda (la clase contenedora)
        DatosProgreso datos = new DatosProgreso();
        datos.cristales = cristalesGuardados;

        string json = JsonUtility.ToJson(datos, true);
        File.WriteAllText(rutaArchivo, json);

        Debug.Log("Juego Guardado en StreamingAssets: " + rutaArchivo);

        // Hace que el archivo aparezca inmediatamente en la carpeta de Unity sin tener que reiniciar el programa
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    // --- REQUISITO: LECTURA DE JSON ---
    public void CargarProgreso()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            DatosProgreso datos = JsonUtility.FromJson<DatosProgreso>(json);

            cristalesGuardados = datos.cristales;
            Debug.Log("Progreso cargado con éxito desde StreamingAssets.");
        }
        else
        {
            Debug.LogWarning("No se encontró archivo JSON previo, se iniciará un inventario nuevo.");
        }
    }
}

// Clase auxiliar necesaria para que Unity pueda serializar la lista a JSON
[System.Serializable]
public class DatosProgreso
{
    public List<string> cristales;
}
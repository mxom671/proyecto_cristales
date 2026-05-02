using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LoaderScenes : MonoBehaviour
{
    [Header("Configuración de Interfaz")]
    public GameObject panelInfo; 

    public void NuevaPartida(string nameScene)
    {
        Debug.Log("Inicializando Operación Nexo-Cristal...");
        //ResetearProgresoJSON();

        SceneManager.LoadScene(nameScene);
    }

    public void ContinuarPartida(string nameScene)
    {
        if (File.Exists(Application.persistentDataPath + "/progreso.json"))
        {
            Debug.Log("Cargando datos del Exotraje...");
            SceneManager.LoadScene(nameScene); 
        }
        else
        {
            Debug.LogWarning("No hay archivos de guardado. Iniciando nueva aventura.");
            NuevaPartida(nameScene);
        }
    }

    public void IrAMisiones(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void SalirJuego()
    {
        Debug.Log("Guardando progreso y cerrando aplicación...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    //private void ResetearProgresoJSON()
    //{
        

    //}
}
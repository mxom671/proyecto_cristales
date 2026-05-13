using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorCristales : MonoBehaviour
{
    [Header("Configuración de Inventario")]
    [SerializeField] private List<string> inventarioEscena = new List<string>();
    public TextMeshProUGUI textoCantidad;
    public TextMeshProUGUI textoInventario; // <-- Arrastra aquí el texto de "Inventario"

    [Header("Ajustes de Nivel")]
    public int metaCristales = 5;
    public string nombreSiguienteEscena = "Tiempo-Despegue (DANI)";

    [Header("Efectos de Victoria")]
    public GameObject avisoDespegue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cristal"))
        {
            Recolectar(other.gameObject);
        }
    }

    private void Recolectar(GameObject cristalObj)
    {
        // 1. Guardar local y globalmente
        inventarioEscena.Add(cristalObj.name);

        if (InventarioGlobal.instancia != null)
        {
            InventarioGlobal.instancia.cristalesGuardados.Add(cristalObj.name);
        }

        // 2. Actualizar Interfaz antes de destruir el objeto
        ActualizarInterfaz(cristalObj.name);

        // 3. Destruir cristal
        Destroy(cristalObj);

        // 4. Verificar meta
        if (inventarioEscena.Count >= metaCristales)
        {
            if (avisoDespegue != null)
            {
                avisoDespegue.SetActive(true);
            }
            Invoke("SiguienteEscena", 2.5f);
        }
    }

    void ActualizarInterfaz(string nombreUltimoCristal)
    {
        // Actualiza el contador (ej: Cantidad: 3)
        if (textoCantidad != null)
        {
            textoCantidad.text = "Cantidad: " + inventarioEscena.Count;
        }

        // Actualiza el inventario con el último nombre (ej: Inventario: Cristal Rojo)
        if (textoInventario != null)
        {
            textoInventario.text = "Inventario: " + nombreUltimoCristal;
        }
    }

    private void SiguienteEscena()
    {
        SceneManager.LoadScene(nombreSiguienteEscena);
    }
}
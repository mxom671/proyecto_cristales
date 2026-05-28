using UnityEngine;
using TMPro; // Requerido para usar TextMeshPro

public class ControladorCristales : MonoBehaviour
{
    [Header("Configuración de Inventario")]
    public int cristalesActuales = 0;

    [Header("Ajustes de Nivel")]
    public int metaCristales = 1;

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI textoCantidad;
    public TextMeshProUGUI textoInventario;

    [Header("Efectos de Victoria")]
    public GameObject avisoDespegue; // Por si tienes algún cartel extra de aviso

    void Start()
    {
        ActualizarInterfaz();
        if (avisoDespegue != null)
        {
            avisoDespegue.SetActive(false);
        }
    }

    // Esta función la llamará cada cristal individual cuando la muñeca lo toque
    public void SumarCristal()
    {
        cristalesActuales++;
        ActualizarInterfaz();

        // Guardamos también en el inventario global de JSON si existe en la escena
        if (InventarioGlobal.instancia != null)
        {
            InventarioGlobal.instancia.cristalesGuardados.Add("Cristal_" + cristalesActuales);
            InventarioGlobal.instancia.GuardarProgreso();
        }

        // CONTROL EXCLUSIVO DEL PORTAL: ¿Llegamos a la meta?
        if (cristalesActuales >= metaCristales)
        {
            if (avisoDespegue != null)
            {
                avisoDespegue.SetActive(true);
            }

            // 🌌 CAMBIAZO: Buscamos tu script PortalSalida y le ordenamos que aparezca
            PortalSalida portal = Object.FindFirstObjectByType<PortalSalida>();
            if (portal != null)
            {
                portal.ActivarPortal();
                Debug.Log("¡Felicidades! Meta alcanzada. El PortalSalida ha sido activado por el código.");
            }
            else
            {
                Debug.LogError("🚨 ERROR: No se encontró el script 'PortalSalida' en ningún objeto de la escena. ¡Asegúrate de tenerlo puesto!");
            }
        }
    }

    void ActualizarInterfaz()
    {
        if (textoCantidad != null)
        {
            textoCantidad.text = "CANTIDAD: " + cristalesActuales + " / " + metaCristales;
        }

        if (textoInventario != null)
        {
            textoInventario.text = "INVENTARIO: " + cristalesActuales + " CRISTAL(ES)";
        }
    }
}
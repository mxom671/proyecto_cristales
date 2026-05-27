using UnityEngine;

public class ItemRecogible : MonoBehaviour
{
    [Header("Configuración del Cristal")]
    public string nombreDelItem = "Cristal de Nucleo";

    private void OnTriggerEnter(Collider other)
    {
        // 1. Verificamos que sea la muñeca con su Tag
        if (other.CompareTag("Player"))
        {
            // 2. 🚨 NUEVO: Buscamos el Controlador de Cristales en la escena y le sumamos 1
            // Esto es lo que obligará a los textos de la interfaz a cambiar de número y abrir el portal
            ControladorCristales controlador = Object.FindFirstObjectByType<ControladorCristales>();
            if (controlador != null)
            {
                controlador.SumarCristal();
                Debug.Log("¡Cristal sumado al Controlador de la interfaz!");
            }
            else
            {
                Debug.LogError("🚨 ¡María! No se encontró el script ControladorCristales en la escena. Revisa si está en X Bot o GestorGlobal.");
            }

            // 3. Guardamos en el archivo JSON de Kevin (Si el inventario global existe, lo guarda; si no, no rompe el juego)
            if (InventarioGlobal.instancia != null)
            {
                InventarioGlobal.instancia.cristalesGuardados.Add(nombreDelItem);
                InventarioGlobal.instancia.GuardarProgreso();
                Debug.Log("Cristal guardado en el archivo JSON.");
            }

            // 4. Destruimos el objeto para que desaparezca del mapa
            Destroy(gameObject);
        }
    }
}
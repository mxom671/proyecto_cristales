using UnityEngine;

public class ItemRecogible : MonoBehaviour
{
    [Header("Configuración del Cristal")]
    public string nombreDelItem = "Cristal de Nucleo";

    [Header("Efecto de Sonido")]
    public AudioClip sonidoRecoger; // 🔊 Arrastra aquí tu archivo de audio (.mp3 o .wav)

    private void OnTriggerEnter(Collider other)
    {
        // 1. Verificamos que sea la muñeca con su Tag
        if (other.CompareTag("Player"))
        {
            // 🔊 NUEVO: Si pusiste un sonido, lo reproduce en el lugar exacto antes de destruir el cristal
            if (sonidoRecoger != null)
            {
                AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
            }
            else
            {
                Debug.LogWarning("⚠️ ¡María! Olvidaste poner el archivo de sonido en el Inspector del cristal.");
            }

            // 2. Buscamos el Controlador de Cristales en la escena y le sumamos 1
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

            // 3. Guardamos en el archivo JSON de Kevin
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
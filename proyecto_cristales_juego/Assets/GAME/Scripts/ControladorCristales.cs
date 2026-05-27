using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControladorCristales : MonoBehaviour
{
    [Header("Configuración de Inventario")]
    [SerializeField] private List<string> inventarioEscena = new List<string>();
    public TextMeshProUGUI textoCantidad;
    public TextMeshProUGUI textoInventario;

    [Header("Ajustes de Nivel")]
    public int metaCristales = 5;

    [Header("Referencias del Portal de Salida")]
    public PortalSalida portalDeSalida;

    [Header("Efectos de Victoria")]
    public GameObject avisoDespegue;

    [Header("Configuración de Audio")]
    public AudioClip sonidoRecoleccion;

    // 📋 NUEVO: Lista para guardar TODOS los cristales que estén en nuestro rango
    private List<GameObject> cristalesEnRango = new List<GameObject>();

    private void Update()
    {
        // Si presionamos la E y tenemos al menos un cristal en la lista...
        if (Input.GetKeyDown(KeyCode.E) && cristalesEnRango.Count > 0)
        {
            // Limpiamos la lista por si algún cristal se destruyó antes de tiempo
            cristalesEnRango.RemoveAll(item => item == null);

            if (cristalesEnRango.Count > 0)
            {
                // Buscamos el cristal que esté más cerca físicamente por distancia
                GameObject cristalMasCercano = ObtenerCristalMasCercano();

                if (cristalMasCercano != null)
                {
                    Debug.Log("⌨️ Recogiendo el cristal más cercano: " + cristalMasCercano.name);
                    Recolectar(cristalMasCercano);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cristal"))
        {
            // Si entramos a la zona del cristal, lo sumamos a la lista de espera
            if (!cristalesEnRango.Contains(other.gameObject))
            {
                cristalesEnRango.Add(other.gameObject);
                Debug.Log("🎯 Cristal en rango: " + other.name + " ¡Presiona E!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cristal"))
        {
            // Si nos alejamos, lo sacamos de la lista
            if (cristalesEnRango.Contains(other.gameObject))
            {
                cristalesEnRango.Remove(other.gameObject);
                Debug.Log("🚶‍♀️ Te alejaste del cristal: " + other.name);
            }
        }
    }

    // Función matemática para encontrar el cristal más cercano a la muñeca
    private GameObject ObtenerCristalMasCercano()
    {
        GameObject masCercano = null;
        float distanciaMinima = Mathf.Infinity;
        Vector3 posicionActual = transform.position;

        foreach (GameObject cristal in cristalesEnRango)
        {
            if (cristal != null)
            {
                float distancia = Vector3.Distance(cristal.transform.position, posicionActual);
                if (distancia < distanciaMinima)
                {
                    distanciaMinima = distancia;
                    masCercano = cristal;
                }
            }
        }
        return masCercano;
    }

    private void Recolectar(GameObject cristalObj)
    {
        inventarioEscena.Add(cristalObj.name);

        if (InventarioGlobal.instancia != null)
        {
            InventarioGlobal.instancia.cristalesGuardados.Add(cristalObj.name);
            InventarioGlobal.instancia.GuardarProgreso();
        }

        if (sonidoRecoleccion != null)
        {
            AudioSource.PlayClipAtPoint(sonidoRecoleccion, cristalObj.transform.position);
        }

        if (textoCantidad != null) textoCantidad.text = "Cantidad: " + inventarioEscena.Count;
        if (textoInventario != null) textoInventario.text = "Último: " + cristalObj.name;

        // Lo sacamos de la lista antes de borrarlo del mapa
        cristalesEnRango.Remove(cristalObj);
        Destroy(cristalObj);

        if (inventarioEscena.Count >= metaCristales)
        {
            if (avisoDespegue != null) avisoDespegue.SetActive(true);

            if (portalDeSalida != null)
            {
                portalDeSalida.ActivarPortal();
            }
            else
            {
                PortalSalida portalEncontrado = FindAnyObjectByType<PortalSalida>();
                if (portalEncontrado != null) portalEncontrado.ActivarPortal();
            }
        }
    }
}
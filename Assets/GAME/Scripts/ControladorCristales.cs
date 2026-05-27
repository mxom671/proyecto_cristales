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

    // Lista para guardar los cristales en nuestro rango
    private List<GameObject> cristalesEnRango = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cristalesEnRango.Count > 0)
        {
            // Limpieza rápida de seguridad
            cristalesEnRango.RemoveAll(item => item == null);

            if (cristalesEnRango.Count > 0)
            {
                // Buscamos el cristal más cercano que NO esté en la espalda
                GameObject cristalMasCercano = ObtenerCristalMasCercano();

                if (cristalMasCercano != null)
                {
                    Debug.Log("⌨️ Recogiendo cristal válido: " + cristalMasCercano.name);
                    Recolectar(cristalMasCercano);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cristal"))
        {
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
            if (cristalesEnRango.Contains(other.gameObject))
            {
                cristalesEnRango.Remove(other.gameObject);
                Debug.Log("🚶‍♀️ Te alejaste del cristal: " + other.name);
            }
        }
    }

    // FUNCIÓN MEJORADA: Permite agarrar en 180° frontales/laterales, pero BLOQUEA la espalda atrás
    private GameObject ObtenerCristalMasCercano()
    {
        GameObject masCercano = null;
        float distanciaMinima = Mathf.Infinity;
        Vector3 posicionActual = transform.position;

        foreach (GameObject cristal in cristalesEnRango)
        {
            if (cristal != null)
            {
                // 1. Conseguimos la dirección hacia el cristal
                Vector3 direccionAlCristal = (cristal.transform.position - posicionActual).normalized;

                // 2. Calculamos si está al frente o a los lados usando el frente del jugador (transform.forward)
                // Si da menor o igual a 0, significa que el cristal está físicamente detrás de su espalda.
                float relacionDireccion = Vector3.Dot(transform.forward, direccionAlCristal);

                if (relacionDireccion >= -0.1f) // Al poner -0.1f permitimos los lados y diagonales, pero NADA atrás
                {
                    float distancia = Vector3.Distance(cristal.transform.position, posicionActual);
                    if (distancia < distanciaMinima)
                    {
                        distanciaMinima = distancia;
                        masCercano = cristal;
                    }
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
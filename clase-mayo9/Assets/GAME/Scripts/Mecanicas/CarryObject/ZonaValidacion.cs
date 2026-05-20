using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZonaValidacionFinal : MonoBehaviour
{
    public string ID_Requerido;
    public List<Transform> slotsDisponibles = new List<Transform>();
    private int objetosEntregados = 0;

    [Header("Paneles de Interfaz (UI)")]
    public GameObject panelCorrecto;
    public GameObject panelIncorrecto;
    public GameObject panelNoHayEspacio;
    public TMP_Text textoContador;

    void Start()
    {
        objetosEntregados = 0;
        ActualizarTexto();
        LimpiarPaneles();
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemTransportable item = other.GetComponent<ItemTransportable>();

        if (item != null && item.enabled)
        {
            // 1. PRIORIDAD: ¿Ya está lleno?
            if (objetosEntregados >= slotsDisponibles.Count)
            {
                MostrarPanel(panelNoHayEspacio);
                return; // Bloquea la entrada
            }

            // 2. ¿Es el ID correcto?
            if (item.zonaID.Trim() == ID_Requerido.Trim())
            {
                EntregarObjeto(item);
            }
            else
            {
                MostrarPanel(panelIncorrecto);
            }
        }
    }

    void EntregarObjeto(ItemTransportable item)
    {
        // El objeto se acopla al slot actual
        item.DropInSlot(slotsDisponibles[objetosEntregados]);

        objetosEntregados++;
        ActualizarTexto();
        MostrarPanel(panelCorrecto);

        Debug.Log($"Objeto entregado en {gameObject.name}. Total: {objetosEntregados}");
    }

    void ActualizarTexto()
    {
        if (textoContador != null)
            textoContador.text = "Cantidad: " + objetosEntregados + " / " + slotsDisponibles.Count;
    }

    void MostrarPanel(GameObject panel)
    {
        LimpiarPaneles();
        if (panel != null) panel.SetActive(true);

        CancelInvoke("LimpiarPaneles");
        Invoke("LimpiarPaneles", 2.5f);
    }

    void LimpiarPaneles()
    {
        if (panelCorrecto) panelCorrecto.SetActive(false);
        if (panelIncorrecto) panelIncorrecto.SetActive(false);
        if (panelNoHayEspacio) panelNoHayEspacio.SetActive(false);
    }
}
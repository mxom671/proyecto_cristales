using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZonaColocacion : MonoBehaviour
{
    public string zoneID; // ID que debe coincidir con el del objeto (ej: granja)
    public List<Transform> slots; // Slot 1, 2, 3
    private int contadorObjetos = 0;

    [Header("Paneles de Interfaz")]
    public GameObject panelCorrecto;
    public GameObject panelIncorrecto;
    public GameObject panelNoEspacio; // Se queda con este nombre
    public TMP_Text textoCantidad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActualizarInterfaz();
        OcultarPaneles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemTransportable item = other.GetComponent<ItemTransportable>();

        // Validamos solo cuando el jugador suelta el objeto (isCarried = false)
        if (item != null && !item.isCarried)
        {
            // 1. Verificar si la zona está llena
            if (contadorObjetos >= slots.Count)
            {
                MostrarFeedback(panelNoEspacio);
                return;
            }

            // 2. Verificar si el ID es el correcto
            if (item.zonaID == zoneID)
            {
                // Usa la función de tu script ItemTransportable para posicionarlo
                item.DropInSlot(slots[contadorObjetos]);
                contadorObjetos++;
                ActualizarInterfaz();
                MostrarFeedback(panelCorrecto);
            }
            else
            {
                // Si el objeto no pertenece a esta zona
                MostrarFeedback(panelIncorrecto);
            }
        }
    }

    void ActualizarInterfaz()
    {
        if (textoCantidad != null)
            textoCantidad.text = "Cantidad: " + contadorObjetos;
    }

    void MostrarFeedback(GameObject panelActivo)
    {
        OcultarPaneles();
        if (panelActivo != null)
        {
            panelActivo.SetActive(true);
            CancelInvoke("OcultarPaneles");
            Invoke("OcultarPaneles", 2.5f);
        }
    }

    void OcultarPaneles()
    {
        if (panelCorrecto != null) panelCorrecto.SetActive(false);
        if (panelIncorrecto != null) panelIncorrecto.SetActive(false);
        if (panelNoEspacio != null) panelNoEspacio.SetActive(false);
    }
}

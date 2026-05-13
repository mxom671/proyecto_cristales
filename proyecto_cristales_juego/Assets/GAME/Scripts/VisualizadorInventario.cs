using UnityEngine;
using TMPro;

public class VisualizadorInventario : MonoBehaviour
{
    public TextMeshProUGUI textoTotal;

    void Start()
    {
        // Le preguntamos al inventario global cuántos cristales tiene guardados
        if (InventarioGlobal.instancia != null)
        {
            int total = InventarioGlobal.instancia.cristalesGuardados.Count;
            textoTotal.text = "Cristales Totales: " + total;
            Debug.Log("Se han recuperado " + total + " cristales del inventario global.");
        }
        else
        {
            textoTotal.text = "Error: No se encontró el inventario global.";
        }
    }
}
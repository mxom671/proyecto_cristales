using UnityEngine;
using TMPro;

public class VisualizadorInventario : MonoBehaviour
{
    public TextMeshProUGUI textoTotal;

    void Update() // Cambiamos Start por Update
    {
        if (InventarioGlobal.instancia != null)
        {
            int total = InventarioGlobal.instancia.cristalesGuardados.Count;
            // Mostramos el total y el último item recogido si quieres
            textoTotal.text = "Inventario: " + total;
        }
    }
}
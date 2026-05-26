using UnityEngine;

public class ItemRecogible : MonoBehaviour
{
    // Aquí verás "Cristal de Nucleo" o "Energia" en el Inspector
    public string nombreDelItem;

    private void OnTriggerEnter(Collider other)
    {
        // Dentro de tu función OnTriggerEnter en ItemRecogible.cs
        if (other.CompareTag("Player"))
        {
            if (InventarioGlobal.instancia != null)
            {
                InventarioGlobal.instancia.cristalesGuardados.Add(nombreDelItem);

                // --- NUEVA LÍNEA: GUARDAR EN JSON AL INSTANTE ---
                InventarioGlobal.instancia.GuardarProgreso();

                Destroy(gameObject);
            }
        }
    }
}
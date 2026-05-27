using UnityEngine;
using System.Collections;
using TMPro; // Requisito para poder usar textos de TextMeshPro

public class ContenedorDeposito : MonoBehaviour
{
    private bool yaFueActivado = false;

    [Header("UI de Alerta (Arrastra el aviso aquí)")]
    public TextMeshProUGUI textoAvisoLarge;

    private void OnTriggerEnter(Collider other)
    {
        if (yaFueActivado) return;

        if (InventarioGlobal.instancia != null && InventarioGlobal.instancia.cristalesGuardados.Count > 0)
        {
            yaFueActivado = true;

            InventarioGlobal.instancia.cristalesGuardados.RemoveAt(0);
            InventarioGlobal.instancia.GuardarProgreso();

            if (Scene4Manager.Instance != null)
            {
                Scene4Manager.Instance.RegistrarSlotActivado();
            }

            // 📢 Activamos el letrero gigante en la pantalla
            if (textoAvisoLarge != null)
            {
                StartCoroutine(MostrarAvisoTemporal());
            }

            Debug.Log("¡Cristal entregado y aviso mostrado!");
        }
    }

    // ⏳ Rutina de tiempo para que el cartel desaparezca solo
    private IEnumerator MostrarAvisoTemporal()
    {
        textoAvisoLarge.gameObject.SetActive(true); // Se enciende en toda la pantalla
        yield return new WaitForSeconds(2.0f);      // Espera 2 segundos exactos en pantalla
        textoAvisoLarge.gameObject.SetActive(false); // Se apaga solo
    }
}
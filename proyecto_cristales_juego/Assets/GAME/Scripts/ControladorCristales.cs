using System.Collections.Generic;
using UnityEngine;

public class ControladorCristales : MonoBehaviour
{
    // Usamos una Lista para cumplir con los requisitos del proyecto [cite: 129]
    [SerializeField] private List<string> inventarioCristales = new List<string>();

    private int metaCristales = 5; // Objetivo de la Escena 3 [cite: 108]

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cristal"))
        {
            Recolectar(other.gameObject);
        }
    }

    private void Recolectar(GameObject cristalObj)
    {
        inventarioCristales.Add(cristalObj.name);
        Destroy(cristalObj);

        Debug.Log("Cristales: " + inventarioCristales.Count);

        if (inventarioCristales.Count >= metaCristales)
        {
            Debug.Log("¡Meta alcanzada en las cuevas!");
        }
    }
}
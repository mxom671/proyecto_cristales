using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Estadísticas")]
    public int vidas = 3;

    [Header("Componentes de Interfaz")]
    public TextMeshProUGUI textoVidas;
    public GameObject panelGameOver;

    void Start()
    {
        // 🔴 NUEVO: Si hay vidas guardadas en el JSON, empezamos con esas en vez de restaurar a 3
        if (InventarioGlobal.instancia != null)
        {
            vidas = InventarioGlobal.instancia.vidasActuales;
        }

        ActualizarInterfaz();

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
    }

    public void RecibirDaño(int cantidad)
    {
        if (vidas <= 0) return;

        vidas -= cantidad;

        // 🔴 NUEVO: Le actualizamos el dato al JSON inmediatamente al recibir daño
        if (InventarioGlobal.instancia != null)
        {
            InventarioGlobal.instancia.vidasActuales = vidas;
            InventarioGlobal.instancia.GuardarProgreso(); // Lo escribe en el archivo
        }

        ActualizarInterfaz();

        if (vidas <= 0)
        {
            vidas = 0;
            ActualizarInterfaz();

            Debug.Log("¡GAME OVER!");

            if (panelGameOver != null)
            {
                panelGameOver.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Debug.LogError("🚨 ¡María! Se te olvidó arrastrar el PanelGameOver en el Inspector de X Bot.");
            }
        }
    }

    void ActualizarInterfaz()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidas;
        }
    }
}
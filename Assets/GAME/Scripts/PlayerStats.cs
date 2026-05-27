using UnityEngine;
using TMPro; // Para usar TextMeshPro
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Estadísticas")]
    public int vidas = 3;

    [Header("Componentes de Interfaz")]
    public TextMeshProUGUI textoVidas; // Arrastra aquí el nuevo texto de vidas
    public GameObject panelGameOver;   // <-- NUEVO: Aquí arrastraremos el cartel rojo

    void Start()
    {
        ActualizarInterfaz();

        // Nos aseguramos de que el panel empiece escondido al iniciar
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(false);
        }
    }

    public void RecibirDaño(int cantidad)
    {
        // 🛡️ ESCUDO: Si ya no tienes vidas, ignora por completo cualquier daño extra
        if (vidas <= 0) return;

        vidas -= cantidad;
        ActualizarInterfaz();

        if (vidas <= 0)
        {
            vidas = 0; // Asegura que el marcador de la interfaz se quede clavado en 0
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
using UnityEngine;
using TMPro; // Para usar TextMeshPro
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int vidas = 3;
    public TextMeshProUGUI textoVidas; // Arrastra aquí el nuevo texto de vidas

    void Start()
    {
        ActualizarInterfaz();
    }

    public void RecibirDaño(int cantidad)
    {
        vidas -= cantidad;
        ActualizarInterfaz();

        if (vidas <= 0)
        {
            Debug.Log("¡GAME OVER!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // Aquí podrías reiniciar la escena o mover al personaje al inicio
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
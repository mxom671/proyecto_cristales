using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int vidas = 3;
    public TextMeshProUGUI textoVidas;

    [Header("Menú de Derrota")]
    [Tooltip("Arrastra aquí directamente el objeto PanelGameOver de tu jerarquía")]
    public GameObject objetoPanelGameOver;

    [Header("Audio de Derrota")]
    [Tooltip("Arrastra aquí el archivo de sonido (.mp3 o .wav) de Game Over")]
    public AudioClip sonidoGameOver;

    private AudioSource miAudioSource;
    private bool estaMuerto = false;

    void Start()
    {
        estaMuerto = false;

        // Conseguimos el componente de audio del propio personaje de forma automática
        miAudioSource = GetComponent<AudioSource>();
        if (miAudioSource == null)
        {
            miAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (objetoPanelGameOver != null)
        {
            objetoPanelGameOver.SetActive(false);
        }

        ActualizarInterfaz();
    }

    public void RecibirDaño(int cantidad)
    {
        if (estaMuerto) return;

        vidas -= cantidad;

        if (vidas <= 0)
        {
            vidas = 0;
            estaMuerto = true;
        }

        ActualizarInterfaz();

        if (estaMuerto)
        {
            Debug.Log("💀 ¡Se acabaron las vidas! Forzando el encendido del Panel...");

            // 🎵 Reproducir sonido de Game Over antes de congelar el tiempo del mapa
            if (miAudioSource != null && sonidoGameOver != null)
            {
                miAudioSource.PlayOneShot(sonidoGameOver);
            }

            if (objetoPanelGameOver != null)
            {
                objetoPanelGameOver.SetActive(true);
                Time.timeScale = 0f; // Congela el mapa
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Scene4Manager : MonoBehaviour
{
    // --- TU ESTRUCTURA DE SINGLETON DIRECTA ---
    public static Scene4Manager Instance;

    [Header("Configuración del Tiempo (Escena 4)")]
    [SerializeField] private float tiempoLimite = 90f;
    private float tiempoRestante;
    private bool juegoTerminado = false;

    [Header("Progreso de Depósitos")]
    [SerializeField] private int slotsNecesarios = 5;
    private int slotsActivados = 0;

    [Header("Cinemática de Victoria")]
    [Tooltip("Tiempo en segundos que esperará el juego DESPUÉS de activar la nave para mostrar el Menú de Victoria.")]
    public float tiempoEsperaMenu = 3.5f;

    [Header("Referencias de Interfaz (UI Canvas)")]
    public TextMeshProUGUI textoTiempo;
    public GameObject panelVictoria;
    public GameObject panelDerrota;
    public TextMeshProUGUI textoEstadisticaVictoria;

    [Header("Objetos de la Escena 4")]
    public GameObject naveEscape;
    public GameObject efectoExplosion;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        tiempoRestante = tiempoLimite;

        if (naveEscape != null) naveEscape.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
        if (efectoExplosion != null) efectoExplosion.SetActive(false);
    }

    void Update()
    {
        if (juegoTerminado) return;

        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            ActualizarRelojUI();
        }
        else
        {
            tiempoRestante = 0;
            ActualizarRelojUI();
            SimularDerrota();
        }
    }

    public void RegistrarSlotActivado()
    {
        if (juegoTerminado) return;

        slotsActivados += 1;

        if (slotsActivados >= slotsNecesarios)
        {
            SimularVictoria();
        }
    }

    void ActualizarRelojUI()
    {
        if (textoTiempo != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    void SimularVictoria()
    {
        juegoTerminado = true;

        // 1. Activamos la nave INMEDIATAMENTE para que pase lo que tenga que pasar en el mapa
        if (naveEscape != null)
        {
            naveEscape.SetActive(true);
        }

        Debug.Log("<color=green>Escena 4:</color> ¡Nave activada! Esperando para mostrar el menú...");

        // 2. Retrasamos la aparición del menú usando Invoke para que se vea la nave primero
        Invoke("MostrarMenuVictoriaEfectivo", tiempoEsperaMenu);
    }

    // Esta función se ejecutará después de que pasen los segundos de espera
    void MostrarMenuVictoriaEfectivo()
    {
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }

        if (textoEstadisticaVictoria != null)
        {
            float tiempoEmpleado = Mathf.Round((tiempoLimite - tiempoRestante) * 10f) / 10f;
            textoEstadisticaVictoria.text = $"Misión completada en: <color=yellow>{tiempoEmpleado}s</color>\n" +
                                            $"Tiempo restante: <color=green>{Mathf.Round(tiempoRestante * 10f) / 10f}s</color>";
        }
    }

    void SimularDerrota()
    {
        juegoTerminado = true;

        if (efectoExplosion != null) efectoExplosion.SetActive(true);
        if (panelDerrota != null) panelDerrota.SetActive(true);

        Debug.Log("<color=red>Escena 4:</color> Tiempo agotado. Explosión de la nave.");
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public float TiempoRestante => tiempoRestante;
    public int SlotsActivados => slotsActivados;
    public bool JuegoTerminado => juegoTerminado;
}
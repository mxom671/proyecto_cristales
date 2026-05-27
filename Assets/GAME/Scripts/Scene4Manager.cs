using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Scene4Manager : MonoBehaviour
{
    // --- TU ESTRUCTURA DE SINGLETON DIRECTA ---
    public static Scene4Manager Instance;

    [Header("Configuración del Tiempo (Escena 4)")]
    [SerializeField] private float tiempoLimite = 90f; // Tiempo en segundos antes de la explosión
    private float tiempoRestante;
    private bool juegoTerminado = false;

    [Header("Progreso de Depósitos")]
    [SerializeField] private int slotsNecesarios = 5;
    private int slotsActivados = 0;

    [Header("Referencias de Interfaz (UI Canvas)")]
    public TextMeshProUGUI textoTiempo;     // Texto en Canvas para el cronómetro (reloj)
    public GameObject panelVictoria;        // Panel UI de Escape Exitoso (Victoria)
    public GameObject panelDerrota;         // Panel UI de Misión Fallida (Derrota)

    public TextMeshProUGUI textoEstadisticaVictoria;

    [Header("Objetos de la Escena 4")]
    public GameObject naveEscape;           // Tu objeto de la nave (Federation Corvette F3)
    public GameObject efectoExplosion;      // Sistema de partículas para simular la explosión por tiempo

    private void Awake()
    {
        // Tu estructura de Singleton defensiva local para tu escena
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

        // Configurar los estados iniciales de los objetos en tu escena
        if (naveEscape != null) naveEscape.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
        if (efectoExplosion != null) efectoExplosion.SetActive(false);
    }

    void Update()
    {
        if (juegoTerminado) return;

        // Cuenta regresiva del tiempo crítico
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

    // Este método lo llamará cada contenedor de forma individual al recibir un cristal
    public void RegistrarSlotActivado()
    {
        if (juegoTerminado) return;

        slotsActivados += 1;

        // Si se llenaron los 5 depósitos se activa la victoria
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
        
        if (naveEscape != null) naveEscape.SetActive(true);
        if (panelVictoria != null) panelVictoria.SetActive(true);

        // --- AÑADE ESTA LÓGICA DE TEXTO AQUÍ ---
        if (textoEstadisticaVictoria != null)
        {
            // Calculamos cuánto tiempo le tomó (Tiempo Limite - Tiempo Restante)
            // Usamos Mathf.Round para que quede con un decimal limpio, tal como te gusta programar
            float tiempoEmpleado = Mathf.Round((tiempoLimite - tiempoRestante) * 10f) / 10f;
            
            // Cambia el texto para mostrar la estadística estilizada
            textoEstadisticaVictoria.text = $"Misión completada en: <color=yellow>{tiempoEmpleado}s</color>\n" +
                                            $"Tiempo restante: <color=green>{Mathf.Round(tiempoRestante * 10f) / 10f}s</color>";
        }

        Debug.Log("<color=green>Escena 4:</color> ¡Todos los nexos cargados! Nave lista.");
    }

    void SimularDerrota()
    {
        juegoTerminado = true;

        if (efectoExplosion != null) efectoExplosion.SetActive(true); // La nave falla y explota
        if (panelDerrota != null) panelDerrota.SetActive(true);

        Debug.Log("<color=red>Escena 4:</color> Tiempo agotado. Explosión de la nave.");
    }

    // Función lista para asignársela al botón de "Reiniciar" en tu UI de derrota
    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- TUS PROPIEDADES GETTERS EN FLECHA ---
    public float TiempoRestante => tiempoRestante;
    public int SlotsActivados => slotsActivados;
    public bool JuegoTerminado => juegoTerminado;
}
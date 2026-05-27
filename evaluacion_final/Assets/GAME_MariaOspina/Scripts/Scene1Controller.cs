using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Scene1Controller : MonoBehaviour
{
    [Header("Configuracion de Gemas")]
    public int totalGemasEnEscena = 15;

    [Header("Componentes UI")]
    public TextMeshProUGUI textoContador;
    public TextMeshProUGUI textoInteraccion;

    [Header("Desglose del Cronometro")]
    public TextMeshProUGUI txtMinutos;
    public TextMeshProUGUI txtSegundos;
    public TextMeshProUGUI txtMilisegundos;

    [Header("Configuracion Raycast")]
    public Transform puntoDisparoRaycast;
    public float distanciaRaycast = 3.5f;
    public LayerMask capaGemas;

    [Header("Audio")]
    public AudioSource fuenteAudio;
    public AudioClip sonidoRecoleccion;

    [Header("Efectos de Particulas")]
    public GameObject prefabParticulasGema;

    [Header("Control de Caida (Respawn)")]
    public Transform jugador;
    public float umbralCaidaY = -10f;

    private Vector3 posicionInicialJugador;
    private bool cambiandoDeEscena = false;

    private void Start()
    {
        if (jugador != null)
        {
            posicionInicialJugador = jugador.position;
        }

        textoInteraccion.gameObject.SetActive(false);
        ActualizarInterfaz();
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.juegoTerminado) return;

        CalcularTiempo();
        ControlarCaida();
        ProcesarRaycast();
    }

    private void CalcularTiempo()
    {
        GameManager.Instance.tiempoTranscurrido += Time.deltaTime;
        float tiempoGlobal = GameManager.Instance.tiempoTranscurrido;

        int minutos = Mathf.FloorToInt(tiempoGlobal / 60f);
        int segundos = Mathf.FloorToInt(tiempoGlobal % 60f);
        int milisegundos = Mathf.FloorToInt((tiempoGlobal % 1f) * 10f);

        txtMinutos.text = minutos.ToString("00");
        txtSegundos.text = segundos.ToString("00");
        txtMilisegundos.text = milisegundos.ToString("0");
    }

    private void ControlarCaida()
    {
        if (jugador != null && jugador.position.y < umbralCaidaY)
        {
            CharacterController cc = jugador.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            jugador.position = posicionInicialJugador;

            if (cc != null) cc.enabled = true;
        }
    }

    private void ProcesarRaycast()
    {
        RaycastHit hit;
        Vector3 direccion = puntoDisparoRaycast != null ? puntoDisparoRaycast.forward : Camera.main.transform.forward;
        Vector3 origen = puntoDisparoRaycast != null ? puntoDisparoRaycast.position : Camera.main.transform.position;

        if (Physics.Raycast(origen, direccion, out hit, distanciaRaycast, capaGemas))
        {
            textoInteraccion.gameObject.SetActive(true);
            textoInteraccion.text = "Presiona E para recoger";

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                RecogerGema(hit.collider.gameObject);
            }
        }
        else
        {
            textoInteraccion.gameObject.SetActive(false);
        }
    }

    private void RecogerGema(GameObject gema)
    {
        if (prefabParticulasGema != null)
        {
            GameObject particulas = Instantiate(prefabParticulasGema, gema.transform.position, Quaternion.identity);
            Destroy(particulas, 2f);
        }

        GameManager.Instance.RegistrarRecoleccion();
        ActualizarInterfaz();

        if (fuenteAudio != null && sonidoRecoleccion != null)
        {
            fuenteAudio.PlayOneShot(sonidoRecoleccion);
        }

        Destroy(gema);

        if (GameManager.Instance.gemasRecolectadas >= totalGemasEnEscena && !cambiandoDeEscena)
        {
            StartCoroutine(SecuenciaFinalizacion());
        }
    }

    private void ActualizarInterfaz()
    {
        int actuales = GameManager.Instance != null ? GameManager.Instance.gemasRecolectadas : 0;
        textoContador.text = "Gemas: " + actuales + "/" + totalGemasEnEscena;
    }

    private IEnumerator SecuenciaFinalizacion()
    {
        cambiandoDeEscena = true;

        GameManager.Instance.GuardarProgreso();

        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene("Scene2");
    }
}
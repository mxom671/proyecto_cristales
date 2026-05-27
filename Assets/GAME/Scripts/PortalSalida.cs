using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalSalida : MonoBehaviour
{
    [Header("Configuracion de Destino")]
    public string nombreEscenaSiguiente = "Nivel2";

    [Header("Referencias Visuales y Efectos")]
    public GameObject[] objetosVisuales;
    public ParticleSystem efectosParticulas;
    public Collider portalCollider;

    [Header("Audios de Apertura (Se ejecutan al abrirse)")]
    public AudioSource fuenteAudioApertura;
    public AudioClip sonidoListo;
    public AudioClip sonidoActivacion;

    [Header("Audios de Transicion (Al entrar al portal)")]
    public AudioClip sonidoWhoosh;
    public AudioClip sonidoEpico;

    private bool yaSeUso = false;
    private bool portalAbierto = false;

    void Start()
    {
        if (portalCollider == null)
            portalCollider = GetComponent<Collider>();

        OcultarPortal();
    }

    public void ActivarPortal()
    {
        if (portalAbierto) return;
        portalAbierto = true;

        MostrarPortal();

        // Sonidos de apertura
        if (fuenteAudioApertura != null)
        {
            if (sonidoListo != null) fuenteAudioApertura.PlayOneShot(sonidoListo);
            if (sonidoActivacion != null) fuenteAudioApertura.PlayOneShot(sonidoActivacion);
        }
        else
        {
            if (sonidoListo != null) AudioSource.PlayClipAtPoint(sonidoListo, transform.position);
        }

        Debug.Log("🔒 ¡El Portal de Salida se ha ABIERTO oficialmente!");
    }

    void OcultarPortal()
    {
        foreach (GameObject visual in objetosVisuales)
            if (visual != null) visual.SetActive(false);

        if (efectosParticulas != null) efectosParticulas.Stop();
        if (portalCollider != null) portalCollider.isTrigger = true;
    }

    void MostrarPortal()
    {
        foreach (GameObject visual in objetosVisuales)
            if (visual != null) visual.SetActive(true);

        if (efectosParticulas != null) efectosParticulas.Play();
        if (portalCollider != null) portalCollider.isTrigger = true;
    }

    // Detecta cuando la muñeca entra al portal
    private void OnTriggerEnter(Collider other)
    {
        if (yaSeUso || !portalAbierto) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("🌌 ¡X Bot entró al portal abierto! Iniciando viaje...");
            yaSeUso = true;
            StartCoroutine(SecuenciaTeletransporte());
        }
    }

    IEnumerator SecuenciaTeletransporte()
    {
        // Reproducir sonidos flotantes que no se cortan al cambiar de escena
        if (sonidoWhoosh != null) AudioSource.PlayClipAtPoint(sonidoWhoosh, transform.position);
        if (sonidoEpico != null) AudioSource.PlayClipAtPoint(sonidoEpico, transform.position);

        yield return new WaitForSeconds(2f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscenaSiguiente);
    }
}
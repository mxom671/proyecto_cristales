using UnityEngine;

public class KillZone : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [Tooltip("Arrastra aquí el archivo de sonido (.mp3 o l .wav) para cuando se queme")]
    public AudioClip sonidoQuemadura;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                // 1. 🎵 Sonido de quemadura
                AudioSource audioJugador = other.GetComponent<AudioSource>();
                if (audioJugador != null && sonidoQuemadura != null)
                {
                    audioJugador.PlayOneShot(sonidoQuemadura);
                }

                // 2. ❤️ Restar vida
                stats.RecibirDaño(1);

                // 3. 🏃‍♀️ ¡RETROCESO MÁS LARGO! (Solo si sigue viva)
                if (stats.vidas > 0)
                {
                    CharacterController controller = other.GetComponent<CharacterController>();
                    if (controller != null) controller.enabled = false;

                    Vector3 posicionSegura = other.transform.position;

                    // --- AQUÍ MODIFICAMOS EL EMPUJE ---
                    posicionSegura.y += 3.5f; // La levanta más alto (antes era 2.0f)
                    posicionSegura -= other.transform.forward * 4.0f; // La echa bastante más atrás (antes era 1.5f)
                    // ----------------------------------

                    other.transform.position = posicionSegura;

                    if (controller != null) controller.enabled = true;

                    Debug.Log("🔥 ¡Empujón largo aplicado! Jugador fuera de la lava.");
                }
            }
        }
    }
}
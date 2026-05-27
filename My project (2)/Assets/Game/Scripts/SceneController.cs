using UnityEngine;
using UnityEngine.UI; // Requerido para manipular el componente Image de la barra
using TMPro; // Requerido para TextMesh Pro

public class SceneController : MonoBehaviour
{
    [Header("Componentes de la Interfaz (R6)")]
    [SerializeField] private TextMeshProUGUI textoContador; // Arrastra aquí tu 'Texto_Contador' (R154)
    [SerializeField] private Image barraProgresoRelleno;   // Arrastra aquí tu 'Relleno_Barra' (R139)

    private void Start()
    {
        ActualizarInterfazHUD();
    }

    /// <summary>
    /// Maneja la lógica local cuando un objeto interactuable es recolectado (R176, R177)
    /// </summary>
    public void ElementoRecolectado(int puntos)
    {
        if (GameManager.Instance != null)
        {
            // 1. Comunicar con el GameManager Global para sumar los puntos (R178)
            GameManager.Instance.SumarElemento(puntos);

            // 2. Actualizar la interfaz gráfica (Texto y Barra) en tiempo real (R136, R177)
            ActualizarInterfazHUD();

            // 3. Revisar condición de Victoria (R179)
            if (GameManager.Instance.HaGanado())
            {
                EjecutarVictoria();
            }
        }
    }

    private void ActualizarInterfazHUD()
    {
        if (GameManager.Instance == null) return;

        // Formato obligatorio: "Elementos: X/20" (R138)
        if (textoContador != null)
        {
            textoContador.text = $"Elementos: {GameManager.Instance.elementosRecolectados}/{GameManager.Instance.totalElementosEnEscena}";
        }

        // Control de la barra visual de 0.0 a 1.0 (R139)
        if (barraProgresoRelleno != null)
        {
            float porcentaje = (float)GameManager.Instance.elementosRecolectados / GameManager.Instance.totalElementosEnEscena;
            barraProgresoRelleno.fillAmount = porcentaje; // Ajusta el relleno visual en tiempo real (R136)
        }
    }

    private void EjecutarVictoria()
    {
        Debug.Log("[SceneController] ¡Victoria! Has recolectado todos los elementos. (R179)");
        // Aquí puedes cargar una pantalla de créditos, activar un texto de "¡Ganaste!" o regresar al menú.
    }
}
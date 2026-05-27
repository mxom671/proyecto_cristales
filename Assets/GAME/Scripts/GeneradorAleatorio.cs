using UnityEngine;

public class GeneradorAleatorio : MonoBehaviour
{
    [Header("Rango de Variación de Posición")]
    public float rangoX = 3.0f; // Qué tanto puede variar a los lados
    public float rangoZ = 3.0f; // Qué tanto puede variar adelante/atrás

    void Start()
    {
        // 📋 PASO 1: Le damos un nombre único al azar para que el ControladorCristales no se confunda
        // Esto transforma "Cristal" en algo como "Cristal_482"
        gameObject.name = gameObject.name + "_" + Random.Range(100, 999);

        // 🎲 PASO 2: Generamos un desfase aleatorio dentro del rango
        float desvioX = Random.Range(-rangoX, rangoX);
        float desvioZ = Random.Range(-rangoZ, rangoZ);

        // 🚀 PASO 3: Movemos el cristal de su posición original usando el desfase
        transform.position = new Vector3(transform.position.x + desvioX, transform.position.y, transform.position.z + desvioZ);

        Debug.Log(gameObject.name + " reposicionado aleatoriamente y renombrado en Start.");
    }
}
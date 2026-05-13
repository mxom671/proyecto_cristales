using UnityEngine;
using UnityEngine.SceneManagement;

public class DañoLava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Solo si lo que entra es el Jugador (X Bot)
        if (other.CompareTag("Player") || other.name.Contains("mixamorig"))
        {
            Debug.Log("¡Caíste en la lava de verdad!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int vidas = 3;

    // EL ARREGLO ESTÁ AQUÍ: Agregamos "int cantidad"
    public void RecibirDaño(int cantidad)
    {
        vidas -= cantidad; // Resta el daño recibido a las vidas totales
        Debug.Log("¡Auch! Vidas restantes: " + vidas);

        if (vidas <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("Game Over");
        // Reinicia la escena como pide el PDF del proyecto
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Descongelamos el tiempo antes de reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f; // ¡MUY IMPORTANTE!: Descongelamos el tiempo antes de viajar de escena

        // 🚨 REEMPLAZA "MenuPrincipal" por el nombre exacto de tu escena de menú entre comillas
        SceneManager.LoadScene("Menu");
    }
}
using UnityEngine;

public class EfectoAgua : MonoBehaviour
{
    [Header("Altura del Agua")]
    [Tooltip("Pon aquí la posición en Y de tu plano de agua")]
    public float alturaLimiteAgua = 0f;

    private Transform jugador;
    private bool estaEnElAgua = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return;

        if (jugador.position.y < alturaLimiteAgua)
        {
            if (!estaEnElAgua)
            {
                estaEnElAgua = true;
                Debug.Log("¡El personaje se ha metido al agua y se está hundiendo!");

            }
        }
        else
        {
            if (estaEnElAgua)
            {
                estaEnElAgua = false;
                Debug.Log("El personaje salió del agua.");
            }
        }
    }
}
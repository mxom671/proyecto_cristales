using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Configuracion del Spawn")]
    public GameObject prefabCangrejo;
    public Transform[] puntosSpawn; // Aquí arrastra los 18 objetos vacíos
    public int cantidadCangrejos = 13;

    void Start()
    {
        SpawnearCangrejosAleatorios();
    }

    void SpawnearCangrejosAleatorios()
    {
        if (puntosSpawn.Length < cantidadCangrejos)
        {
            Debug.LogError("No hay suficientes puntos de spawn para la cantidad de cangrejos solicitada.");
            return;
        }

        // Creamos una lista copia de los puntos para poder manipularla
        List<Transform> listaPuntos = new List<Transform>(puntosSpawn);

        for (int i = 0; i < cantidadCangrejos; i++)
        {
            // Elegimos un índice al azar de la lista restante
            int indiceAleatorio = Random.Range(0, listaPuntos.Count);
            Transform puntoElegido = listaPuntos[indiceAleatorio];

            // Instanciamos el cangrejo en esa posición y rotación
            Instantiate(prefabCangrejo, puntoElegido.position, puntoElegido.rotation);

            // Eliminamos el punto usado de la lista para que no se repita
            listaPuntos.RemoveAt(indiceAleatorio);
        }
    }
}
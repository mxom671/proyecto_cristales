using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemigo : MonoBehaviour
{
    [Header("Configuración del Enemigo")]
    public GameObject prefabEnemigo; 

    [Header("Puntos de Spawn (Los 20 vacíos)")]
    public List<Transform> puntosDeSpawn = new List<Transform>();

    [Header("Cantidad a Spawnear")]
    [Range(1, 20)]
    public int cantidadDeEnemigos = 10; 

    void Start()
    {
        if (prefabEnemigo == null)
        {
            Debug.LogError("¡Falta asignar el Prefab del Enemigo en el SpawnManager!");
            return;
        }

        if (puntosDeSpawn.Count == 0)
        {
            Debug.LogError("¡No has asignado ningún punto de spawn en la lista!");
            return;
        }

        SpawnearEnemigosAleatorios();
    }

    void SpawnearEnemigosAleatorios()
    {

        List<Transform> puntosDisponibles = new List<Transform>(puntosDeSpawn);

        int totalASpawnear = Mathf.Min(cantidadDeEnemigos, puntosDisponibles.Count);

        for (int i = 0; i < totalASpawnear; i++)
        {
            int indiceAleatorio = Random.Range(0, puntosDisponibles.Count);
            Transform puntoElegido = puntosDisponibles[indiceAleatorio];

            Instantiate(prefabEnemigo, puntoElegido.position, puntoElegido.rotation);

            puntosDisponibles.RemoveAt(indiceAleatorio);
        }

        Debug.Log($"¡Se han distribuido {totalASpawnear} cangrejos de forma aleatoria por la playa!");
    }
}
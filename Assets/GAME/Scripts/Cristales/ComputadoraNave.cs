using UnityEngine;
using System.Collections.Generic; // REQUERIDO: Para que Unity entienda Pilas y Colas

public class ComputadoraNave : MonoBehaviour
{
    // 1. LA COLA: Controla las tareas técnicas en orden (Primero en entrar, Primero en salir)
    private Queue<string> tareasDeReparacion = new Queue<string>();

    // 2. LA PILA: Controla los mensajes de radio (Último en llegar, Primero en leerse)
    private Stack<string> historialRadio = new Stack<string>();

    void Start()
    {
        // Añadimos tareas a la COLA de reparación
        tareasDeReparacion.Enqueue("1. Limpiar los residuos térmicos del motor principal.");
        tareasDeReparacion.Enqueue("2. Insertar los Cristales Subterráneos reactivos.");
        tareasDeReparacion.Enqueue("3. Calibrar la potencia para el despegue.");

        // Añadimos mensajes a la PILA de la radio (el último es el más reciente)
        historialRadio.Push("[Radio - Hace 10 min]: Nave estrellada en la Isla.");
        historialRadio.Push("[Radio - Hace 5 min]: Detectada alta radiación en la cueva.");
        historialRadio.Push("[Radio - ¡AHORA!]: ¡Alerta! Los niveles de lava están subiendo.");

        // Mostramos los sistemas en la Consola para que el profesor vea que funciona
        MostrarSistemasEnConsola();
    }

    void MostrarSistemasEnConsola()
    {
        Debug.Log("==== SISTEMA OPERATIVO DE LA NAVE INICIADO ====");

        // Peek() sirve para mirar qué hay al frente de la cola sin borrarlo
        if (tareasDeReparacion.Count > 0)
        {
            Debug.Log("Próxima tarea técnica en la COLA: " + tareasDeReparacion.Peek());
        }

        // Peek() en la pila nos muestra el último mensaje recibido (el de arriba de la torre)
        if (historialRadio.Count > 0)
        {
            Debug.Log("Mensaje urgente en el tope de la PILA: " + historialRadio.Peek());
        }

        Debug.Log("==============================================");
    }
}
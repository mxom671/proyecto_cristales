using UnityEngine;
using UnityEngine.InputSystem;

public class ObtenerObjeto : MonoBehaviour
{
    public Transform carryPoint;
    private ItemTransportable objetoEnMano;
    public float distanciaInteraccion = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Detecta el clic con el nuevo Input System
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (objetoEnMano == null)
            {
                LanzarRayoParaRecoger();
            }
            else
            {
                SoltarObjeto();
            }
        }
    }

    void LanzarRayoParaRecoger()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, distanciaInteraccion))
        {
            ItemTransportable item = hit.collider.GetComponent<ItemTransportable>();
            if (item != null)
            {
                objetoEnMano = item;
                // Usa la función original sin cambios
                objetoEnMano.PickUp(carryPoint);
            }
        }
    }

    void SoltarObjeto()
    {
        if (objetoEnMano != null)
        {
            // Usa la función original sin cambios
            objetoEnMano.Release();
            objetoEnMano = null;
        }
    }
}

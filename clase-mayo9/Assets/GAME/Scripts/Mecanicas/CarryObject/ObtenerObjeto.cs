using UnityEngine;
using UnityEngine.InputSystem;

public class ObtenerObjeto : MonoBehaviour
{
    public Transform carryPoint;
    private ItemTransportable objetoEnMano;
    public float distanciaInteraccion = 4f;
    public LayerMask capaInteractiva;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (objetoEnMano == null) LanzarRayo();
            else Soltar();
        }
    }

    void LanzarRayo()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, distanciaInteraccion, capaInteractiva))
        {
            ItemTransportable item = hit.collider.GetComponent<ItemTransportable>();
            if (item != null)
            {
                objetoEnMano = item;
                objetoEnMano.PickUp(carryPoint); // Llama a la nueva función con col.enabled = false
            }
        }
    }

    void Soltar()
    {
        if (objetoEnMano != null)
        {
            objetoEnMano.Release(); // Llama a la nueva función con col.enabled = true
            objetoEnMano = null;
        }
    }
}
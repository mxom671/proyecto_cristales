using Unity.Mathematics;
using UnityEngine;

public class ItemTransportable : MonoBehaviour
{
    public string zonaID; //nombre de zona correcta

    public bool isCarried = false;
    private Rigidbody rb;
    private Collider colliderObj;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliderObj = GetComponent<Collider>();
    }


    public void PickUp(Transform carryPoint)
    {
        isCarried = true;
        if (rb != null)
        {
            rb.isKinematic = true; // Desactiva la física para que el objeto no caiga
            rb.linearVelocity = Vector3.zero; // Detiene cualquier movimiento residual
            rb.angularVelocity = Vector3.zero;
        }
        transform.SetParent(carryPoint); // Hace que el objeto sea hijo del punto de transporte
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void DropInSlot(Transform slot)
    {
        isCarried = false;
        transform.SetParent(null); // Desvincula el objeto del punto de transporte
        transform.position = slot.position; // Coloca el objeto en la posición del slot
        transform.rotation = slot.rotation; // Alinea la rotación con el slot
        transform.localScale = slot.lossyScale;
        if (rb != null)
        {
            rb.isKinematic = false; // Reactiva la física para que el objeto pueda caer
        }
    }


    public void Release()
    {
        isCarried = false;
        transform.SetParent(null); // Desvincula el objeto del punto de transporte
        if (rb != null)
        {
            rb.isKinematic = false; // Reactiva la física para que el objeto pueda caer
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

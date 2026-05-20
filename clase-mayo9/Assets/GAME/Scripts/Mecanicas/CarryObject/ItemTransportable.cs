using UnityEngine;

public class ItemTransportable : MonoBehaviour
{
    public string zonaID;
    public bool isCarried = false;
    private Rigidbody rb;
    private Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform carryPoint)
    {
        isCarried = true;
        if (col != null) col.enabled = false; // Evita colisiones con Erika

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Release()
    {
        isCarried = false;
        transform.SetParent(null);
        if (col != null) col.enabled = true; // Reactiva físicas

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public void DropInSlot(Transform slot)
    {
        isCarried = false;

        // 1. Desactivamos físicas y colisiones por completo
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        if (col != null) col.enabled = false;

        // 2. IMPORTANTE: Asignamos el padre ANTES de corregir la escala
        transform.SetParent(slot);

        // 3. LA SOLUCIÓN MAESTRA: Usamos lossyScale para forzar la escala global a 1
        // Esto ignora la escala deformada del Slot padre.
        transform.position = slot.position; // Se mueve al centro del slot
        transform.rotation = slot.rotation; // Copia la rotación del slot

        // Calculamos la escala local necesaria para que en el mundo se vea de tamaño 1,1,1
        Vector3 parentScale = slot.lossyScale;
        transform.localScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f / parentScale.z);

        // Opcional: Si aun así se ve rara de tamaño, prueba esta línea más agresiva:
        // transform.localScale = Vector3.one; // (Si el slot tiene escala deformada, esto fallará, por eso usamos la línea de arriba)

        this.enabled = false; // Desactivamos el script del objeto
    }
}
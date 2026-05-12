using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObjects : MonoBehaviour
{
    private Vector3 mOffset;
    private float coorZ;
    private Rigidbody rb;
    private bool isDragging;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
            {
                isDragging = true;

                if (rb && rb.useGravity)
                    rb.useGravity = false;

                coorZ = Camera.main.WorldToScreenPoint(transform.position).z;
                mOffset = transform.position - GetMouseWorldPosition();
            }
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            transform.position = GetMouseWorldPosition() - mOffset;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;

            if (rb && !rb.useGravity)
                rb.useGravity = true;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseP = Mouse.current.position.ReadValue();
        mouseP.z = coorZ;
        return Camera.main.ScreenToWorldPoint(mouseP);
    }
}

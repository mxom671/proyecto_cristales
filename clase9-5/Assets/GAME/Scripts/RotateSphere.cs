using UnityEngine;

public class RotateSphere : MonoBehaviour
{
    public float speedRotate = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, speedRotate* Time.deltaTime, 0f);
    }
}

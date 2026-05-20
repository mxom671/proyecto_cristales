using UnityEngine;
using System.Collections.Generic;

public class InventarioGlobal : MonoBehaviour
{
    public static InventarioGlobal instancia;
    public List<string> cristalesGuardados = new List<string>();

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
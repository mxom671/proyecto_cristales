using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ControllerDrag : MonoBehaviour
{
    public List<GameObject> objsInstaciados;
    public Transform positionInicial;
    GameObject objInstanciado;
    bool t = true;

    public TMP_Text textoContador;
    private int contadorObjetos = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(IniciarCorrutina());
        ActualizarTexto(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IniciarCorrutina()
    {
        while (t)
        {
            yield return new WaitForSeconds(3);
            objInstanciado = InstanciarObjs();

        }

    }

    public GameObject InstanciarObjs()
    {
        int n = Random.Range(0, objsInstaciados.Count);
        GameObject obj = Instantiate(objsInstaciados[n],
            positionInicial.position, objsInstaciados[n].transform.rotation);

        return obj;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Draggable"))
        {
            contadorObjetos++;
            ActualizarTexto();
            Debug.Log("Objeto arrastrado dentro del trigger");
            Debug.Log("Objeto entró. Total: " + contadorObjetos);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Draggable"))
        {
            contadorObjetos--;
            ActualizarTexto();
            Debug.Log("Objeto arrastrado fuera del trigger");
            Debug.Log("Objeto salió. Total: " + contadorObjetos);
        }

    }

    void ActualizarTexto()
    {
        if (textoContador != null)
        {
            textoContador.text = "Cantidad: " + contadorObjetos;
        }
    }
}

using UnityEngine;

public class CristalNucleo : MonoBehaviour
{
    // Usamos esta función que es más potente para Character Controllers
    private void OnTriggerEnter(Collider other)
    {
        // No importa cómo se llame, si tiene el componente que mueve al astronauta, se destruye
        if (other.GetComponent<CharacterController>() != null || other.CompareTag("Player"))
        {
            Debug.Log("¡LO TOCASTE! Desapareciendo cristal...");
            Destroy(gameObject);
        }
    }
}
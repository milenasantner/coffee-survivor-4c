using UnityEngine;

public class PCZoneTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spieler hat PC-Zone betreten");
            Debug.Log("TODO: Hier macht PERSON 3 weiter - Minigame aktivieren");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spieler hat PC-Zone verlassen");
            Debug.Log("TODO: Hier macht PERSON 3 weiter - Minigame deaktivieren");
        }
    }
}
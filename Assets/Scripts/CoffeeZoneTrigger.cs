using UnityEngine;

public class CoffeeZoneTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spieler hat Kaffee-Zone betreten");
            Debug.Log("TODO: Hier macht PERSON 2 weiter - Kaffee auffüllen");
        }
    }
}
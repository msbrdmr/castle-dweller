using Unity.VisualScripting;
using UnityEngine;

public class SightZoneCollisionDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is in my sight zone!");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collision");

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left my sight zone.");
        }
    }
}

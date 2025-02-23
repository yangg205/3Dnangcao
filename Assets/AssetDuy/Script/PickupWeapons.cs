using UnityEngine;

public class PickupWeapons : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player gần vũ khí!");
            GameManager.instance.SetNearbyWeapon(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.ClearNearbyWeapon(gameObject);
        }
    }
}

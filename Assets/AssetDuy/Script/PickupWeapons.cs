using UnityEngine;

public class PickupWeapons : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player gần vật phẩm!");
            GameManager.instance.SetNearbyItem(gameObject); // Đổi từ SetNearbyWeapon thành SetNearbyItem
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.ClearNearbyItem(gameObject); // Đổi từ ClearNearbyWeapon thành ClearNearbyItem
        }
    }
}

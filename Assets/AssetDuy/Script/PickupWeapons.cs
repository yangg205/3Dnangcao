using UnityEngine;

public class PickupWeapons : MonoBehaviour
{
    public int weaponIndex; // Vũ khí tương ứng với vị trí trong mảng weapons của GameManager

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.UnlockWeapon(weaponIndex);
            Destroy(gameObject); // Xóa vũ khí khỏi cảnh sau khi nhặt
        }
    }
}

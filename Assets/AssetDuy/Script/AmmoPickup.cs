using TMPro;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30; // Số đạn nhặt được
    public TextMeshProUGUI pickupText; // UI hiển thị "Nhấn E để nhặt đạn"

    private bool isPlayerInRange = false;
    private GameObject player; // Lưu Player khi vào vùng nhặt

    void Start()
    {
        if (pickupText != null)
            pickupText.gameObject.SetActive(false); // Ẩn text khi bắt đầu
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.gameObject; // Lưu tham chiếu tới Player

            if (pickupText != null)
            {
                pickupText.text = "Nhấn [E] để nhặt đạn";
                pickupText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null; // Xóa tham chiếu khi rời vùng nhặt

            if (pickupText != null)
                pickupText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupAmmo();
        }
    }

    private void PickupAmmo()
    {
        if (player != null)
        {
            ShootingController shooter = player.GetComponentInChildren<ShootingController>();
            if (shooter != null)
            {
                shooter.PickupAmmo(ammoAmount);
                Destroy(gameObject);
                if (pickupText != null)
                    pickupText.gameObject.SetActive(false);
            }
        }
    }
}

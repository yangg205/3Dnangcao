using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image weaponIcon;
    public Sprite[] weaponIcons; // Mảng chứa icon của từng vũ khí
    private Dictionary<WeaponType, int> equippedWeaponIndices = new Dictionary<WeaponType, int>();
    public static GameManager instance;
    public int highScore;
    public int currentScore;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currentScoreText;
    public GameObject[] weapons;
    private bool[] unlockedWeapons;

    [SerializeField] private int currentWeaponIndex = 0;
    public TextMeshProUGUI pickupText;
    private GameObject nearbyWeapon;

    public WeaponType[] weaponTypes; // Danh mục từng vũ khí
    private int healthPackCount = 0; // Số lượng vật phẩm hồi máu
    public TextMeshProUGUI healthPackText; // UI hiển thị số lượng

    void Start()
    {
        instance = this;
        unlockedWeapons = new bool[weapons.Length];

        // Tắt tất cả vũ khí lúc bắt đầu
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Tìm và đặt vũ khí cận chiến làm mặc định
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weaponTypes[i] == WeaponType.Melee)
            {
                currentWeaponIndex = i;
                unlockedWeapons[i] = true;
                break;
            }
        }

        // Bật vũ khí cận chiến
        weapons[currentWeaponIndex].SetActive(true);

        pickupText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
        highScoreText.text = highScore.ToString();
        currentScoreText.text = currentScore.ToString();

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeaponByType(WeaponType.Rifle);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeaponByType(WeaponType.Pistol);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeaponByType(WeaponType.Melee);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchWeaponByType(WeaponType.Medkit); // Chuyển sang Medkit

        if (Input.GetKeyDown(KeyCode.E) && nearbyWeapon != null)
        {
            PickUpItem(nearbyWeapon);
        }
    }
    public enum WeaponType
    {
        Rifle,   // Súng trường
        Pistol,  // Súng lục
        Melee,   // Cận chiến
        Medkit   // Túi cứu thương
    }

    void SwitchWeaponByType(WeaponType type)
    {
        if (!equippedWeaponIndices.ContainsKey(type))
        {
            Debug.Log($"Bạn chưa có vũ khí thuộc loại {type}!");
            return;
        }

        int newWeaponIndex = equippedWeaponIndices[type];

        if (newWeaponIndex != currentWeaponIndex)
        {
            SwitchWeapon(newWeaponIndex);
        }
    }

    void SwitchWeapon(int newIndex)
    {
        weaponIcon.sprite = weaponIcons[newIndex]; // Cập nhật icon vũ khí/Medkit
        if (unlockedWeapons[newIndex] && newIndex != currentWeaponIndex)
        {
            weapons[currentWeaponIndex].SetActive(false);
            weapons[newIndex].SetActive(true);
            currentWeaponIndex = newIndex;

            // Cập nhật icon vũ khí
            weaponIcon.sprite = weaponIcons[newIndex];
        }
    }

    public void UnlockWeapon(int index)
    {
        if (index >= 0 && index < weapons.Length)
        {
            unlockedWeapons[index] = true;
        }
    }

    void PickUpWeapon(GameObject weapon)
    {
        int weaponIndex = -1;

        // Tìm vũ khí có cùng tên trong danh sách weapons[]
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].name == weapon.name.Replace("(Clone)", "").Trim())
            {
                weaponIndex = i;
                break;
            }
        }

        if (weaponIndex == -1)
        {
            Debug.LogError($"Vũ khí {weapon.name} không tồn tại trong danh sách weapons[].");
            return;
        }

        WeaponType weaponType = weaponTypes[weaponIndex];

        // Nếu đã có vũ khí thuộc loại này, thay thế vũ khí cũ
        if (equippedWeaponIndices.ContainsKey(weaponType))
        {
            int oldWeaponIndex = equippedWeaponIndices[weaponType];
            weapons[oldWeaponIndex].SetActive(false); // Ẩn vũ khí cũ
        }

        // Cập nhật vũ khí mới vào đúng vị trí phím số (1,2,3)
        equippedWeaponIndices[weaponType] = weaponIndex;
        unlockedWeapons[weaponIndex] = true;

        // Nếu đang cầm vũ khí của loại này -> đổi ngay sang vũ khí mới
        if (weaponType == weaponTypes[currentWeaponIndex])
        {
            SwitchWeapon(weaponIndex);
            weaponIcon.sprite = weaponIcons[weaponIndex]; // Cập nhật icon
        }

        Debug.Log($"Nhặt {weapon.name}, cập nhật thay thế vũ khí thuộc loại {weaponType}!");
        Destroy(weapon);
        nearbyWeapon = null;
        pickupText.gameObject.SetActive(false);
    }
    void PickUpItem(GameObject item)
    {
        string itemName = item.name.Replace("(Clone)", "").Trim();

        if (itemName == "Medkit")
        {
            PickUpHealthPack(); // Gọi đúng chức năng để tăng số lượng Medkit
            Destroy(item);
            pickupText.gameObject.SetActive(false);
            return;
        }

        PickUpWeapon(item);
    }
    public void PickUpMedkit(GameObject medkit)
    {
        int medkitIndex = -1;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weaponTypes[i] == WeaponType.Medkit)
            {
                medkitIndex = i;
                break;
            }
        }

        if (medkitIndex == -1)
        {
            Debug.LogError("Không tìm thấy Medkit trong danh sách weapons[].");
            return;
        }

        equippedWeaponIndices[WeaponType.Medkit] = medkitIndex;
        unlockedWeapons[medkitIndex] = true;

        // Chuyển sang cầm Medkit
        SwitchWeapon(medkitIndex);

        Debug.Log("Nhặt Medkit!");
        Destroy(medkit);
    }
    void UseMedkit()
    {
        if (healthPackCount > 0 && PlayerMovement.instance.currentHealth < PlayerMovement.instance.maxHealth)
        {
            PlayerMovement.instance.Heal(20);
            healthPackCount--;

            if (healthPackCount == 0)
            {
                Debug.Log("Hết Medkit! Tự động chuyển về vũ khí cận chiến.");
                SwitchWeaponByType(WeaponType.Melee);
            }

            UpdateHealthPackUI();
        }
        else
        {
            Debug.Log("Không thể sử dụng Medkit!");
        }
    }

    public void SetNearbyItem(GameObject item)
    {
        nearbyWeapon = item;
        string itemName = item.name.Replace("(Clone)", "").Trim();

        if (itemName == "Medkit")
        {
            pickupText.text = $"Nhấn [E] để nhặt vật phẩm hồi máu";
        }
        else
        {
            pickupText.text = $"Nhấn [E] để nhặt {itemName}";
        }

        pickupText.gameObject.SetActive(true);
    }

    public void ClearNearbyItem(GameObject item)
    {
        if (nearbyWeapon == item)
        {
            nearbyWeapon = null;
            pickupText.gameObject.SetActive(false);
        }
    }
    void UpdateHealthPackUI()
    {
        healthPackText.text = $"{healthPackCount}";
    }
    public void PickUpHealthPack()
    {
        healthPackCount++;
        UpdateHealthPackUI();
        pickupText.gameObject.SetActive(false); // Ẩn UI sau khi nhặt
        Debug.Log("Nhặt vật phẩm hồi máu!");
    }
    public void UseHealthPack()
    {
        if (healthPackCount > 0 && PlayerMovement.instance.currentHealth < PlayerMovement.instance.maxHealth)
        {
            int healAmount = Mathf.Min(20, PlayerMovement.instance.maxHealth - PlayerMovement.instance.currentHealth);
            PlayerMovement.instance.Heal(healAmount);
            healthPackCount--;
            UpdateHealthPackUI();
            Debug.Log($"Đã sử dụng vật phẩm hồi máu! Hồi {healAmount} máu.");
        }
        else
        {
            Debug.Log("Không thể sử dụng vật phẩm hồi máu!");
        }
    }
}
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

        // Chuyển đổi vũ khí theo nhóm
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeaponByType(WeaponType.Rifle);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeaponByType(WeaponType.Pistol);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchWeaponByType(WeaponType.Melee);

        // Nếu có vũ khí gần đó và nhấn E -> Nhặt
        if (nearbyWeapon != null && Input.GetKeyDown(KeyCode.E))
        {
            PickUpWeapon(nearbyWeapon);
        }
    }
    public enum WeaponType
    {
        Rifle,   // Súng trường
        Pistol,  // Súng lục
        Melee    // Cận chiến
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

    public void SetNearbyWeapon(GameObject weapon)
    {
        nearbyWeapon = weapon;
        string weaponName = weapon.name.Replace("(Clone)", "").Trim();
        pickupText.text = $"Nhấn [E] để nhặt {weaponName}";
        pickupText.gameObject.SetActive(true);
    }

    public void ClearNearbyWeapon(GameObject weapon)
    {
        if (nearbyWeapon == weapon)
        {
            nearbyWeapon = null;
            pickupText.gameObject.SetActive(false);
        }
    }
}
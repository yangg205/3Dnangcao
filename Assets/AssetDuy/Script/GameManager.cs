using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int highScore;
    public int currentScore;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currentScoreText;
    public GameObject[] weapons;
    private bool[] unlockedWeapons;

    [SerializeField] private int currentWeaponIndex = 0;
    void Start()
    {
        instance = this;
        unlockedWeapons = new bool[weapons.Length];

        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        unlockedWeapons[0] = true;
        weapons[0].SetActive(true);
        currentWeaponIndex = 0;
    }
    void Update()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
        highScoreText.text = highScore.ToString();
        currentScoreText.text = currentScore.ToString();
        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchWeapon(i);
            }
        }
    }
    void SwitchWeapon(int newIndex)
    {
        if (unlockedWeapons[newIndex] && newIndex != currentWeaponIndex)
        {
            // Ẩn vũ khí hiện tại
            weapons[currentWeaponIndex].SetActive(false);

            // Bật vũ khí mới
            weapons[newIndex].SetActive(true);

            // Cập nhật vũ khí hiện tại
            currentWeaponIndex = newIndex;
        }
    }
    public void UnlockWeapon(int index)
    {
        if (index >= 0 && index < weapons.Length)
        {
            unlockedWeapons[index] = true; // Đánh dấu vũ khí đã nhặt
        }
    }
}

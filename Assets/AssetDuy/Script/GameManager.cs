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

    [SerializeField] private int currentWeaponIndex = 0;
    void Start()
    {
        instance = this;
        SwitchWeapon(currentWeaponIndex);
    }
    void Update()
    {
        if(currentScore > highScore)
        {
            highScore = currentScore;
        }
        highScoreText.text = highScore.ToString();
        currentScoreText.text = currentScore.ToString();
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchWeapon(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchWeapon(5);
        }
    }
    void SwitchWeapon(int newIndex)
    {
        weapons[currentWeaponIndex].SetActive(false);
        weapons[newIndex].SetActive(true);
        currentWeaponIndex = newIndex;  
    }    
}

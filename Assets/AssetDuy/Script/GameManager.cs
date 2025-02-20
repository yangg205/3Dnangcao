using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] weapons;

    [SerializeField] private int currentWeaponIndex = 0;
    void Start()
    {
        SwitchWeapon(currentWeaponIndex);
    }
    void Update()
    {
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

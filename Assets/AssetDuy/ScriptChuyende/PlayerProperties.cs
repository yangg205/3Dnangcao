using Fusion;
using TMPro;
using UnityEngine;

public class PlayerProperties : NetworkBehaviour
{
    //thuoc tinh health se dong bo hoa qua mang
    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public float health { get; set; }
    public float maxHealth { get; set; }

    public TextMeshProUGUI healthText;

    //khi mau thay doi thi cap nhat lai UI
    public void OnHealthChanged()
    {
        healthText.text = $"{health}/{maxHealth}";
    }

    [Networked, OnChangedRender(nameof(OnSpeedChanged))]
    public float speed { get; set; }
    public Animator anim;
    public int speedHash = Animator.StringToHash("Speed");

    public void OnSpeedChanged()
    {
        anim.SetFloat(speedHash, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            health -= 10;
        }
    }

    private void Start()
    {
        maxHealth = 100;
        health = maxHealth;
        healthText.text = $"{health}/{maxHealth}";
    }
}

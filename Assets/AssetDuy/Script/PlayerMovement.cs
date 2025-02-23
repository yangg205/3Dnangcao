using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //Player Health
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    public DeathScreen deathScreen;
    
    //Slider Health
    //DeathScreen
    //Player Movement
    public float movementSpeed = 5f;
    private CharacterController controller;

    //Player Jump
    public float jumpForce = 2f;

    //Gravity
    public float gravity = -9.81f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    private bool isGrounded;
    private Vector3 velocity;
    public static PlayerMovement instance; // Thêm biến instance
    private void Awake()
    {
        instance = this; // Đảm bảo instance được khởi tạo khi game chạy
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.minValue = 1;
        healthSlider.maxValue = currentHealth;
        healthSlider.value = currentHealth;
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        HandleMovement();
        HandleGravity();

        //Handle Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce - 2 * gravity);
        }
        controller.Move(velocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Alpha4))
    {
        GameManager.instance.UseHealthPack();
    }
    }
    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        movement.y = 0;
        controller.Move(movement * movementSpeed * Time.deltaTime);
    }
    void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        healthSlider.value -= damageAmount;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    private void Die()
    {
        deathScreen.showDeadScreen = true;
        Debug.Log("You die");
    }
    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        healthSlider.value = currentHealth;
        Debug.Log($"Hồi {healAmount} máu! Máu hiện tại: {currentHealth}/{maxHealth}");
    }
}

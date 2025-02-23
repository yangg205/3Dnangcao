using UnityEngine;
using UnityEngine.Windows;
public class PlayerController : MonoBehaviour
{
    //Playermovenet & Gravity
    public float movementSpeed = 5f;
    public float jumpForce = 2f;
    private CharacterController controller;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    private bool isGrounded;
    private Vector3 velocity;

    //Footsteps
    public AudioSource leftFootAudioSource;
    public AudioSource rightFootAudioSource;
    public AudioClip[] footStepSounds;
    public float footStepInterval = 0.5f;
    private float nextFootStepTime;
    private bool isLeftFootStep = true;
    private object healthPotionAmount;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        HandleMovement();
        HandleGravity();

        //Handle FootSteps
        if(isGrounded && controller.velocity.magnitude > 0.1f && Time.time >= nextFootStepTime)
        {
            PlayerFootStepSound();
            nextFootStepTime = Time.time + footStepInterval;
        }

        controller.Move(velocity * Time.deltaTime);

        //Handle Jump
        if(UnityEngine.Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }
    }
    void HandleMovement()
    {
        float horizontalInput = UnityEngine.Input.GetAxis("Horizontal");
        float verticalInput = UnityEngine.Input.GetAxis("Vertical");

        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;   

        controller.Move(movement * movementSpeed * Time.deltaTime);
    }
    void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }
    void PlayerFootStepSound()
    {
        AudioClip footStepClip = footStepSounds[Random.Range(0, footStepSounds.Length)];

        if(isLeftFootStep)
        {
            leftFootAudioSource.PlayOneShot(footStepClip);
        }
        else
        {
            rightFootAudioSource.PlayOneShot(footStepClip);
        }
        isLeftFootStep = !isLeftFootStep;
    }


public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;  // Máu của nhân vật
    public float maxHealth = 100f;  // Máu tối đa
    public float healthPotionAmount = 20f;  // Số máu hồi khi nhặt thuốc

    // Cập nhật máu khi nhân vật va chạm với vật phẩm
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPotion"))  // Kiểm tra xem vật phẩm có phải là thuốc máu không
        {
            Heal(healthPotionAmount);  // Tăng máu
            Destroy(other.gameObject);  // Xóa vật phẩm khỏi hiện trường
        }
    }

    // Phương thức để hồi máu
    void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)  // Nếu máu vượt quá mức tối đa, giới hạn nó ở mức tối đa
        {
            health = maxHealth;
        }
        Debug.Log("Current Health: " + health);
    }
}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.name);  // Kiểm tra xem vật phẩm có va chạm với nhân vật không
        if (other.CompareTag("HealthPotion"))
        {
            Heal(healthPotionAmount);
            Destroy(other.gameObject);
        }
    }

    private void Heal(object healthPotionAmount)
    {
        throw new System.NotImplementedException();
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
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
    void Start()
    {
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
}

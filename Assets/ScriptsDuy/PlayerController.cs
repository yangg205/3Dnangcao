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
}

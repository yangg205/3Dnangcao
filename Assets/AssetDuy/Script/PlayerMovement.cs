using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour
{
    //Player Health
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public TextMeshProUGUI point;
    int _point;
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
        ////Debug.Log(GameManager.instance.currentScore);
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //if (isGrounded && velocity.y < 0)
        //{
        //    velocity.y = -2f;
        //}
        //HandleMovement();
        //HandleGravity();

        ////Handle Jump
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    velocity.y = Mathf.Sqrt(jumpForce - 2 * gravity);
        //}
        //controller.Move(velocity * Time.deltaTime);
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    GameManager.instance.UseHealthPack();
        //}
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            //Debug.Log(GameManager.instance.currentScore);
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            HandleMovement();
            HandleGravity();

            //Handle Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce - 2 * gravity);
            }
            controller.Move(velocity * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                GameManager.instance.UseHealthPack();
            }
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
        Debug.Log("You died");

        // Gửi điểm lên server khi chết
        string email = PlayerPrefs.GetString("email"); // Lấy email đã lưu
        int totalPoint = GameManager.instance.currentScore; // Điểm hiện tại của người chơi
        Debug.Log($"Điểm gửi lên server: {totalPoint}");
        StartCoroutine(UpdatePointToServer(email, totalPoint));
        // Quay lại Scene "Map 1" sau khi gửi điểm (đợi một chút để đảm bảo gửi xong)
        StartCoroutine(LoadSceneWithDelay("Map 1", 3f));
    }

    IEnumerator UpdatePointToServer(string email, int totalPoint)
    {
        string apiUrl = "http://yang2206-001-site1.ptempurl.com/api/updatepoint";
        string json = $"{{\"email\":\"{email}\",\"total_point\":{totalPoint}}}";

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            string username = "11212993";
            string password = "60-dayfreetrial";
            string encodedAuth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));

            request.SetRequestHeader("Authorization", $"Basic {encodedAuth}");
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"Sending POST request to {apiUrl} with body: {json}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error updating points: {request.error}");
                Debug.LogError($"Response from server: {request.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"Server response: {request.downloadHandler.text}");
            }
        }
    }
    IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset trạng thái chuột và load Scene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        healthSlider.value = currentHealth;
        Debug.Log($"Hồi {healAmount} máu! Máu hiện tại: {currentHealth}/{maxHealth}");
    }

}

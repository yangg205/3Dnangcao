using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;   // Tốc độ di chuyển
    public float jumpForce = 7f; // Lực nhảy
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Lấy Rigidbody của nhân vật
    }

    void Update()
    {
        // Lấy đầu vào từ bàn phím
        float moveX = Input.GetAxis("Horizontal"); // A/D hoặc phím mũi tên trái/phải
        float moveZ = Input.GetAxis("Vertical");   // W/S hoặc phím mũi tên lên/xuống

        // Tạo vector di chuyển
        Vector3 movement = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;

        // Di chuyển nhân vật
        transform.Translate(movement, Space.Self);

        // Nhảy khi nhấn phím Space
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Đặt lại trạng thái nhảy
        }
    }

    // Kiểm tra va chạm với mặt đất để bật lại nhảy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

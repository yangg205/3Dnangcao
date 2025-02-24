using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển
    public float rotationSpeed = 720f; // Tốc độ xoay (chỉ cho 3D)

    void Update()
    {
        // Nhận input từ bàn phím
        float horizontal = Input.GetAxis("Horizontal"); // A/D hoặc phím mũi tên Trái/Phải
        float vertical = Input.GetAxis("Vertical"); // W/S hoặc phím mũi tên Lên/Xuống

        // Vector di chuyển
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (movement.magnitude > 1)
        {
            movement.Normalize(); // Giữ tốc độ nhất quán khi di chuyển chéo
        }

        // Di chuyển nhân vật
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Xoay nhân vật theo hướng di chuyển (nếu trong không gian 3D)
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}

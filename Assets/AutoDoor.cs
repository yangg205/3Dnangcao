using System.Collections;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public float openSpeed = 2.0f;      // Tốc độ mở cửa
    public float openDistance = 3.0f;   // Khoảng cách cửa mở ra từ trong ra ngoài
    public float openTime = 3.0f;       // Thời gian cửa mở
    public float closeTime = 3.0f;      // Thời gian cửa đóng lại
    private Vector3 closedPosition;     // Vị trí cửa khi đóng
    private Vector3 openPosition;       // Vị trí cửa khi mở
    private bool isOpen = false;        // Trạng thái của cửa (mở hay đóng)
    private bool hasOpened = false;     // Kiểm tra cửa đã mở hay chưa
    private Collider doorCollider;      // Collider của cửa

    private void Start()
    {
        // Ghi lại vị trí đóng ban đầu của cửa
        closedPosition = transform.position;

        // Tính toán vị trí mở cửa
        openPosition = transform.position + transform.up * openDistance;

        // Lấy Collider của cửa
        doorCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        // Kiểm tra trạng thái cửa và di chuyển cửa đến vị trí mở hoặc đóng
        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, openSpeed * Time.deltaTime);
        }
    }

    // Hàm mở cửa
    public void OpenDoor()
    {
        isOpen = true;
    }

    // Hàm đóng cửa
    public void CloseDoor()
    {
        isOpen = false;
    }

    // OnTriggerEnter sẽ được gọi khi người chơi đi qua vùng trigger
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng có tag "Player" vào vùng trigger và cửa chưa mở
        if (!hasOpened && other.CompareTag("Player"))
        {
            OpenDoor();  // Mở cửa
            hasOpened = true;  // Đánh dấu là cửa đã mở và không mở lại

            // Tắt Is Trigger sau khi người chơi đi qua
            doorCollider.isTrigger = false;

            // Bắt đầu quá trình tự động đóng cửa
            StartCoroutine(AutoCloseDoor());
        }
    }

    // Coroutine tự động đóng cửa sau một khoảng thời gian
    private IEnumerator AutoCloseDoor()
    {
        // Đợi cửa mở trong khoảng thời gian
        yield return new WaitForSeconds(openTime);

        // Đóng cửa lại
        CloseDoor();

        // Đợi trong thời gian cửa đóng
        yield return new WaitForSeconds(closeTime);

        // Sau khi cửa đóng lại, không mở lại nữa
        // Cửa đã đóng, không làm gì thêm để mở lại.
    }
}

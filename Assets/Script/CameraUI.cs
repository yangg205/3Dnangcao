using UnityEngine;

public class CameraAutoMove : MonoBehaviour
{
    public Transform[] targetPositions; // Các điểm đích mà camera sẽ di chuyển đến
    public float moveSpeed = 2f;        // Tốc độ di chuyển của camera

    private Vector3 startPosition;      // Vị trí ban đầu của camera
    private Vector3 currentTarget;      // Mục tiêu hiện tại của camera
    private float journeyLength;        // Khoảng cách giữa vị trí ban đầu và đích
    private float startTime;            // Thời gian bắt đầu di chuyển
    private int currentTargetIndex = 0; // Chỉ số của mục tiêu hiện tại

    void Start()
    {
        // Khởi tạo vị trí ban đầu và mục tiêu đầu tiên
        startPosition = transform.position;
        currentTarget = targetPositions[currentTargetIndex].position;
        journeyLength = Vector3.Distance(startPosition, currentTarget);
        startTime = Time.time;
    }

    void Update()
    {
        // Tính toán thời gian đã trôi qua và di chuyển camera theo một quỹ đạo
        float distanceCovered = (Time.time - startTime) * moveSpeed;
        float fractionOfJourney = distanceCovered / journeyLength;

        // Di chuyển camera
        transform.position = Vector3.Lerp(startPosition, currentTarget, fractionOfJourney);

        if (fractionOfJourney >= 1)
        {
            // Khi đến đích, quay 90 độ sang phải (quay trên trục Y)
            transform.Rotate(0, 90f, 0);

            // Chuyển đến mục tiêu tiếp theo
            currentTargetIndex = (currentTargetIndex + 1) % targetPositions.Length; // Tiến đến mục tiêu tiếp theo (vòng lặp qua các đích)
            currentTarget = targetPositions[currentTargetIndex].position; // Cập nhật mục tiêu mới

            // Cập nhật lại vị trí ban đầu và reset thời gian
            startPosition = transform.position;
            journeyLength = Vector3.Distance(startPosition, currentTarget);
            startTime = Time.time;
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI; // Để sử dụng Text UI cho việc hiển thị "F"

public class RayCast : MonoBehaviour
{
    [SerializeField]
    private LayerMask layerMask;
    private int CongItem = 0;
    [SerializeField]
    private TextMeshProUGUI ItemCong;

    void Start()
    {
      
    }

    void Update()
    {
        // Kiểm tra va chạm với vật phẩm trong phạm vi raycast
        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            Destroy(hit.transform.gameObject);
            CongItem++;
            ItemCong.text = "Item:" + CongItem;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1, Color.yellow);
        }
    }
}

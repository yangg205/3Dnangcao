using System.Collections.Generic;
using UnityEngine;

public class MapGenerator2 : MonoBehaviour
{
    public Transform player; // Nhân vật
    public GameObject[] chunkPrefabs; // Các prefab chunks
    public int renderDistance = 3; // Số chunk giữ xung quanh nhân vật
    public float chunkSize = 50f; // Kích thước mỗi chunk

    private List<GameObject> activeChunks = new List<GameObject>();
    private Transform lastAnchorPoint; // Điểm neo của chunk cuối cùng

    void Start()
    {
        // Tạo chunk đầu tiên
        SpawnInitialChunks();
    }

    void Update()
    {
        // Kiểm tra nếu cần thêm hoặc hủy chunks
        UpdateChunks();
    }

    void SpawnInitialChunks()
    {
        // Tạo chunk đầu tiên
        GameObject firstChunk = Instantiate(chunkPrefabs[0], player.position, Quaternion.identity);
        activeChunks.Add(firstChunk);

        // Lấy anchor point cuối của chunk đầu tiên
        lastAnchorPoint = firstChunk.transform.Find("End Point");

        // Tạo các chunk tiếp theo
        for (int i = 1; i < renderDistance; i++)
        {
            SpawnNextChunk();
        }
    }

    void SpawnNextChunk()
    {
        // Chọn ngẫu nhiên một prefab chunk
        GameObject chunkPrefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];

        // Sinh chunk mới tại vị trí anchor point
        GameObject newChunk = Instantiate(chunkPrefab, lastAnchorPoint.position, Quaternion.identity);

        // Cập nhật lastAnchorPoint với chunk vừa tạo
        lastAnchorPoint = newChunk.transform.Find("End Point");

        // Thêm chunk vào danh sách
        activeChunks.Add(newChunk);
    }

    void UpdateChunks()
    {
        // Hủy chunks quá xa
        if (activeChunks.Count > renderDistance)
        {
            GameObject oldChunk = activeChunks[0];
            activeChunks.RemoveAt(0);
            Destroy(oldChunk);
        }

        // Tạo chunk mới nếu cần
        if (Vector3.Distance(player.position, lastAnchorPoint.position) < chunkSize)
        {
            SpawnNextChunk();
        }
    }
}

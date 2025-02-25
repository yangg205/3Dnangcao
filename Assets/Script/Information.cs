using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Information : MonoBehaviour
{
    [SerializeField] private TMP_Text InfoText; // Hiển thị thông tin người chơi
    [SerializeField] private Button NapTienButton; // Nút nạp tiền

    private int currentMoney = 0; // Lưu số tiền hiện tại
    private bool isCursorVisible = true; // Trạng thái của con trỏ (Mặc định là hiển thị)

    private void Start()
    {
        // Gắn sự kiện cho nút "Nạp tiền"
        NapTienButton.onClick.AddListener(Naptien);

        // Lấy email từ PlayerPrefs
        string email = PlayerPrefs.GetString("email");
        if (string.IsNullOrEmpty(email))
        {
            Debug.LogError("Email chưa được lưu trong PlayerPrefs!");
            return;
        }

        // Bắt đầu lấy thông tin người chơi và kiểm tra cập nhật
        StartCoroutine(GetPlayerInfo(email));
        StartCoroutine(CheckForUpdates(email));
    }

    private void Update()
    {
        // Hiển thị/Ẩn con trỏ khi nhấn ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursor(!isCursorVisible);
        }
    }

    private IEnumerator GetPlayerInfo(string email)
    {
        string apiUrl = "http://yang2206-001-site1.ptempurl.com/api/information";
        string json = $"{{\"email\":\"{email}\"}}";

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

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching player info: {request.error}");
            }
            else
            {
                string responseText = request.downloadHandler.text;
                PlayerResponse response = JsonUtility.FromJson<PlayerResponse>(responseText);
                if (response != null && response.data != null)
                {
                    UpdatePlayerInfo(response.data.name, response.data.total_money);
                }
                else
                {
                    Debug.LogError("Failed to parse player data.");
                }
            }
        }
    }

    private IEnumerator CheckForUpdates(string email)
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // Kiểm tra cập nhật mỗi 5 giây
            yield return GetPlayerInfo(email);
        }
    }

    private void UpdatePlayerInfo(string name, int totalMoney)
    {
        if (totalMoney != currentMoney) // Cập nhật khi số tiền thay đổi
        {
            currentMoney = totalMoney;
            InfoText.text = $"{name} ${currentMoney}";
            Debug.Log($"Updated player info: {name}, Money: {currentMoney}");
        }
    }

    public void Naptien()
    {
        // Mở trang web trong trình duyệt
        string url = "http://11212993:60-dayfreetrial@yang2206-001-site1.ptempurl.com";
        Debug.Log($"Opening URL: {url}");
        Application.OpenURL(url);
    }

    private void ToggleCursor(bool visible)
    {
        isCursorVisible = visible;

        Cursor.visible = isCursorVisible;
        Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;

        // Debug trạng thái con trỏ
        Debug.Log($"Cursor visible: {Cursor.visible}, Cursor lock state: {Cursor.lockState}");
    }
}

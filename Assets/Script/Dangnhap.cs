using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class Dangnhap : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI notificationText; // Thông báo trạng thái đăng nhập
    public GameObject notificationPanel; // Panel hiển thị thông báo
    public Loading loading;

    private void Start()
    {
        HideNotification(); // Ẩn thông báo khi bắt đầu game
    }

    // Gọi khi nhấn nút Đăng nhập
    public void OnLoginClick()
    {
        var email = emailInput.text.Trim();
        var password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowNotification("Email và mật khẩu không được để trống.");
            return;
        }

        var player = new Player
        {
            email = email,
            password = password,
        };
        var json = JsonUtility.ToJson(player);
        StartCoroutine(DangNhap(json));
    }

    // Coroutine thực hiện gọi API đăng nhập
    IEnumerator DangNhap(string json)
    {
        var url = "http://yang2206-001-site1.ptempurl.com/api/login"; // API URL
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // Chuẩn bị dữ liệu JSON
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Thêm header (nếu cần Basic Auth)
            string username = "11212993";
            string password = "60-dayfreetrial";
            string encodedAuth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
            request.SetRequestHeader("Authorization", $"Basic {encodedAuth}");
            request.SetRequestHeader("Content-Type", "application/json");

            // Gửi yêu cầu và chờ phản hồi
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Lỗi kết nối: {request.error}");
                ShowNotification("Không thể kết nối tới máy chủ.");
            }
            else
            {
                // Phân tích phản hồi JSON từ server
                var responseText = request.downloadHandler.text;
                Debug.Log($"Phản hồi từ server: {responseText}");

                // Phân tích JSON thành đối tượng
                var response = JsonUtility.FromJson<LoginResponse>(responseText);
                if (response != null && response.data != null && response.data.status)
                {
                    Debug.Log("Đăng nhập thành công, chuyển tới Map 1.");
                    // Lưu email và tên người chơi vào PlayerPrefs
                    PlayerPrefs.SetString("email", emailInput.text);
                    PlayerPrefs.Save();
                    //UnityEngine.SceneManagement.SceneManager.LoadScene("Map 1");
                    loading.StartLoading();
                }
                else
                {
                    ShowNotification(response?.data?.message ?? "Lỗi không xác định.");
                    Debug.LogError($"Lỗi đăng nhập: {response?.data?.message}");
                }
            }
        }
    }

    // Hiển thị thông báo với nội dung và tự động ẩn sau 5 giây
    private void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true); // Hiển thị panel thông báo
        StartCoroutine(HideNotificationAfterDelay(2f)); // Tự động ẩn sau 5 giây
    }

    // Ẩn thông báo
    private void HideNotification()
    {
        notificationPanel.SetActive(false);
    }

    // Coroutine tự động ẩn thông báo sau thời gian chỉ định
    IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideNotification();
    }
}

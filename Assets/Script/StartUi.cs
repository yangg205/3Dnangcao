using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class StartUi : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickStart()
    {
        // Lấy email từ PlayerPrefs
        string email = PlayerPrefs.GetString("email");

        // Gọi API reset điểm
        StartCoroutine(ResetPointAndStartGame(email));
    }


    IEnumerator ResetPointAndStartGame(string email)
    {
        string apiUrl = "http://yang2206-001-site1.ptempurl.com/api/resetpoint";

        // Tạo nội dung body JSON với email
        string json = $"{{\"email\":\"{email}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            // Thiết lập body và các header
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Header xác thực
            string username = "11212993";
            string password = "60-dayfreetrial";
            string encodedAuth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
            request.SetRequestHeader("Authorization", $"Basic {encodedAuth}");
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"Sending POST request to {apiUrl} with body: {json}");

            yield return request.SendWebRequest();

            // Xử lý kết quả
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error resetting points: {request.error}");
                Debug.LogError($"Response from server: {request.downloadHandler.text}");
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Debug.Log($"Server response: {responseText}");

                // Kiểm tra phản hồi
                bool isSuccess = responseText.Contains("\"data\":true");
                if (isSuccess)
                {
                    Debug.Log("Points reset successfully.");
                    UnityEngine.SceneManagement.SceneManager.LoadScene("TestDemo");
                }
                else
                {
                    Debug.LogError("Reset points failed. Check API response.");
                }
            }
        }
    }
}

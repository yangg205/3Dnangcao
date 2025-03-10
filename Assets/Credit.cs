using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Credit : MonoBehaviour
{
    public GameObject panel;
    public RectTransform creditText;
    public float scrollSpeed = 5f;
    private bool IsCreditActive = false;

    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void ShowCredit()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            IsCreditActive = true;
            Time.timeScale = 0f; // Dừng thời gian game
        }
    }

    void Update()
    {
        if (IsCreditActive && creditText != null)
        {
            // Cuộn credit
            creditText.anchoredPosition += Vector2.up * scrollSpeed * Time.unscaledDeltaTime;

            // Kiểm tra nếu đã cuộn hết credit
            if (creditText.anchoredPosition.y >= creditText.rect.height + 1f)
            {
                EndCredit();
            }
        }
    }

    private void EndCredit()
    {
        if (panel != null)
        {
            panel.SetActive(false);
            string email = PlayerPrefs.GetString("email");
            int totalPoint = GameManager.instance.currentScore;

            StartCoroutine(UpdatePointToServer(email, totalPoint));
            StartCoroutine(LoadSceneWithDelay("Map 1", 0f));
            Time.timeScale = 1f;
        }
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

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error updating points: {request.error}");
            }
            else
            {
                Debug.Log($"Server response: {request.downloadHandler.text}");
            }
        }
    }

    IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Đợi trong thời gian thực

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneName);
    }
}

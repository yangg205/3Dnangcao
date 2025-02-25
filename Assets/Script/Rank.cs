using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Rank : MonoBehaviour
{
    public TextMeshProUGUI top;
    public GameObject BXH;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BXH.SetActive(false);
    }
    public void OnClickBHX()
    {
        BXH.SetActive(true);
        StartCoroutine(Gettop10());
    }
    public void OffClickBXH()
    {
        BXH.SetActive(false);
    }
    IEnumerator Gettop10()
    {
        var url = "http://yang2206-001-site1.ptempurl.com/api/TopPlayer";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Thêm header Basic Auth
            string username = "11212993";
            string password = "60-dayfreetrial";
            string encodedAuth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
            request.SetRequestHeader("Authorization", $"Basic {encodedAuth}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Lỗi kết nối: {request.error}");
            }
            else
            {
                var responseText = request.downloadHandler.text;
                Debug.Log($"Phản hồi từ server: {responseText}");

                // Chuyển JSON thành đối tượng Top10Response
                var response = JsonUtility.FromJson<Top10Response>(responseText);

                if (response != null && response.data != null)
                {
                    // Hiển thị danh sách BXH
                    string bxh = "";
                    int rank = 1;

                    foreach (var player in response.data)
                    {
                        bxh += $"{rank}. {player}\n";
                        rank++;
                    }

                    top.text = bxh; // Cập nhật text trong UI
                    Debug.Log($"BXH:\n{bxh}");
                }
                else
                {
                    Debug.LogError("Phản hồi không hợp lệ hoặc không có dữ liệu BXH.");
                }
            }
        }
    }
}

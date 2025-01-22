using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class Dangky : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public void OnRegisterClick()
    {
        var name = nameInput.text;
        var email = emailInput.text;
        var password = passwordInput.text;
        var player = new Player
        {
            name = name,
            email = email,
            password = password,
        };
        var json = JsonUtility.ToJson(player);
        StartCoroutine(DangNhap(json));
    }
    IEnumerator DangNhap(string json)
    {
        var url = "http://yang2206-001-site1.ptempurl.com/api/regiter";
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //basic auth
        string username = "11212993";
        string password = "60-dayfreetrial";
        string encodedAuth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
        request.SetRequestHeader("Authorization", $"Basic {encodedAuth}");
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var text = request.downloadHandler.text;
            var respone = JsonUtility.FromJson<loginorregister>(text);
            if (respone.status)
            {
                Debug.Log("dang ky thanh cong");
                //load scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("Map 1");
            }
            else
            {
                Debug.Log("dang ky that bai");
                //thong bao loi
            }
        }
    }
}

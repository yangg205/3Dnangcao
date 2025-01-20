using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Rank : MonoBehaviour
{
    public TextMeshProUGUI top;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Gettop10());
    }
    IEnumerator Gettop10()
    {
        var url = $"http://localhost:5279/api/TopPlayer";
        var request = new UnityWebRequest(url);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var text = request.downloadHandler.text;
            //chuyen tu text sang obj
            var model = JsonUtility.FromJson<Top10>(text);
            if (model.status)
            {
                var data = model.data;
                string bxh = "";
                foreach (var x in data)
                {
                    bxh += x.ToString() + "\n";
                }
                top.text = bxh;
                Debug.Log(bxh);
            }
            else
            {
                Debug.Log("Status is false");
            }
        }
        Debug.Log("Received JSON: " + request.downloadHandler.text);

    }
}

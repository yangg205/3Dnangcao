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
        var url = $"http://yang2206-001-site1.ptempurl.com/api/TopPlayer";
        var request = new UnityWebRequest(url,"GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        //basic auth
        string username = "11212993";
        string password = "60-dayfreetrial";
        string encodedAuth = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));
        request.SetRequestHeader("Authorization", $"Basic {encodedAuth}");
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
                int rank = 1;
                foreach (var x in data)
                {
                    bxh += $"{rank}. {x.ToString()}\n";
                    rank++;
                }
                top.text = bxh;
                Debug.Log(bxh);
            }
            else
            {
                Debug.Log("Status is false");
            }
        }

    }
}

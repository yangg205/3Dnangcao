using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject paneldangkydangnhap;
    public GameObject paneldangky;
    public GameObject paneldangnhap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paneldangkydangnhap.SetActive(true);
        paneldangky.SetActive(false);
        paneldangnhap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickPanelDangNhap()
    {
        paneldangnhap.SetActive (true);
        paneldangkydangnhap.SetActive (false);
        paneldangky.SetActive (false);
    }
    public void OnClickPanelDangKy()
    {
        paneldangnhap.SetActive (false);
        paneldangkydangnhap.SetActive (false);
        paneldangky.SetActive (true);
    }
    public void OnClickBack()
    {
        paneldangnhap.SetActive(false);
        paneldangkydangnhap.SetActive(true);
        paneldangky.SetActive(false);
    }
}

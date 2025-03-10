using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject loadingPanel;
    public TextMeshProUGUI Text;
    public Slider Slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingPanel.SetActive(false);
    }
    public void StartLoading()
    {
        loadingPanel.SetActive(true);
        StartCoroutine(Load());

    }
    private IEnumerator Load()
    {
        Slider.value = 0;
        while(Slider.value < 1)
        {
            Slider.value += Time.deltaTime/3;//tang gia tri slider day trong 3 giay
            UpdateText();
            yield return null;
        }
        LoadingComplete();
    }
    private void UpdateText()
    {
        if (Text != null)
        {
            //cap nhat gia tri hien thi
            int load = Mathf.RoundToInt(Slider.value * 100);
            Text.text = $"{load}%";
        }
    }

    private void LoadingComplete()
    {
        //loadingPanel.SetActive(false);
        SceneManager.LoadScene("Map 1");
    }
}

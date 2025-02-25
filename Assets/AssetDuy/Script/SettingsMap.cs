using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMap : MonoBehaviour
{
    public Button map1Button;
    public Button map2Button;
    public Button continueButton;
    [SerializeField] private string selectedMap;
    void Start()
    {
        map1Button.onClick.AddListener(() => SelectedMap("TestDemo"));
        map2Button.onClick.AddListener(() => SelectedMap("Example"));
        continueButton.onClick.AddListener(LoadSelectedMap);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void SelectedMap(string mapName)
    {
        selectedMap = mapName;
    }
    public void LoadSelectedMap()
    {
        SceneManager.LoadScene(selectedMap);
    }
}

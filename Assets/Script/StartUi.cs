using UnityEngine;

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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map 2");
    }
}

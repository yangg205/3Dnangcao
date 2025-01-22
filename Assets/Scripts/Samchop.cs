using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Samchop : MonoBehaviour
{
    [SerializeField]
    public Light light;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100) > 98)
        {
            StartCoroutine(LightEffect());
        }
    }
    IEnumerator LightEffect()
    {
        light.enabled = true;
        yield return new WaitForSeconds(0.1f);
        light.enabled = false;
    }
}

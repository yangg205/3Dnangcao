using System.Collections;
using UnityEngine;

public class Samchop : MonoBehaviour
{
    public Light light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

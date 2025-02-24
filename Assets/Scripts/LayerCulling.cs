using UnityEngine;

public class LayerCulling : MonoBehaviour
{
    public int small = 5;
    public int large = 100;
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float[] distances = new float [32];
        distances[7] = small;
        distances[8] = large;
        camera.layerCullDistances = distances;
    }
}

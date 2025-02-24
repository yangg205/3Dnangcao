using System.Collections;
using UnityEngine;

public class AssetBundleLoader : MonoBehaviour
{
    string bundleName = "pompom";
    string assetName = "Capsule";
    IEnumerator Start()
    {
        string path = Application.streamingAssetsPath + "/Bundles/" + bundleName;
        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        if (bundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            yield break;
        }

        GameObject prefab = bundle.LoadAsset<GameObject>(assetName);
        Instantiate(prefab);
        bundle.Unload(false);
    }
}

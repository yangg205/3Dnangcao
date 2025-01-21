using UnityEngine;

public class ScopeController : MonoBehaviour
{
    public GameObject scopeImage;
    public float zoomSpeed = 2f;
    public Camera mainCamera;
    public Camera scopeCamera;
    private bool isScope = false;
    private float originalFOV;
    
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isScope = !isScope;
            scopeImage.SetActive(isScope);
            if(isScope)
            {
                mainCamera.gameObject.SetActive(false);
                scopeCamera.gameObject.SetActive(true);
                scopeCamera.enabled = true;
                originalFOV = scopeCamera.fieldOfView;
                scopeCamera.fieldOfView /= zoomSpeed;
            }
            else
            {
                mainCamera.gameObject.SetActive(true);
                scopeCamera.gameObject.SetActive(false);
                scopeCamera.enabled = false;
                scopeCamera.fieldOfView = originalFOV;
            }
        }
        if(isScope)
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            scopeCamera.fieldOfView -= zoom * zoomSpeed * 10f;
            scopeCamera.fieldOfView = Mathf.Clamp(scopeCamera.fieldOfView, 10f, 80f);
        }
    }
}

using UnityEngine;

public class Dichuyen : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    private float horizontal, vertical; //hướng di chuyển

    [SerializeField]
    private float speed = 3f; //tốc độ di chuyển

    private Vector3 movement; //tọa độ của player

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }
}
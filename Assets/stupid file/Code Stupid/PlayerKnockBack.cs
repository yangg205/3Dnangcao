using UnityEngine;

public class PlayerKnockBack : MonoBehaviour
{
    public float knockbackDrag = 5f;
    public float knockbackDuration = 0.5f; // Thời gian bị đẩy lùi
    private float knockbackSpeed;
    private CharacterController characterController;
    private Vector3 knockbackDirection;
    private float knockbackTimer;
    private float knockbackForce;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (knockbackTimer > 0)
        {
            characterController.Move(knockbackDirection * knockbackForce * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
             knockbackSpeed = Mathf.Lerp(knockbackSpeed, 0, Time.deltaTime * knockbackDrag); // Giảm tốc dần
        }
    }

    public void ApplyKnockback(Vector3 sourcePosition, float force)
    {
        knockbackDirection = (transform.position - sourcePosition).normalized;
        knockbackDirection.y = 0.25f; // Không đẩy lên trời
        knockbackForce = force;
        knockbackTimer = knockbackDuration;
    }
}

using Fusion;
using UnityEngine;

public class PlayerMovementt : NetworkBehaviour
{
    public CharacterController controller;
    public float speed = 2f;

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.fixedDeltaTime);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LimitCamera : MonoBehaviour
{
    public GameObject Player;
    private void LateUpdate()
    {
        transform.position = new Vector3 (Player.transform.position.x, 40, Player.transform.position.z);
    }
}

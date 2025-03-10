using UnityEngine;

public class CubeCredit : MonoBehaviour
{
    public Credit credit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            credit.ShowCredit();
        }
    }
}

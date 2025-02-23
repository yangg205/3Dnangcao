using UnityEngine;

public class MedkitPickUp : MonoBehaviour
{
        private bool isNearPlayer = false;

        void Update()
        {
            if (isNearPlayer && Input.GetKeyDown(KeyCode.E))
            {
                GameManager.instance.PickUpMedkit(gameObject);
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isNearPlayer = true;
                GameManager.instance.SetNearbyItem(gameObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isNearPlayer = false;
                GameManager.instance.ClearNearbyItem(gameObject);
            }
        }
    }
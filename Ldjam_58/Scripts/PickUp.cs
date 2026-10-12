using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;

    private StorageHandler storageHandler;

    private bool isPlayerInTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void Awake()
    {
        storageHandler = GameObject.Find("Storage").GetComponent<StorageHandler>();
    }

    private void Update()
    {
        if (isPlayerInTrigger)
        {
            if (Input.GetMouseButtonDown(0))
            {
                storageHandler.AddObjectToLeftHand(gameObject, audioClip);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                storageHandler.AddObjectToRightHand(gameObject, audioClip);
            }
        }
    }
}
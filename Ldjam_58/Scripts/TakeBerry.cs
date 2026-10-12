using UnityEngine;

public class TakeBerry : MonoBehaviour
{
    [SerializeField]
    private GameObject berry;

    [SerializeField]
    private GameObject blueberryEmpty;

    [SerializeField]
    private AudioClip audioClip;

    private StorageHandler storageHandler;

    private bool isPlayerInTrigger = false;

    private void Start()
    {
        storageHandler = GameObject.Find("Storage").GetComponent<StorageHandler>();
    }

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

    private void Update()
    {
        if (isPlayerInTrigger)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject currentBerry = Instantiate(berry, transform.position, transform.rotation);
                currentBerry.SetActive(false);
                Instantiate(blueberryEmpty, transform.position, transform.rotation);

                storageHandler.AddObjectToLeftHand(currentBerry, audioClip);

                gameObject.SetActive(false);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                GameObject currentBerry = Instantiate(berry, transform.position, transform.rotation);
                currentBerry.SetActive(false);
                Instantiate(blueberryEmpty, transform.position, transform.rotation);

                storageHandler.AddObjectToRightHand(currentBerry, audioClip);

                gameObject.SetActive(false);
            }
        }
    }
}
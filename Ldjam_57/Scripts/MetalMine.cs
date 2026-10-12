using TMPro;
using UnityEngine;

public class MetalMine : MonoBehaviour
{
    bool isColliding = false;
    public TMP_Text textMesh;
    void Update()
    {
        if (isColliding && Input.GetKeyDown(KeyCode.F))
        {
            PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            playerStats.metalCount++;
            textMesh.text = string.Empty;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = false;
        }
    }
}

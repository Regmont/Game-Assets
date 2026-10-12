using TMPro;
using UnityEngine;

public class BioMine : MonoBehaviour
{
    bool isColliding = false;
    TMP_Text textMesh;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            Transform playerUI = player.transform.Find("PlayerUI");
            if (playerUI != null)
            {
                textMesh = playerUI.Find("PressText")?.GetComponent<TMP_Text>();
            }
        }

        if (textMesh == null)
        {
            Debug.LogWarning("PressText (TMP_Text) не найден по пути: Player/PlayerUI/PressText");
        }
    }

    void Update()
    {
        if (isColliding && Input.GetKeyDown(KeyCode.F))
        {
            PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            playerStats.bioMaterialCount++;
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
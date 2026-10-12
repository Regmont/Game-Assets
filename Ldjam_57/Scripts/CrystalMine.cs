using TMPro;
using UnityEngine;

public class CrystalMine : MonoBehaviour
{
    int parts = 4;
    bool isColliding = false;
    Transform crystalParts;
    TMP_Text textMesh;

    private void Start()
    {
        crystalParts = transform.Find("Crystal_Parts");

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
            if (parts <= 0) return;

            PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            playerStats.crystalCount++;

            string partToDestroy = "CPart_" + (5 - parts);
            Transform partTransform = crystalParts.Find(partToDestroy);

            if (partTransform != null)
            {
                Destroy(partTransform.gameObject);
            }

            parts--;

            if (parts == 0)
            {
                Interact interact = GetComponent<Interact>();
                if (interact != null)
                {
                    interact.collected = true;
                }
                if (textMesh != null)
                {
                    textMesh.text = string.Empty;
                }
            }
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
using TMPro;
using UnityEngine;

public class ProcessMaterials : MonoBehaviour
{
    TMP_Text textMesh;
    bool isColliding = false;
    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

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
        if (isColliding)
        {
            string text = string.Empty;

            if (playerStats.metalCount > 0)
            {
                text += "Metal Alloy [1]\n";
            }
            if (playerStats.bioMaterialCount > 0)
            {
                text += "Bio-gel [2]\n";
            }
            if (playerStats.crystalCount > 0)
            {
                text += "Battery [3]";
            }

            textMesh.text = text;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (playerStats.metalCount > 0)
                {
                    playerStats.metalCount--;
                    playerStats.processedAlloy++;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

                if (playerStats.bioMaterialCount > 0)
                {
                    playerStats.bioMaterialCount--;
                    playerStats.bioGel++;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayerStats playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

                if (playerStats.crystalCount > 0)
                {
                    playerStats.crystalCount--;
                    playerStats.powerCells++;
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
            textMesh.text = string.Empty;
        }
    }
}

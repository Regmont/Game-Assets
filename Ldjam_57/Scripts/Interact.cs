using TMPro;
using UnityEngine;

public class Interact : MonoBehaviour
{
    TMP_Text textMesh;
    public bool collected = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            textMesh.text = "Press [F]";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        textMesh.text = string.Empty;
    }
}
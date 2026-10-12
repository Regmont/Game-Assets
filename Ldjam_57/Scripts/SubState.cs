using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubState : MonoBehaviour
{
    int stateAmount = 4;
    TMP_Text textMesh;
    private Collider playerCollider;

    public int state = 0;
    private int lastState = -1;

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
        if (other.CompareTag("Player"))
        {
            playerCollider = other;
            UpdateText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textMesh.text = string.Empty;
            playerCollider = null;
        }
    }

    void Update()
    {
        if (state != lastState && playerCollider != null)
        {
            UpdateText();
            lastState = state;
        }

        if (state >= stateAmount)
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    private void UpdateText()
    {
        switch (state)
        {
            case 0:
                textMesh.text = "[F]\nMetal Alloy - 3";
                break;
            case 1:
                textMesh.text = "[F]\nMetal Alloy - 5\nBattery - 1";
                break;
            case 2:
                textMesh.text = "[F]\nMetal Alloy - 1\nBio-gel - 3\nBattery - 3";
                break;
            case 3:
                textMesh.text = "[F]\nRETURN FROM WHENCE YOU CAME";
                break;
            default:
                textMesh.text = string.Empty;
                break;
        }
    }
}
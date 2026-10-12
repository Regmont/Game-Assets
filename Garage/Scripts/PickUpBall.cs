using TMPro;
using UnityEngine;

public class BallPickup : MonoBehaviour
{
    [SerializeField]
    private float pickupRadius = 1f;

    [SerializeField]
    private KeyCode pickupKey = KeyCode.Space;

    [SerializeField]
    private TMP_Text pickupText;

    [SerializeField]
    private BallController ballController;

    [SerializeField]
    private GameObject ball;

    private bool isPlayerInRange = false;
    private Transform playerTransform;

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (playerTransform == null)
        {
            FindPlayer();
            return;
        }

        float distance = Vector3.Distance(ball.transform.position, playerTransform.position);
        isPlayerInRange = distance <= pickupRadius;

        UpdatePickupText();

        if (isPlayerInRange && Input.GetKeyDown(pickupKey))
        {
            TryPickupBall();
        }
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure player has 'Player' tag.");
        }
    }

    private void UpdatePickupText()
    {
        if (pickupText != null)
        {
            if (isPlayerInRange && !ballController.IsHoldingBall)
            {
                pickupText.text = $"Press {pickupKey} to pick up ball";
            }
            else
            {
                pickupText.text = string.Empty;
            }
        }
    }

    private void TryPickupBall()
    {
        if (!ballController.IsHoldingBall && ballController.BallPhysical.activeInHierarchy)
        {
            PickupBall();
        }
    }

    private void PickupBall()
    {
        ballController.BallPhysical.SetActive(false);
        ballController.BallInHands.SetActive(true);
        ballController.ResetBallState();

        if (pickupText != null)
        {
            pickupText.text = string.Empty;
        }
    }
}
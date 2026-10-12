using UnityEngine;
using System.Collections.Generic;

public class ShowPickUp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer leftHandSprite;
    [SerializeField] private SpriteRenderer rightHandSprite;
    [SerializeField] private Sprite leftGrabSprite;
    [SerializeField] private Sprite rightGrabSprite;

    private HashSet<Collider> activeTriggers = new HashSet<Collider>();
    private Sprite basicLeftSprite;
    private Sprite basicRightSprite;
    private bool leftHandShowingGrab = false;
    private bool rightHandShowingGrab = false;

    public bool IsInPickupRange => activeTriggers.Count > 0;

    public void ForceShowGrabSprites()
    {
        if (leftHandSprite != null && leftGrabSprite != null)
        {
            leftHandSprite.sprite = leftGrabSprite;
            leftHandShowingGrab = true;
        }

        if (rightHandSprite != null && rightGrabSprite != null)
        {
            rightHandSprite.sprite = rightGrabSprite;
            rightHandShowingGrab = true;
        }
    }

    public void ForceShowBasicSprites()
    {
        if (leftHandSprite != null && basicLeftSprite != null)
        {
            leftHandSprite.sprite = basicLeftSprite;
            leftHandShowingGrab = false;
        }

        if (rightHandSprite != null && basicRightSprite != null)
        {
            rightHandSprite.sprite = basicRightSprite;
            rightHandShowingGrab = false;
        }
    }

    public void ClearAllTriggers()
    {
        activeTriggers.Clear();
        UpdateHandSprites();
    }

    private void Awake()
    {
        FindHandSprites();
        StoreBasicSprites();
    }

    private void FindHandSprites()
    {
        if (leftHandSprite == null)
        {
            leftHandSprite = GameObject.Find("LeftHand")?.GetComponent<SpriteRenderer>();
        }

        if (rightHandSprite == null)
        {
            rightHandSprite = GameObject.Find("RightHand")?.GetComponent<SpriteRenderer>();
        }
    }

    private void StoreBasicSprites()
    {
        if (leftHandSprite != null)
        {
            basicLeftSprite = leftHandSprite.sprite;
        }

        if (rightHandSprite != null)
        {
            basicRightSprite = rightHandSprite.sprite;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsValidTrigger(other))
        {
            activeTriggers.Add(other);
            UpdateHandSprites();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (activeTriggers.Contains(other))
        {
            activeTriggers.Remove(other);
            UpdateHandSprites();
        }
    }

    private void Update()
    {
        CleanupDestroyedColliders();
        UpdateHandSprites();
    }

    private void CleanupDestroyedColliders()
    {
        activeTriggers.RemoveWhere(col => col == null || !col.gameObject.activeInHierarchy);
    }

    private bool IsValidTrigger(Collider collider)
    {
        return collider.isTrigger &&
               (collider.CompareTag("Relic") || collider.CompareTag("Food") || collider.CompareTag("Bush"));
    }

    private void UpdateHandSprites()
    {
        bool shouldShowGrab = activeTriggers.Count > 0;

        UpdateLeftHandSprite(shouldShowGrab);
        UpdateRightHandSprite(shouldShowGrab);
        ValidateSpriteStates();
    }

    private void UpdateLeftHandSprite(bool shouldShowGrab)
    {
        if (leftHandSprite == null || leftGrabSprite == null || basicLeftSprite == null)
        {
            return;
        }

        if (shouldShowGrab && !leftHandShowingGrab)
        {
            if (leftHandSprite.sprite == basicLeftSprite)
            {
                leftHandSprite.sprite = leftGrabSprite;
                leftHandShowingGrab = true;
            }
        }
        else if (!shouldShowGrab && leftHandShowingGrab)
        {
            if (leftHandSprite.sprite == leftGrabSprite)
            {
                leftHandSprite.sprite = basicLeftSprite;
                leftHandShowingGrab = false;
            }
        }
    }

    private void UpdateRightHandSprite(bool shouldShowGrab)
    {
        if (rightHandSprite == null || rightGrabSprite == null || basicRightSprite == null)
        {
            return;
        }

        if (shouldShowGrab && !rightHandShowingGrab)
        {
            if (rightHandSprite.sprite == basicRightSprite)
            {
                rightHandSprite.sprite = rightGrabSprite;
                rightHandShowingGrab = true;
            }
        }
        else if (!shouldShowGrab && rightHandShowingGrab)
        {
            if (rightHandSprite.sprite == rightGrabSprite)
            {
                rightHandSprite.sprite = basicRightSprite;
                rightHandShowingGrab = false;
            }
        }
    }

    private void ValidateSpriteStates()
    {
        if (leftHandSprite != null)
        {
            if (leftHandSprite.sprite != leftGrabSprite && leftHandSprite.sprite != basicLeftSprite)
            {
                leftHandShowingGrab = false;
            }
        }

        if (rightHandSprite != null)
        {
            if (rightHandSprite.sprite != rightGrabSprite && rightHandSprite.sprite != basicRightSprite)
            {
                rightHandShowingGrab = false;
            }
        }
    }
}
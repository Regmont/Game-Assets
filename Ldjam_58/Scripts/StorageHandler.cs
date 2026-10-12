using System.Collections.Generic;
using UnityEngine;

public class StorageHandler : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    [SerializeField] private PlayerHunger playerHunger;
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private SpriteRenderer leftHandSprite;
    [SerializeField] private SpriteRenderer rightHandSprite;

    private List<ItemInHand> items = new();

    private float lastDropTime = 0f;
    private float dropCooldown = 0.5f;
    private float lastPickupTime = 0f;
    private float pickupDropDelay = 0.1f;

    private Sprite basicLeftSprite;
    private Sprite basicRightSprite;

    private void Start()
    {
        basicLeftSprite = leftHandSprite.sprite;
        basicRightSprite = rightHandSprite.sprite;

        Sprite[] leftSprites = Resources.LoadAll<Sprite>("Sprites/Left Objects");
        Sprite[] rightSprites = Resources.LoadAll<Sprite>("Sprites/Right Objects");

        for (int i = 0; i < leftSprites.Length; i++)
        {
            string itemName = leftSprites[i].name.Replace("Left", "").Trim();
            items.Add(new ItemInHand(itemName, leftSprites[i], rightSprites[i]));
        }
    }

    public bool LeftHandIsEmpty()
    {
        return leftHand == null;
    }

    public bool RightHandIsEmpty()
    {
        return rightHand == null;
    }

    public GameObject GetLeftHandItem()
    {
        return leftHand;
    }

    public GameObject GetRightHandItem()
    {
        return rightHand;
    }

    public void RemoveRelicFromHands(GameObject relic)
    {
        if (leftHand == relic)
        {
            leftHand = null;
            leftHandSprite.sprite = basicLeftSprite;
        }
        else if (rightHand == relic)
        {
            rightHand = null;
            rightHandSprite.sprite = basicRightSprite;
        }
    }

    public void AddObjectToLeftHand(GameObject gameObject, AudioClip audioClip)
    {
        if (leftHand == null && Time.time > lastDropTime + dropCooldown)
        {
            gameObject.SetActive(false);
            leftHand = gameObject;
            lastPickupTime = Time.time;
            leftHandSprite.sprite = items.Find(x => x.ItemName == gameObject.name).LeftHandSprite;
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }

    public void AddObjectToRightHand(GameObject gameObject, AudioClip audioClip)
    {
        if (rightHand == null && Time.time > lastDropTime + dropCooldown)
        {
            gameObject.SetActive(false);
            rightHand = gameObject;
            lastPickupTime = Time.time;
            rightHandSprite.sprite = items.Find(x => x.ItemName== gameObject.name).RightHandSprite;
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }

    private void Update()
    {
        if (Time.time < lastPickupTime + pickupDropDelay)
        {
            return;
        }

        bool isInteractingWithAltar = IsPlayerNearAltar();

        if (Input.GetMouseButtonDown(0) && !LeftHandIsEmpty() && !isInteractingWithAltar)
        {
            Vector3 dropVector = transform.position + transform.forward * 0.8f - transform.right * 0.3f;
            DropItem(leftHand, dropVector);
            leftHand = null;
            lastDropTime = Time.time;
            leftHandSprite.sprite = basicLeftSprite;
        }

        if (Input.GetMouseButtonDown(1) && !RightHandIsEmpty() && !isInteractingWithAltar)
        {
            Vector3 dropVector = transform.position + transform.forward * 0.8f + transform.right * 0.3f;
            DropItem(rightHand, dropVector);
            rightHand = null;
            lastDropTime = Time.time;
            rightHandSprite.sprite = basicRightSprite;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!RightHandIsEmpty() && rightHand.gameObject.CompareTag("Food"))
            {
                Destroy(rightHand.gameObject);
                rightHand = null;
                playerHunger.AddHunger(20f);
                playerHealth.RestoreHealth();
                rightHandSprite.sprite = basicRightSprite;

                return;
            }

            if (!LeftHandIsEmpty() && leftHand.gameObject.CompareTag("Food"))
            {
                Destroy(leftHand.gameObject);
                leftHand = null;
                playerHunger.AddHunger(20f);
                playerHealth.RestoreHealth();
                leftHandSprite.sprite = basicLeftSprite;

                return;
            }
        }
    }

    private bool IsPlayerNearAltar()
    {
        Altar altar = FindFirstObjectByType<Altar>();
        if (altar != null && altar.enabled)
        {
            return altar.IsPlayerInRange();
        }

        return false;
    }

    private void DropItem(GameObject item, Vector3 dropVector)
    {
        item.transform.position = dropVector;
        item.transform.rotation = Quaternion.identity;
        item.SetActive(true);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

public class ItemInHand
{
    public string ItemName => _itemName;
    public Sprite LeftHandSprite => _leftHandSprite;
    public Sprite RightHandSprite => _rightHandSprite;

    private readonly string _itemName;
    private readonly Sprite _leftHandSprite;
    private readonly Sprite _rightHandSprite;

    public ItemInHand(string itemName, Sprite leftHandSprite, Sprite rightHandSprite)
    {
        _itemName = itemName;
        _leftHandSprite = leftHandSprite;
        _rightHandSprite = rightHandSprite;
    }
}
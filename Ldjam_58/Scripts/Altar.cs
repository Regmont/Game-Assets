using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Altar : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int relicsRequired = 3;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 3f;

    [Header("Audio")]
    [SerializeField] private AudioClip placementSound;
    [SerializeField] private AudioClip completionSound;
    [Range(0f, 1f)][SerializeField] private float placementVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float completionVolume = 1f;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem placementEffect;
    [SerializeField] private ParticleSystem completionEffect;
    [SerializeField] private Light altarLight;
    [SerializeField] private Color[] lightColors;

    [Header("UI")]
    [SerializeField] private Text relicCountText;
    [SerializeField] private Image fadeImage;

    [Header("Debug")]
    [SerializeField] private bool enableDebug = true;
    [SerializeField] private bool showGizmos = true;

    private int currentRelics = 0;
    private StorageHandler storageHandler;
    private Transform player;
    private bool isCompleted = false;
    private Canvas fadeCanvas;

    public int CurrentRelics => currentRelics;
    public int RelicsRequired => relicsRequired;
    public bool IsCompleted => isCompleted;
    public float CompletionPercentage => (float)currentRelics / relicsRequired;

    public void ForceComplete()
    {
        if (!isCompleted)
        {
            currentRelics = relicsRequired;
            StartCoroutine(CompleteAltar());
        }
    }

    public void AddRelic()
    {
        if (!isCompleted)
        {
            currentRelics = Mathf.Clamp(currentRelics + 1, 0, relicsRequired);
            UpdateUI();
            UpdateVisuals();

            if (currentRelics >= relicsRequired)
            {
                StartCoroutine(CompleteAltar());
            }
        }
    }

    public void ResetAltar()
    {
        currentRelics = 0;
        isCompleted = false;
        UpdateUI();
        UpdateVisuals();

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
            fadeImage.color = Color.clear;
        }
    }

    public bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= interactionDistance;
    }

    private void Start()
    {
        FindRequiredComponents();
        SetupFadeImage();
        SetupLightColors();
        UpdateUI();
        UpdateVisuals();
    }

    private void Update()
    {
        if (isCompleted || storageHandler == null || player == null)
        {
            return;
        }

        if (IsPlayerInRange())
        {
            HandleRelicPlacement();
        }
    }

    private void FindRequiredComponents()
    {
        storageHandler = GameObject.Find("Storage")?.GetComponent<StorageHandler>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (storageHandler == null && enableDebug)
        {
            Debug.LogWarning("Altar: StorageHandler not found");
        }

        if (player == null && enableDebug)
        {
            Debug.LogWarning("Altar: Player not found");
        }
    }

    private void SetupFadeImage()
    {
        if (fadeImage == null)
        {
            CreateFadeImage();
        }
        else
        {
            fadeImage.color = Color.clear;
            fadeImage.gameObject.SetActive(false);
        }
    }

    private void SetupLightColors()
    {
        if (lightColors == null || lightColors.Length == 0)
        {
            lightColors = new Color[]
            {
                Color.red,
                Color.yellow,
                Color.green,
                Color.cyan
            };
        }
    }

    private void HandleRelicPlacement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceRelic(storageHandler.GetLeftHandItem());
        }
        else if (Input.GetMouseButtonDown(1))
        {
            TryPlaceRelic(storageHandler.GetRightHandItem());
        }
    }

    private void TryPlaceRelic(GameObject handItem)
    {
        if (handItem != null && handItem.CompareTag("Relic"))
        {
            PlaceRelic(handItem);
        }
    }

    private void PlaceRelic(GameObject relic)
    {
        currentRelics++;

        PlayPlacementSound();
        PlayPlacementEffects();
        RemoveRelicFromStorage(relic);
        UpdateVisuals();
        UpdateUI();

        if (enableDebug)
        {
            Debug.Log($"Altar: Relic placed {currentRelics}/{relicsRequired}");
        }

        if (currentRelics >= relicsRequired)
        {
            StartCoroutine(CompleteAltar());
        }
    }

    private void PlayPlacementSound()
    {
        if (placementSound != null)
        {
            AudioSource.PlayClipAtPoint(placementSound, transform.position, placementVolume);
        }
    }

    private void PlayPlacementEffects()
    {
        if (placementEffect != null)
        {
            placementEffect.Play();
        }
    }

    private void RemoveRelicFromStorage(GameObject relic)
    {
        storageHandler.RemoveRelicFromHands(relic);
        Destroy(relic);
    }

    private void UpdateVisuals()
    {
        if (altarLight != null && lightColors.Length > 0)
        {
            int colorIndex = Mathf.Clamp(currentRelics, 0, lightColors.Length - 1);
            altarLight.color = lightColors[colorIndex];
            altarLight.intensity = 1f + (currentRelics * 0.5f);
        }
    }

    private void UpdateUI()
    {
        if (relicCountText != null)
        {
            relicCountText.text = $"{currentRelics}/{relicsRequired}";
        }
    }

    private IEnumerator CompleteAltar()
    {
        isCompleted = true;

        PlayCompletionEffects();

        if (enableDebug)
        {
            Debug.Log("Altar: Completed, starting end sequence");
        }

        yield return StartCoroutine(FadeToBlack());
    }

    private void PlayCompletionEffects()
    {
        if (completionSound != null)
        {
            AudioSource.PlayClipAtPoint(completionSound, transform.position, completionVolume);
        }

        if (completionEffect != null)
        {
            completionEffect.Play();
        }
    }

    private IEnumerator FadeToBlack()
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = Color.black;
    }

    private void CreateFadeImage()
    {
        GameObject fadeObject = new GameObject("FadeImage");

        fadeCanvas = FindFirstObjectByType<Canvas>();
        if (fadeCanvas == null)
        {
            GameObject canvasObj = new GameObject("FadeCanvas");
            fadeCanvas = canvasObj.AddComponent<Canvas>();
            fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        fadeObject.transform.SetParent(fadeCanvas.transform, false);
        fadeImage = fadeObject.AddComponent<Image>();
        fadeImage.color = Color.clear;
        fadeImage.rectTransform.anchorMin = Vector2.zero;
        fadeImage.rectTransform.anchorMax = Vector2.one;
        fadeImage.rectTransform.offsetMin = Vector2.zero;
        fadeImage.rectTransform.offsetMax = Vector2.zero;
        fadeObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
        {
            return;
        }

        Gizmos.color = isCompleted ? Color.green :
                      currentRelics > 0 ? Color.yellow : Color.white;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);

        #if UNITY_EDITOR
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f,
            $"Altar: {currentRelics}/{relicsRequired}");
        #endif
    }
}
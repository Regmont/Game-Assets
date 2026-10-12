using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    TMP_Text Timer;

    TMP_Text MetalCount;
    TMP_Text BioMaterialCount;
    TMP_Text CrystalCount;

    TMP_Text ProcessedAlloy;
    TMP_Text BioGel;
    TMP_Text PowerCells;

    public float remainingTime = 600f;
    
    PlayerStats playerStats;
    SubState subState;


    public Image healthBar;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;

    public Image ProgressBar;
    public Sprite sprite1_1;
    public Sprite sprite2_1;
    public Sprite sprite3_1;
    public Sprite sprite4_1;


    void Start()
    {
        Timer = transform.Find("Timer")?.GetComponent<TMP_Text>();
        MetalCount = transform.Find("Metal/MetalCount")?.GetComponent<TMP_Text>();
        BioMaterialCount = transform.Find("BioMaterial/BioMaterialCount")?.GetComponent<TMP_Text>();
        CrystalCount = transform.Find("Crystal/CrystalCount")?.GetComponent<TMP_Text>();
        ProcessedAlloy = transform.Find("ProcessedAlloy/PACount")?.GetComponent<TMP_Text>();
        BioGel = transform.Find("BioGel/BGCount")?.GetComponent<TMP_Text>();
        PowerCells = transform.Find("PowerCells/PCCount")?.GetComponent<TMP_Text>();

        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        subState = GameObject.Find("Station").GetComponent<SubState>();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        Timer.text = $"{minutes}:{seconds:00}";

        if (remainingTime > 1)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene("DrownScene");
        }

        int currentHealth = playerStats.GetHp();

        switch (currentHealth)
        {
            case 4:
                healthBar.sprite = sprite1;
                break;
            case 3:
                healthBar.sprite = sprite2;
                break;
            case 2:
                healthBar.sprite = sprite3;
                break;
            case 1:
                healthBar.sprite = sprite4;
                break;
            default:
                healthBar.sprite = sprite5;
                break;
        }

        int state = subState.state;

        switch (state)
        {
            case 0:
                ProgressBar.sprite = sprite1_1;
                break;
            case 1:
                ProgressBar.sprite = sprite2_1;
                break;
            case 2:
                ProgressBar.sprite = sprite3_1;
                break;
            case 3:
                ProgressBar.sprite = sprite4_1;
                break;
        }

        MetalCount.text = playerStats.metalCount.ToString();
        BioMaterialCount.text = playerStats.bioMaterialCount.ToString();
        CrystalCount.text = playerStats.crystalCount.ToString();
        ProcessedAlloy.text = playerStats.processedAlloy.ToString();
        BioGel.text = playerStats.bioGel.ToString();
        PowerCells.text = playerStats.powerCells.ToString();
    }
}
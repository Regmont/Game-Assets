using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health System")]
    [SerializeField] private int maxHealth = 4;
    [SerializeField] private int currentHealth;
    [SerializeField] private List<Image> healthSprites;

    [Header("Dependencies")]
    [SerializeField] private PlayerHunger hungerSystem;

    public System.Action<int> OnHealthChanged;

    public int GetCurrentHealth => currentHealth;

    public void RestoreHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            UpdateHealthUI();
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        HandleHungerDamage();
    }

    private void HandleHungerDamage()
    {
        if (hungerSystem != null && hungerSystem.ShouldTakeHungerDamage())
        {
            TakeDamage(1);
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < healthSprites.Count; i++)
        {
            healthSprites[i].gameObject.SetActive(false);
        }

        if (currentHealth < maxHealth)
        {
            int frameIndex = maxHealth - currentHealth - 1;
            if (frameIndex >= 0 && frameIndex < healthSprites.Count)
            {
                healthSprites[frameIndex].gameObject.SetActive(true);
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            TakeDamage(1);
            MonsterController monster = collision.gameObject.GetComponent<MonsterController>();
            if (monster != null)
            {
                monster.DisableMonster();
            }
        }
    }
}
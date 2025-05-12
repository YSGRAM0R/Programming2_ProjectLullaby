using UnityEngine;
using UnityEngine.UI; 

public class HealthStaminaSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthRegenRate = 2f;
    public Slider healthSlider;
    public bool isHealthRegenerating = true;
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 5f; 
    public float sprintStaminaDrainRate = 10f;
    public Slider staminaSlider;

    [Header("Player Settings")]
    public float staminaUseDelay = 0.5f; 
    private float lastStaminaUseTime;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        if (isHealthRegenerating)
        {
            RegenerateHealth();
        }
        
        RegenerateStamina();
        HandleSprinting();
        UpdateUI();
    }
    
    void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }
    
    void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }
    
    void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && Time.time - lastStaminaUseTime > staminaUseDelay)
        {
            currentStamina -= sprintStaminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            lastStaminaUseTime = Time.time;
        }
        else
        {
            RegenerateStamina();
        }
    }
    
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void Die()
    {
        Debug.Log("Player Died");
    }
    
    void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }
    
    public void ToggleHealthRegeneration(bool enable)
    {
        isHealthRegenerating = enable;
    }
}

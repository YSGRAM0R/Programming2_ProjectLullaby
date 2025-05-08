using UnityEngine;
using UnityEngine.UI; // For UI components like Slider and Button

public class HealthStaminaSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthRegenRate = 2f; // Health regeneration per second
    public Slider healthSlider; // Reference to UI health slider
    public bool isHealthRegenerating = true; // Whether health regeneration is active or not

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 5f; // Stamina regeneration per second
    public float sprintStaminaDrainRate = 10f; // Stamina drained per second while sprinting
    public Slider staminaSlider; // Reference to UI stamina slider

    [Header("Player Settings")]
    public float staminaUseDelay = 0.5f; // Delay between stamina usage actions (e.g., sprinting)
    private float lastStaminaUseTime;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // If you have UI sliders to show health and stamina, initialize them
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

    // Health Regeneration
    void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    // Stamina Regeneration
    void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    // Handle Sprinting Stamina Drain
    void HandleSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && Time.time - lastStaminaUseTime > staminaUseDelay)
        {
            // Sprinting drains stamina
            currentStamina -= sprintStaminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            lastStaminaUseTime = Time.time;
        }
        else
        {
            // If not sprinting, regenerate stamina
            RegenerateStamina();
        }
    }

    // Apply damage to the player
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Heal the player
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // Handle Player Death
    void Die()
    {
        // Handle the player's death, like playing an animation or ending the game
        Debug.Log("Player Died");
        // Example: Disable player movement, show death UI, etc.
    }

    // Update Health and Stamina UI
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

    // Method to toggle health regeneration
    public void ToggleHealthRegeneration(bool enable)
    {
        isHealthRegenerating = enable;
    }
}

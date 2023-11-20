using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject respawnPosition;

    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth = 150;

    public HealthSlider healthAmount;

    private void Start()
    {
        currentHealth = maxHealth;

        healthAmount.SetMaxhealth(maxHealth);
    }

    private void FixedUpdate()
    {
        ResetHealth();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        currentHealth -= 10;
        healthAmount.HealthAmount(currentHealth);
    }

    private void ResetHealth()
    {
        if (currentHealth < 1)
        {
            // Als de Health van de speler kleiner is dan 1, respawned hij op de respawnPosition gameObject
            gameObject.transform.position = respawnPosition.transform.position;
            // Na het respawnen krijgt hij weer maxHealth
            currentHealth = maxHealth;
            healthAmount.HealthAmount(maxHealth);
        }
    }
}

using System;
using UnityEngine;

public class HealthManager
{
    public bool IsDie => CurrentHealth <= MinHealth;
    public bool IsALife => CurrentHealth > MinHealth;

    public event Action<float> HealthChanged = delegate { };

    public int OwnerId;

    private int regenRate = 5;
    private float regenCooldown = 3.0f;
    private float maxRegenCooldown = 3.0f;

    private bool canRegen = false;
    private bool startCooldown = false;
    private float _currentPlayerHealth = 100.0f;
    private float lastPlayerHealthUpdate;

    public float CurrentHealth
    {
        get
        {
            return _currentPlayerHealth;
        }
        private set
        {
            _currentPlayerHealth = value;
        }
    }

    public float MaxHealth { get; private set; } = 100;
    public float MinHealth { get; private set; } = 0;

    public HealthManager(float maxPlayerHealth = 100)
    {
        MaxHealth = maxPlayerHealth;
        _currentPlayerHealth = maxPlayerHealth;
        lastPlayerHealthUpdate = maxPlayerHealth;
    }

    public void TakePureDamage(float pureDamage)
    {
        SetPureHealth(CurrentHealth - pureDamage);
    }

    public void SetPureHealth(float newHealth)
    {
        if (newHealth == CurrentHealth)
            return;

        var isTakingDamage = newHealth < _currentPlayerHealth;

        if (isTakingDamage) //damage here 
        {
            canRegen = false;
            startCooldown = true;
            regenCooldown = maxRegenCooldown;
        }
        else { } //heal here 

        CurrentHealth = newHealth;

        if (CurrentHealth < MinHealth)
            CurrentHealth = MinHealth;

        if (Math.Abs(lastPlayerHealthUpdate - CurrentHealth) > 1 || CurrentHealth == MinHealth || CurrentHealth == MaxHealth)
        {
            CurrentHealth = GetNormalizedHealth(newHealth);
            lastPlayerHealthUpdate = CurrentHealth;
            HealthChanged(CurrentHealth);
        }
    }

    public void Tick()
    {
        if (startCooldown)
        {
            regenCooldown -= Time.deltaTime;
            if (regenCooldown <= 0)
            {
                canRegen = true;
                startCooldown = false;
            }
        }

        if (canRegen)
        {
            if (CurrentHealth < MaxHealth)
            {
                SetPureHealth(CurrentHealth + regenRate * Time.deltaTime);
            }
            else
            {
                SetPureHealth(MaxHealth);
                regenCooldown = maxRegenCooldown;
                canRegen = false;
            }
        }
    }

    private float GetNormalizedHealth(float health)
    {
        if (health < MinHealth)
            return MinHealth;
        if (health > MaxHealth)
            return MaxHealth;
        else
            return (float)Math.Round(health, 0);
    }
}

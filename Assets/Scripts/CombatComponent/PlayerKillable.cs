using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerKillable : Killable
{
    [Header("Regeneration Values")]
    [SerializeField] float timeBeforeRegen; // Rime from the last instance of damage before you start regenerating
    [SerializeField] float healthRegenRate; // Regenerate rate per second

    float timeBeforeRegenTimer;
    bool isRegerating = false;

    private void Start()
    {
        currentHP = maxHP;
        OnTakeDamage.AddListener(ResetRegenTimer); // Resetting the timer everytime player takes damage
        OnDie.AddListener(PlayerDied);
        timeBeforeRegenTimer = timeBeforeRegen; // Initalizing timer
    }

    private void Update()
    {
        if (!isDead)
        {
            if (isRegerating)
            {
                Regen();
            }
            else
            {
                RegenTimer();
            }
        }
    }
    private void Regen()
    {
        GainHealth(healthRegenRate * Time.deltaTime);

        if (currentHP >= maxHP)
        {
            isRegerating = false; // Once full health reset timer and stop regening
        }
    }
    private void RegenTimer()
    {
        // Time before regen timer, resets when takes damage -> see Start()
        timeBeforeRegenTimer -= Time.deltaTime;
        if (timeBeforeRegenTimer <= 0)
        {
            isRegerating = true;
            timeBeforeRegenTimer = timeBeforeRegen;
        }
    }
    public void ResetRegenTimer()
    {
        timeBeforeRegenTimer = timeBeforeRegen;
    }
    private void PlayerDied() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedEffect
{
    public float percentSpeed; // inital percent increase in speed so 0.2 == 20% speed boost
    public float currentPercentSpeed; // the speed increase will gradually decrease based on the duration timer
    public float duration; // max duration
    public float durationTimer; // current duration, decreasing with over time

    public SpeedEffect(float _duration, float _percentIncrease)
    {
        percentSpeed = _percentIncrease;
        currentPercentSpeed = _percentIncrease;
        duration = _duration;
        durationTimer = _duration;
    }
}

public class MovementModification : MonoBehaviour
{
    public float boostForAll; // 0-1 value represent percentage
    public UnityEvent OnModifyMovement = new UnityEvent();
    List<SpeedEffect> speedEffects = new List<SpeedEffect>();

    private void Update()
    {
        for(int i = 0; i < speedEffects.Count; i++)
        {
            speedEffects[i].durationTimer -= Time.deltaTime;
            if (speedEffects[i].durationTimer <= 0f)
            {
                speedEffects.RemoveAt(i);
                i--;
                continue;
            }
            speedEffects[i].currentPercentSpeed = speedEffects[i].percentSpeed * (speedEffects[i].durationTimer / speedEffects[i].duration);
        }
    }

    public void SetBoost(float boost)
    {
        boostForAll = boost;
        OnModifyMovement.Invoke();
    }

    public void AddSpeedBoost(float duration, float percentIncrease)
    {
        SpeedEffect deathSpeed = new SpeedEffect(duration, percentIncrease);
        speedEffects.Add(deathSpeed);
    }
    public float CalcBoostedSpeed(float amount, bool increaseAmount) {
        float output = amount;
        foreach(SpeedEffect effect in speedEffects) {
            if (increaseAmount) {
                output += effect.currentPercentSpeed * amount;
            }
            else {
                output -= effect.currentPercentSpeed * amount;
                output = Mathf.Max(0, output);
            }
        }

        return output;
    }
    public float GetBoost(float baseAmount, float boostedAmount, bool useBuff) {
        float output = Mathf.Lerp(baseAmount, boostedAmount, boostForAll);
        if (useBuff) {
            return CalcBoostedSpeed(output, boostedAmount > baseAmount);
        }
        return output;
    }
}


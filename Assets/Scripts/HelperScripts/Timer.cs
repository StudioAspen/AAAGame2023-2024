using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Timer
{
    bool isActive = false;
    float currentTime;
    public UnityEvent OnTimerFinish = new UnityEvent();
    public void StartTimer(float amount, UnityAction call = null) {
        currentTime = amount;
        isActive = true;
        AddEventToEnd(call);
    }
    public void UpdateTimer() {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0 && isActive) {
            isActive = false;
            OnTimerFinish.Invoke();
        }
    }
    public void AddEventToEnd(UnityAction call) {
        OnTimerFinish.AddListener(call);
    }
    public void RemoveEventToEnd(UnityAction call) {
        OnTimerFinish.AddListener(call);
    }
    public void EndTimer() {
        OnTimerFinish.Invoke();
        OnTimerFinish.RemoveAllListeners();
        isActive = false;
    }
    public void CancelTimer() {
        OnTimerFinish.RemoveAllListeners();
        isActive = false;
    }
    public bool IsActive()
    {
        return isActive;
    }
}

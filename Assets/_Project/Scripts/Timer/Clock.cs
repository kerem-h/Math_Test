using System;
using UnityEngine;

public class Clock
{
    public float RemainingSeconds { get; private set; }
    public Action onTimerEnd;
    
    public Clock(float duration)
    {
        RemainingSeconds = duration;
    }
    

    public void Tick(float deltaTime)
    {
        if (RemainingSeconds <= 0) return;
        
        RemainingSeconds -= deltaTime;
        CheckTime();
    }

    private void CheckTime() {
        if (RemainingSeconds <= 0) {
            RemainingSeconds = 0;
            onTimerEnd?.Invoke();
        }
    }

    public string GetFormatedTime_MMSS()
    {
        if (RemainingSeconds <= 0) return "00:00";
        
        int minutes = Mathf.FloorToInt(RemainingSeconds / 60f);
        int seconds = Mathf.FloorToInt(RemainingSeconds % 60f);
        
        return $"{minutes:00}:{seconds:00}";
    }
}
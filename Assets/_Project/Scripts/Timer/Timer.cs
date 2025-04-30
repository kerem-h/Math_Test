using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Singleton
    public static Timer Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    public Action<string> OnTimerChanged;
    public Action OnTimerStarted;
    public Action OnTimerEnded;

    private Clock _clock;
    private int _previousSecond;

    // The elapsed time in seconds since the timer started — made public and static
    public float elapsedTime;

    private void Start()
    {
        MailManager.Instance.OnTestStarted += StartTimer;
        _previousSecond = Mathf.FloorToInt(GameData.TestTime);
    }

    private void Update()
    {
        // If there's no clock (timer hasn’t started), do nothing.
        if (_clock == null) return;

        // Update the clock (countdown) based on real time.
        _clock.Tick(Time.deltaTime);

        // Only accumulate elapsed time if the timer is still going.
        if (_clock.RemainingSeconds > 0)
        {
            elapsedTime += Time.deltaTime;
        }

        // Check if the displayed time has changed.
        if (IsTimeChanged())
        {
            SetTimerUi();
        }
    }

    private void SetTimerUi()
    {
        OnTimerChanged?.Invoke(_clock.GetFormatedTime_MMSS());
        _previousSecond = Mathf.FloorToInt(_clock.RemainingSeconds);
    }

    private bool IsTimeChanged()
    {
        return _previousSecond != Mathf.FloorToInt(_clock.RemainingSeconds);
    }

    public void StartTimer()
    {
        // Initialize a new clock and subscribe to its end event.
        _clock = new Clock(GameData.TestTime);
        _clock.onTimerEnd += OnTimerEnd;

        // Reset elapsed time each time the timer starts.
        elapsedTime = 0f;

        OnTimerStarted?.Invoke();
    }

    private void OnTimerEnd()
    {
        OnTimerEnded?.Invoke();
        // At this point, elapsedTime will hold the total time from start to finish.
    }
}

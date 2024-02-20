using System;
using UnityEngine;


public class Timer : MonoBehaviour
{
    #region Singletion

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
    

    private void Start()
    {
        MailManager.Instance.OnTestStarted += StartTimer;
        
        _previousSecond = Mathf.FloorToInt(GameData.TestTime);
    }
    
    private void Update()
    {
        if (_clock == null) return;
        
        _clock.Tick(Time.deltaTime);
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
        _clock = new Clock(GameData.TestTime);
        _clock.onTimerEnd += OnTimerEnd;
        OnTimerStarted?.Invoke();
    }

    private void OnTimerEnd()
    {
        OnTimerEnded?.Invoke();
    }
    

}
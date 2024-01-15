using TMPro;
using UnityEngine;

public class TimerUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public GameObject timerPanel;

    private void Start()
    {
        if (GameData.IsSolution)
        {
            timerPanel.SetActive(false);
            return;
        }
        Timer.Instance.OnTimerChanged += OnTimerChanged;
        Timer.Instance.OnTimerStarted += OnTimerStarted;
    }

    private void OnTimerStarted()
    {
        timerPanel.SetActive(true);
    }


    private void OnTimerChanged(string text)
    {
        timerText.text = text;
    }
}

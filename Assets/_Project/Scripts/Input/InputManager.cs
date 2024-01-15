using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Action OnNextQuestion;
    public Action OnPreviousQuestion;
    
    public Action OnNextAnswer;
    public Action OnPreviousAnswer;
    public Action OnAnswerSelected;
    
    private bool isPlaying = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        MailManager.Instance.OnTestStarted += () => isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying) {
            AnswerNavigation();
            QuestionNavigation();
        }
    }

    private void QuestionNavigation()
    {
        if (Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.Q))
        {
            // Next Question
            OnNextQuestion?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.A))
        {
            // Previous Question
            OnPreviousQuestion?.Invoke();
        }
    }

    private void AnswerNavigation()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnPreviousAnswer?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnNextAnswer?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Select Answer
            OnAnswerSelected?.Invoke();
        }
    }
}

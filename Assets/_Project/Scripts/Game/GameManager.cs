using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action OnQuestionChanged;
    public Action OnTestChanged;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else
            Destroy(gameObject);
        if (!GameData.IsSolution) InitializeData();

    }

    private void Start()
    {
        InputManager.Instance.OnNextQuestion += () =>
        {
            ChangeQuestion(GameData.CurrentQuestion + 1);
            OnQuestionChanged?.Invoke();
        };
        InputManager.Instance.OnPreviousQuestion += () =>
        {
            ChangeQuestion(GameData.CurrentQuestion - 1);
            OnQuestionChanged?.Invoke();
        };
        AnswerController.Instance.OnAnswerSelected += (currentIndex) => GameData.SelectQuestion(currentIndex);

        Popup.Instance.OnPopupButtonClicked += i =>
        {
            ChangeQuestion(i - 1);
            OnQuestionChanged?.Invoke();
        };
        
        Timer.Instance.OnTimerEnded += FinishTest; 
    }

    public void ChangeQuestion(int i)
    {
        if (i < 0) {i = 0; return;}
        else if (i == GameData.QuestionCount[GameData.CurrentTest])
        { i = GameData.QuestionCount[GameData.CurrentTest] - 1; return; }
        GameData.CurrentQuestion = i;
    }
    
    private void InitializeData()
    {
        // Change this part if you need it
        GameData.TestTimes = new[] { 1080f };
        GameData.QuestionCount = new[] {25};
        GameData.TestCount = 1;
        GameData.AnswerCount = 5;
        
        GameData.CurrentQuestion = 0;
        GameData.CurrentTest = 0;
        GameData.IsSolution = false;

        GameData.Questions = new List<Question[]>();
        for (int i = 0; i < GameData.TestCount; i++) {
            GameData.Questions.Add(new Question[GameData.QuestionCount[i]]);
            for (int j = 0; j < GameData.QuestionCount[i] ; j++)
            {
                var question = new Question();
                GameData.Questions[i][j] = question;
            }
        }
    }

    public void FinishTest() {
        if (GameData.IsGameFinished())
        {
            GameData.CurrentTest = 0;
            GameData.CurrentQuestion = 0;
            SceneManager.LoadScene(Scenes.FinishScene.ToString());
        }
        else {
            GameData.CurrentTest++;
            GameData.CurrentQuestion = 0;
            OnQuestionChanged?.Invoke();
            OnTestChanged?.Invoke();
        }
        
        
    }
}

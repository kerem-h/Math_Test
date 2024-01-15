using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action OnQuestionChanged;
    
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
        
        Timer.Instance.OnTimerEnded += FinishGame;
    }

    public void ChangeQuestion(int i)
    {
        if (i < 0) {i = 0; return;}
        else if (i == GameData.QuestionCount)
        { i = GameData.QuestionCount - 1; return; }
        GameData.CurrentQuestion = i;
    }
    
    private void InitializeData()
    {
        GameData.QuestionCount = 24;
        GameData.CurrentQuestion = 0;
        GameData.AnswerCount = 4;
        GameData.IsSolution = false;

        GameData.Questions = new Question[GameData.QuestionCount];
        for (int i = 0; i < GameData.QuestionCount ; i++)
        {
            var question = new Question();
            question.SetQuestion();
            GameData.Questions[i] = question;
        }
    }

    public void FinishGame()
    {
        SceneManager.LoadScene(Scenes.FinishScene.ToString());
    }
}

using System;
using UnityEngine;

public class QuestionManager : MonoBehaviour {
    
    #region Singleton

    public static QuestionManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else 
            Destroy(gameObject);
    }

    #endregion
    public Action OnQuestionChanged;

    private void Start()
    {
        GameManager.Instance.OnQuestionChanged += SetQuestion;
    }

    private void SetQuestion()
    {
        GameData.GetQuestion(GameData.CurrentQuestion, (question => {
            OnQuestionChanged?.Invoke();
        }));
    }
}

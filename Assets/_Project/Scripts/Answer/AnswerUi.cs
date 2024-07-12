using System;
using UnityEngine;

public class AnswerUi : MonoBehaviour
{
    public AnswerButtons[] AnswerButtons;
    public static AnswerUi Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetAnswerButtons();
        GameManager.Instance.OnQuestionChanged += SetAnswerButtons;
        AnswerController.Instance.OnAnswerSelected += Select;
        AnswerController.Instance.OnHowerChanged += HoverButton;
    }

    private void HoverButton(int i)
    {
        if (i == -1)
        {
            ResetHover();
            return;
        }
        var currentButton = AnswerButtons[i];
        ResetHover();
        currentButton.Hover();
    }

    public void Select(int current)
    {
        if (current == -1)
        {
            ResetSelection();
            return;
        }
        var button = AnswerButtons[current];
        if (!button.IsSelected)
            ResetSelection();
        button.Select();
    }

    public void DisableButton()
    {
        AnswerButtons[^1].gameObject.SetActive(false);
    }

    public void EnableButton()
    {
        AnswerButtons[^1].gameObject.SetActive(true);
    }
    private void SetAnswerButtons()
    {
        GameData.GetQuestion(GameData.CurrentQuestion, (question) =>
        {
            if (question == null)
            {
                Debug.LogError("Question is null.");
                return;
            }

            var isSolution = GameData.IsSolution;
            var selectedAnswer = question.selectedAnswer;

            for (int i = 0; i < AnswerButtons.Length; i++)
            {
                var button = AnswerButtons[i];
                button.Index = i;
                button.ResetHover();
                button.ResetSelection();

                if (isSolution)
                {
                    button.DisableButton();
                    if (i == question.correctAnswer)
                        button.SetCorrectButton();
                    if (i == question.selectedAnswer && question.selectedAnswer != question.correctAnswer)
                        button.SetWrongButton();
                }
                else
                {
                    if (i == question.selectedAnswer)
                        button.Select();
                }
            }
        });
    }


    private void ResetHover()
    {
        for (int i = 0; i < AnswerButtons.Length; i++)
        {
            var button = AnswerButtons[i];
            button.ResetHover();
        }
    }
    private void ResetSelection()
    {
        for (int i = 0; i < AnswerButtons.Length; i++)
        {
            var button = AnswerButtons[i];
            button.ResetSelection();
        }
    }
}

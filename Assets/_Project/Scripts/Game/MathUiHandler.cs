using TMPro;
using UnityEngine;

public class MathUiHandler : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI[] AnswerTexts;

    public GameObject ExplanationButton;
    public TextMeshProUGUI ExplanationText;

    public PopupAnimationSettings OpenAnimation;
    public PopupAnimationSettings CloseAnimation;
    public GameObject PopupPanel;
    public GameObject ExplanationPanel;
    
    private void Start()
    {
        SetExplanationButton();
        SetExplanationText();
        QuestionManager.Instance.OnQuestionChanged += SetExplanationText;
    }

    private void SetExplanationButton()
    {
        if (GameData.IsSolution)
        {
            ExplanationButton.SetActive(true);
        }
        else
            ExplanationButton.SetActive(false);
    }

    private void SetExplanationText() {
        var questionData = GameData.GetCurrentQuestionData();
        if (questionData == null) return;
        ExplanationText.text = questionData.Explanation;
    }

    public void SetQuestionUi(string question, string[] answers)
    {
        QuestionText.text = question;
        for (int i = 0; i < answers.Length; i++)
        {
            AnswerTexts[i].text = answers[i];
        }
    }

    public void Explanation()
    {
        if (PopupPanel.transform.localScale != Vector3.zero) {
            CloseAnimation.OnAnimationEnd += () => ExplanationPanel.SetActive(false);
            TweenAnimations.PopUpAnimation(PopupPanel, CloseAnimation);
        }
        else {
            ExplanationPanel.SetActive(true);
            TweenAnimations.PopUpAnimation(PopupPanel, OpenAnimation);
        }
    }
}
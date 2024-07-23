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
    [SerializeField] private GameObject _visual1;

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

    private void SetExplanationText()
    {
        QuestionData questionData = null;
        GameData.GetCurrentQuestionData((questionDat) =>
        {
            questionData = questionDat;
            if (questionData == null) return;
            ExplanationText.text = questionData.Explanation;
        });
    }

    public void SetQuestionUi(string question, string[] answers, int correctAnswerIndex)
    {
        QuestionText.text = question;
        // TODO: Implement this where the image is the visual for the question and there will be text where the numbers will be placed. Images should be set with their texts and it
        // TODO: should be checked if the question contains "|" character. If it contains, the text should be hidden and the image should be shown.
        // if (question.Contains("|"))
        // {
        //     QuestionText.gameObject.SetActive(false);
        //     _visual1.SetActive(true);
        // }
        // else
        // {
        //     QuestionText.gameObject.SetActive(true);
        //     _visual1.SetActive(false);
        // }
        AnswerTexts[correctAnswerIndex].text = answers[0];
        var index = 1;
        for (int i = 0; i < GameData.AnswerCount; i++) {
            
            if (i == correctAnswerIndex) continue;
            AnswerTexts[i].text = answers[index].Trim();
            index++;
        }
    }

    public void Explanation()
    {
        if (PopupPanel.transform.localScale != Vector3.zero)
        {
            CloseAnimation.OnAnimationEnd += () => {
                PopupPanel.transform.localScale = Vector3.zero;
                ExplanationPanel.SetActive(false);
            };
            TweenAnimations.PopUpAnimation(PopupPanel, CloseAnimation);
        }
        else {
            ExplanationPanel.SetActive(true);
            TweenAnimations.PopUpAnimation(PopupPanel, OpenAnimation);
        }
    }
}
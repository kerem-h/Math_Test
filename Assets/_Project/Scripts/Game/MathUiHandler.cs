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
    [SerializeField] private GameObject prefabPattern;
    [SerializeField] private PatternManager patternManager;

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
        if (question.Contains("|"))
        {
            QuestionText.gameObject.SetActive(false);
            var list = question.Trim('|').Split('|');
            var _number = int.Parse(list[0]);
            var _values = list[1].Split(',');
            patternManager.DeleteAllPatterns();
            patternManager.SpawnPattern(_number, _values );
        }
        else
        {
            QuestionText.gameObject.SetActive(true);
            patternManager.DeleteAllPatterns();
        }
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
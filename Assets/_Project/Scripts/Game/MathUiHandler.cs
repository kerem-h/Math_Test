using TMPro;
using UnityEngine;

public class MathUiHandler : MonoBehaviour
{
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI[] AnswerTexts;

    public void SetQuestionUi(string question, string[] answers)
    {
        QuestionText.text = question;
        for (int i = 0; i < answers.Length; i++)
        {
            AnswerTexts[i].text = answers[i];
        }
    }
}
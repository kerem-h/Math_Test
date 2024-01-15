using TMPro;
using UnityEngine;

public class QuestionUi : MonoBehaviour
{
    
    public TextMeshProUGUI QuestionText;
    public GameObject NextButton;
    public GameObject PreviousButton;
    
    private void Start()
    {
        SetQuestionUi();
        QuestionManager.Instance.OnQuestionChanged += SetQuestionUi;
    }

    private void SetQuestionUi()
    {
        var question = GameData.GetQuestion(GameData.CurrentQuestion);
        SetQuestionButtons();
        QuestionText.text = (GameData.CurrentQuestion+1).ToString();
    }

    private void SetQuestionButtons()
    {
        NextButton.SetActive(true);
        PreviousButton.SetActive(true);
        if (GameData.CurrentQuestion == 0) PreviousButton.SetActive(false);
        else if (GameData.CurrentQuestion == GameData.QuestionCount - 1) NextButton.SetActive(false);
    }

    public void NextQuestionButton()
    {
        GameManager.Instance.ChangeQuestion(GameData.CurrentQuestion + 1);
        GameManager.Instance.OnQuestionChanged?.Invoke();
    }
    public void PreviousQuestionButton()
    {
        GameManager.Instance.ChangeQuestion(GameData.CurrentQuestion - 1);
        GameManager.Instance.OnQuestionChanged?.Invoke();
    }
}

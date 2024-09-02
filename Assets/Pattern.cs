using System;
using TMPro;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    
    public TextMeshProUGUI[] Texts;
    
    public void SetPattern(string[] texts)
    {
        try
        {
            for (int i = 0; i < Texts.Length; i++)
            {
                if (texts.Length <= i)
                {
                    Texts[i].text = "";
                    continue;
                }
                if (texts[i] == "?" && GameData.IsSolution)
                { 
                    Texts[i].color = Color.red;
                    var changedTextIndex = i;
                    GameData.GetQuestion(GameData.CurrentQuestion, question => 
                    {
                        if (question == null)
                        {
                            Debug.LogError("Question is null.");
                            return;
                        }

                        // var correctAnswer = question.correctAnswer;
                        var answerStrings = question.questionData.AnswerStrings[0];
                        Texts[changedTextIndex].text = answerStrings;
                    });

                }
                else {
                    Texts[i].color = Color.black;
                    Texts[i].text = texts[i];
                }
            }
            // ExplanationText.text = explanation;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Start()
    {
        if (GameData.IsSolution)
        {
            // ShowSolution();
        }
    }

}

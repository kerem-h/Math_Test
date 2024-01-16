using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.Text.RegularExpressions;

public class MathHandler : MonoBehaviour
{
    public Dictionary<string, string> variables = new()
    {
        { "{A}", "{1}" }, { "{B}", "{2}" },
        { "{C}", "{3}" }, { "{D}", "{4}" }, { "{E}", "{5}" }, { "{F}", "{6}" }, { "{G}", "{7}" }, { "{H}", "{8}" },
        { "{I}", "{9}" },
        { "{J}", "{10}" }, { "{K}", "{11}" }, { "{L}", "{12}" }, { "{M}", "{13}" }, { "{N}", "{14}" },
        { "{O}", "{15}" }, { "{P}", "{16}" },
        { "{Q}", "{17}" }, { "{R}", "{18}" }, { "{S}", "{19}" }, { "{T}", "{20}" }, { "{U}", "{21}" },
        { "{V}", "{22}" }, { "{W}", "{23}" },
        { "{X}", "{24}" }, { "{Y}", "{25}" }, { "{Z}", "{26}" }
    };

    public static MathHandler Instance;
    public List<QuestionData> questions;
    MathUiHandler _mathUiHandler;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _mathUiHandler = FindObjectOfType<MathUiHandler>();
        GameManager.Instance.OnQuestionChanged += SetQuestionUi;
        StartCoroutine(HandleQuestions());
    }

    private void SetQuestionUi()
    {
        var question = questions[GameData.CurrentQuestion];
        var answer = question.FinalAnswer;
        var answers = new string[4];
        answers[0] = answer;
        for (int i = 1; i < answers.Length; i++)
        {
            var random = UnityEngine.Random.Range(0, questions.Count);
            answers[i] = questions[random].FinalAnswer;
        }
        _mathUiHandler.SetQuestionUi(question.Question, answers);
    }

    private IEnumerator HandleQuestions()
    {
        yield return new WaitUntil(() => questions != null);

        for (int i = 0; i < questions.Count; i++)
        {
            questions[i] = ChangeVariables(questions[i]);
            SetParameters(questions[i]);
            SetQuestionText(questions[i]);
            CalculateAnswer(questions[i]);
        }
        SetQuestionUi();
    }

    private void CalculateAnswer(QuestionData question)
    {

        var answer = question.Answer;
        var steps = answer.Split(",");
        for (int i = 0; i < steps.Length; i++)
        {
            question.ParameterCount += 1;
            var step = steps[i];
            step = SetResult(question, step);
            var result = EvaluateExpression(step);
            string key = "{" + question.ParameterCount + "}";
            question.Variables.Add(key, result);
        }
        string k = "{" + question.ParameterCount + "}";
        var final =  question.Variables[k];
        question.FinalAnswer = final.ToString();
    }

    private string SetResult(QuestionData question, string step) {
        foreach (var variable in question.Variables) {
            step = step.Replace(variable.Key, variable.Value.ToString());
        }
        return step;
    }

    private QuestionData ChangeVariables(QuestionData question)
    {
        foreach (var v in variables)
        {
            question.Question = question.Question.Replace(v.Key, v.Value);
            question.Answer = question.Answer.Replace(v.Key, v.Value);
            for (int i = 0; i < question.Options.Length; i++)
            {
                question.Options[i] = question.Options[i].Replace(v.Key, v.Value);
            }

            question.Explanation = question.Explanation.Replace(v.Key, v.Value);
        }

        return question;
    }

    private static void SetParameters(QuestionData question)
    {
        foreach (var option in question.Options)
        {
            var _ = option.Split(":");
            var match = _[0];
            var range = _[1].Split(",");
            var min = range[0][range[0].Length - 1].ToString();
            var max = range[1][0].ToString();
            var random = UnityEngine.Random.Range(int.Parse(min), int.Parse(max) + 1);
            
            question.Variables.Add(match, random);
            question.ParameterCount += 1;
            
            question.Question = question.Question.Replace(match, random.ToString());
            question.Answer = question.Answer.Replace(match, random.ToString());
            question.Explanation = question.Explanation.Replace(match, random.ToString());
        }
    }
    
    private static void SetQuestionText(QuestionData question)
    {
        EvaluateCalculations(question);
    }


    private static void EvaluateCalculations(QuestionData question)
    {
        string pattern = @"\[(.*?)\]";
        // Find matches
        MatchCollection matches = Regex.Matches(question.Question, pattern);

        foreach (Match match in matches) {
            string expression = match.Groups[1].Value;
            var result = EvaluateExpression(expression);

            question.Question = question.Question.Replace(match.Value, result.ToString());
            
        }

        question.Question = question.Question.Trim('\ufeff');
        
        MatchCollection matches2 = Regex.Matches(question.Explanation, pattern);
        
        foreach (Match match in matches2) {
            string expression = match.Groups[1].Value;
            var result = EvaluateExpression(expression);

            question.Explanation = question.Explanation.Replace(match.Value, result.ToString());
        }
    }

    static float EvaluateExpression(string expression)
    {
        DataTable table = new DataTable();
        table.Columns.Add("expression", string.Empty.GetType(), expression);
        DataRow row = table.NewRow();
        table.Rows.Add(row);
        float result = float.Parse((string)row["expression"]);
        return result;
    }
}



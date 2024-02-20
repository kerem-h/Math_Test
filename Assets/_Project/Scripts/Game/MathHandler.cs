using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

public class MathHandler : MonoBehaviour
{
    private Dictionary<string, string> variables = new()
    {
        { "{A}", "{1}" }, { "{B}", "{2}" }, { "{C}", "{3}" }, 
        { "{D}", "{4}" }, { "{E}", "{5}" }, { "{F}", "{6}" },
        { "{G}", "{7}" }, { "{H}", "{8}" }, { "{I}", "{9}" },
        { "{J}", "{10}" }, { "{K}", "{11}" }, { "{L}", "{12}" },
        { "{M}", "{13}" }, { "{N}", "{14}" }, { "{O}", "{15}" },
        { "{P}", "{16}" }, { "{Q}", "{17}" }, { "{R}", "{18}" }, 
        { "{S}", "{19}" }, { "{T}", "{20}" }, { "{U}", "{21}" },
        { "{V}", "{22}" }, { "{W}", "{23}" }, { "{X}", "{24}" },
        { "{Y}", "{25}" }, { "{Z}", "{26}" }
    };

    MathUiHandler _mathUiHandler;
    public List<List<QuestionData>> questionsList;

    #region Singleton

        public static MathHandler Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(this);
            }
        }

    #endregion


    private void Start()
    {
        _mathUiHandler = FindObjectOfType<MathUiHandler>();
        GameManager.Instance.OnQuestionChanged += SetQuestionUi;
        if (GameData.IsSolution) {
            SetQuestionUi();
            return;
        }
        StartCoroutine(HandleQuestions());
    }

    private void SetQuestionUi() {
        
        var questionData = GameData.GetCurrentQuestionData();
        var answers = questionData.AnswerStrings;
        
        _mathUiHandler.SetQuestionUi(questionData.Question, answers);
    }

    private IEnumerator HandleQuestions()   
    {
        yield return new WaitUntil(() => questionsList != null);
        ProcessAllQuestions();
        RandomizeQuestions();
        SetQuestionUi();
    }

    private void ProcessAllQuestions()
    {
        foreach (var questionSet in questionsList)
        {
            foreach (var question in questionSet)
            {
                ProcessQuestion(question);
            }
        }
    }
    private void ProcessQuestion(QuestionData question)
    {
        question = ChangeVariables(question);
        SetParameters(question);
        CalculateAnswer(question);
        SetQuestionText(question);
    }
    private void RandomizeQuestions()
    {
        for (int i = 0; i < questionsList.Count; i++)
        {
            var questionCount = GameData.QuestionCount[i];
            List<QuestionData> randomizedQuestions = new List<QuestionData>();
            var questions = questionsList[i].ToList();


            while (randomizedQuestions.Count < questionCount)
            {
                Helper.Shuffle(questions);
                foreach (var question in questions) {
                    if (randomizedQuestions.Count >= questionCount) break;
                    randomizedQuestions.Add(question);
                }
            }

          
            questionsList[i] = randomizedQuestions;
            GameData.SetQuestionData(questionsList[i], i);
        }
    }
    


    private void CalculateAnswer(QuestionData question)
    {

        var answer = question.AnswerFormule;
        var steps = answer.Split(";");
        
        for (int i = 0; i < steps.Length; i++)
        {
            question.ParameterCount += 1;
            var step = steps[i];
            step = SetResult(question, step);
            var result = EvaluateExpression(step);
            string key = "{" + question.ParameterCount + "}";
            question.Variables.Add(key, result);
        }
        
        SetAnswers(question);
    }

    private static void SetAnswers(QuestionData question)
    {
        List<int> clockIndexes = new List<int>();
        var data = question.Answers.Split(":");
        var dec = data[0].Trim();
        var tempAnswers = data[1].Trim().Split(";");
        
        for (int i = 0; i < question.ClockVariables.Count; i++) {
            
            for (int j = 0; j < tempAnswers.Length; j++) {
                
                if (tempAnswers[j].Contains(question.ClockVariables[i])) {
                    clockIndexes.Add(j);
                }
            }
        }

        foreach (var v in question.Variables) {
            question.Answers = question.Answers.Replace(v.Key, v.Value.ToString("F" + dec));
        }
        
        var _ = question.Answers.Split(":");
        var answers = _[1].Trim().Split(";");
        string pattern = @"\[(.*?)\]";

        for (int i = 0; i < answers.Length; i++) {
            var answerText = answers[i];
            
            MatchCollection matches = Regex.Matches(answerText, pattern);
            foreach (Match match in matches) {
                string expression = match.Groups[1].Value;
                var result = EvaluateExpression(expression);
                if (clockIndexes.Contains(i)) {
                    answerText = answerText.Replace(match.Value, GetClockFromMinute((int) result));
                }
                answerText = answerText.Replace(match.Value, result.ToString("F" + dec));
            }
            
            answers[i] = answerText;
        }
        question.AnswerStrings = answers;
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
            question.Explanation = question.Explanation.Replace(v.Key, v.Value);
            question.AnswerFormule = question.AnswerFormule.Replace(v.Key, v.Value);
            question.Answers = question.Answers.Replace(v.Key, v.Value);
            
            for (int i = 0; i < question.ClockVariables.Count; i++) 
                question.ClockVariables[i] = question.ClockVariables[i].Replace(v.Key, v.Value);
            
            for (int i = 0; i < question.Ranges.Length; i++) 
                question.Ranges[i] = question.Ranges[i].Replace(v.Key, v.Value);
        }
        return question;
    }

    private static void SetParameters(QuestionData question)
    {
        try
        {
            foreach (var option in question.Ranges)
            {
                var _ = option.Split(":");
                var match = _[0].Trim();
                var range = _[1].Trim().Split(",");
                var min = int.Parse(range[0].Trim('('));
                var max = int.Parse(range[1]);
                var iter = int.Parse(range[2].Trim(')'));

                int stepCount = (max + 1 - min) / iter + 1;
                int randomIndex = UnityEngine.Random.Range(0, stepCount);
                var randomValue = min + randomIndex * iter;
                
                question.Variables.Add(match, randomValue);
                question.ParameterCount += 1;

                if (question.ClockVariables.Contains(match)) {
                    var clock = GetClockFromMinute(randomValue);
                    question.Question = question.Question.Replace(match, clock);
                    question.AnswerFormule = question.AnswerFormule.Replace(match, randomValue.ToString());
                    question.Explanation = question.Explanation.Replace(match, clock);
                }
                else {
                    question.Question = question.Question.Replace(match, randomValue.ToString());
                    question.AnswerFormule = question.AnswerFormule.Replace(match, randomValue.ToString());
                    question.Explanation = question.Explanation.Replace(match, randomValue.ToString());
                }
            
            }
        }
        catch (Exception exception) {
            DebugManager.Instance.AddLogs("Error Setting Parameters on the question " + question.QuestionIndex + ":\n" + exception.Message + "\n" + "Check your Ranges in the CSV file");
            Debug.Log("Error Setting Parameters on the question " + question.QuestionIndex + ":\n" + exception.Message);
        }
        
    }

    private static string GetClockFromMinute(int value) {
        // Ensure value is within 24 hours
        value = value % 1440;
        
        // Convert minutes to clock format
        string clock = "";
        int hours = value / 60;
        int minutes = value % 60;
        if (hours < 10) {
            if (minutes < 10) clock = "0" + hours + ":0" + minutes;
            else clock = "0" + hours + ":" + minutes;
        }
        else {
            if (minutes < 10) clock = hours + ":0" + minutes;
            else clock = hours + ":" + minutes;
        }
        if (hours < 12) clock += " AM";
        else clock += " PM";
        return clock;
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
        try
        {
            // DebugManager.Instance.AddLogs("Evaluating expression: " + expression);
            // ExpressionEvaluator.Evaluate(expression, out float res);
            // DebugManager.Instance.AddLogs("Result: " + res);
            // return res;
            DataTable table = new DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            float result = float.Parse((string)row["expression"]);
            return result;
        }
        catch (Exception e)
        {
            DebugManager.Instance.AddLogs("Error evaluating expression: " + expression + "\n" + e.Message);
            Debug.Log("Error evaluating expression: " + expression + "\n" + e.Message);
            throw;
        }
    }
}



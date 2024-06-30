using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Globalization;

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
        { "{Y}", "{25}" }, { "{Z}", "{26}" } , { "{AA}", "{27}" }, { "{AB}", "{28}" }, { "{AC}", "{29}" }, 
        { "{AD}", "{30}" },  { "{AE}", "{31}" },  { "{AF}", "{32}" },
        { "{AG}", "{33}" },  { "{AH}", "{34}" },  { "{AI}", "{35}" },
        { "{AJ}", "{36}" }, { "{AK}", "{37}" }, { "{AL}", "{38}" },
        { "{AM}", "{39}" }, { "{AN}", "{40}" }, { "{AO}", "{41}" },
        { "{AP}", "{42}" }, { "{AQ}", "{43}" }, { "{AR}", "{44}" }, 
        { "{AS}", "{45}" }, { "{AT}", "{46}" }, { "{AU}", "{47}" },
        { "{AV}", "{48}" }, { "{AW}", "{49}" }, { "{AX}", "{50}" },
        { "{AY}", "{51}" }, { "{AZ}", "{52}" }
    };

    MathUiHandler _mathUiHandler;
    public List<List<QuestionData>> questionsList;
    public bool IsDataLoaded;
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

    private void SetQuestionUi()
    {
        var question = GameData.GetQuestion(GameData.CurrentQuestion);
        var AnswerIndex = question.correctAnswer;

        
        var questionData = GameData.GetCurrentQuestionData();
        var answers = questionData.AnswerStrings;
        GameData.AnswerCount = questionData.AnswerCount;
        if (questionData.AnswerCount == 5)
        {
            AnswerUi.Instance.EnableButton();
        }
        else
        {
            AnswerUi.Instance.DisableButton();
        }
        
        _mathUiHandler.SetQuestionUi(questionData.Question, answers, AnswerIndex);
    }

    private IEnumerator HandleQuestions()   
    {
        yield return new WaitUntil(() => IsDataLoaded);
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
        question.AnswerCount = question.Answers.Split(":")[1].Split(";").Length;
        
        // change the placeholder variables with the actual variables
        ChangeVariables(question);
        
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
        var dec = answer.Split(":")[0].Trim();
        var steps = answer.Split(":")[1].Trim().Split(";");
        
        for (int i = 0; i < steps.Length; i++)
        {
            question.ParameterCount += 1;
            var step = steps[i];
            step = SetResult(question, step);
            var result = EvaluateExpression(step);
            string key = "{" + question.ParameterCount + "}";
            question.Variables.Add(key, float.Parse(result.ToString("F" + dec, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture));
        }
        
        SetAnswers(question);
    }

    private static void SetAnswers(QuestionData question)
    {
        List<int> clockIndexes = new List<int>();
        var data = question.Answers.Split(":");
        var dec = data[0].Split(";")[0].Trim();
        var decText = data[0].Split(";")[1].Trim();
        var tempAnswers = data[1].Trim().Split(";");
        

        foreach (var v in question.Variables) {
            question.Answers = question.Answers.Replace(v.Key, v.Value.ToString(CultureInfo.InvariantCulture));
        }
        
        var _ = question.Answers.Split(":");
        var answers = _[1].Trim().Split(";");
        string pattern = @"\[(.*?)\]";

        for (int i = 0; i < answers.Length; i++) {
            var answerText = answers[i];
            
            MatchCollection matches = Regex.Matches(answerText, pattern);
            foreach (Match match in matches)
            {
                var val = StringModifier.RandomlyDeletePlusOrMinus(match.Groups[1].Value);
                val = StringModifier.RandomlyDeleteSlashOrAsterisk(val);
                string expression = val;
                expression = expression.Replace(",", ".");
                expression = expression.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
                var result = EvaluateExpression(expression);
                if (clockIndexes.Contains(i)) {
                    answerText = answerText.Replace(match.Value, GetClockFromMinute(float.Parse(result.ToString(CultureInfo.InvariantCulture))).ToString(CultureInfo.InvariantCulture));
                }
                // if rounding is not wanted delete this.
                if (dec != "0")
                {
                    answerText = answerText.Replace(match.Value, result.ToString("F" + dec, CultureInfo.InvariantCulture));
                }
                
                else
                {
                    if (decText == "Floor")
                        answerText = answerText.Replace(match.Value, MathF.Floor(result).ToString(CultureInfo.InvariantCulture));
                    
                    else if (decText == "Ceil")
                        answerText = answerText.Replace(match.Value, MathF.Ceiling(result).ToString(CultureInfo.InvariantCulture));
                    else if (decText == "Round")
                        answerText = answerText.Replace(match.Value, MathF.Round(result).ToString(CultureInfo.InvariantCulture));
                    else if (decText == "None")
                        answerText = answerText.Replace(match.Value, result.ToString(CultureInfo.InvariantCulture));
                }
            }
            
            answers[i] = answerText;
        }
        
        
        
        question.AnswerStrings = answers;
    }




    private string SetResult(QuestionData question, string step) {
        foreach (var variable in question.Variables) {
            step = step.Replace(variable.Key, variable.Value.ToString(CultureInfo.InvariantCulture));
        }
        return step;
    }

    private void ChangeVariables(QuestionData question)
    {
        RandomStringSelector(question);

        // this code changes all the placeholders with the variables in the dictionary
        foreach (var v in variables)
        {
            question.Question = question.Question.Replace(v.Key, v.Value);
            question.Explanation = question.Explanation.Replace(v.Key, v.Value);
            question.AnswerFormule = question.AnswerFormule.Replace(v.Key, v.Value);
            question.Answers = question.Answers.Replace(v.Key, v.Value);
            
            for (int i = 0; i < question.Ranges.Length; i++) 
                question.Ranges[i] = question.Ranges[i].Replace(v.Key, v.Value);
        }
    }

    private static void RandomStringSelector(QuestionData question)
    {
        // give me the pattern for {[combien,selicon]}
        string pattern = @"\{\[(.*?)\]\}";
        MatchCollection matches = Regex.Matches(question.Question, pattern);
        // This code checks if there is a list of strings. and replaces the variable with a random value from the list. It is requested from the 
        // customer for diversity of the questions
        foreach (Match match in matches)
        {
            var split = match.Value.Trim('{').Trim('}').Trim('[').Trim(']').Split(',');
            List<string> variables = new List<string>();
            foreach (var item in split)
            {
                variables.Add(item);
            }
            var randomIndex = UnityEngine.Random.Range(0, variables.Count);
            question.Question = question.Question.Replace(match.Value, variables[randomIndex]);
        }
    }

    private static void SetParameters(QuestionData question)
    {
        // This place is for setting the parameters
        foreach (var option in question.Ranges)
        {
            float randomValue;
            var _ = option.Split(":");
            var match = _[0].Trim();
            // This part is for the list of values
            if (_[1].Contains("["))
            {
                var list = _[1].Trim().Trim('(').Trim('[').Trim(')').Trim(']');
                var numbers = list.Split(",");
                float[] intNumbers = new float[numbers.Length];
                for (int i = 0; i < numbers.Length; i++) {
                    intNumbers[i] = float.Parse(numbers[i].Trim(), CultureInfo.InvariantCulture);
                }
                randomValue = intNumbers[UnityEngine.Random.Range(0, intNumbers.Length)];
            }
            // This part is for the range of values
            else
            {
                var range = _[1].Trim().Split(",");
                var min = float.Parse(range[0].Trim('('), CultureInfo.InvariantCulture);
                var max = float.Parse(range[1], CultureInfo.InvariantCulture);
                var iter = float.Parse(range[2].Trim(')'), CultureInfo.InvariantCulture);
                int stepCount = (int)((max - min) / iter) + 1;

                int randomIndex = UnityEngine.Random.Range(0, stepCount);
                randomValue = min + randomIndex * iter;
            }
                
            question.Variables.Add(match, randomValue);
            question.ParameterCount += 1;

            
            // after setting the parameter value, we need to change the variable in the question with the value
            question.Explanation = question.Explanation.Replace("Floor" + match, MathF.Floor(randomValue).ToString(CultureInfo.InvariantCulture));
            question.Explanation = question.Explanation.Replace("Ceil" + match, MathF.Ceiling(randomValue).ToString(CultureInfo.InvariantCulture));
            question.Explanation = question.Explanation.Replace("Round" + match, MathF.Round(randomValue).ToString(CultureInfo.InvariantCulture));
                    
            question.Question = question.Question.Replace(match, randomValue.ToString(CultureInfo.InvariantCulture));
            question.AnswerFormule = question.AnswerFormule.Replace(match, randomValue.ToString(CultureInfo.InvariantCulture));
            question.Explanation = question.Explanation.Replace(match, randomValue.ToString(CultureInfo.InvariantCulture));
        }
      
        
    }

    private static string GetClockFromMinute(float value)
    {
        value = (int)value;
        // Ensure value is within 24 hours
        if (value < 0) value = 1440 + value;
        if (value == 0) return "00h00m";
         
        value = value % 1440;
        
        // Convert minutes to clock format
        string clock = "";
        int hours = (int)(value / 60);
        int minutes = (int)(value % 60);
        if (hours < 10) {
            if (minutes < 10) clock = "0" + hours + "h0" + minutes;
            else clock = "0" + hours + "h" + minutes;
        }
        else {
            if (minutes < 10) clock = hours + "h0" + minutes;
            else clock = hours + "h" + minutes;
        }

        clock += "m";
        return clock;
    }

    
    private static string GetClockFromSecond(float value, bool showSeconds)
    {
        // Ensure value is within 24 hours (86400 seconds)
        if (value < 0) value = 86400 + value;
        
        if (value == 0 && showSeconds) return "00h00m00s";
        if (value < 60 && !showSeconds) return "00h00m";

        value = value % 86400;

        // Convert seconds to clock format
        string clock = "";
        int hours = (int)(value / 3600);
        int minutes = (int)((value % 3600) / 60);
        int seconds = (int)(value % 60);

        if (hours > 0)
        {
            clock += hours < 10 ? "0" + hours + "h" : hours + "h";
        }

        if (minutes > 0 || hours > 0) // Show minutes if there are hours or minutes
        {
            clock += minutes < 10 ? "0" + minutes + "m" : minutes + "m";
            // Deplicated due to customer request
            // if (!showSeconds && (hours > 0 && minutes > 0)) clock= clock.Substring(0, clock.Length - 1);
        }

        if (showSeconds && (seconds > 0 || minutes > 0 || hours > 0)) // Show seconds if there are hours or minutes or seconds
        {
            clock += seconds < 10 ? "0" + seconds + "s" : seconds + "s";
            
            // Deplicated due to customer request   
            // if (minutes > 0 && seconds > 0) clock= clock.Substring(0, clock.Length - 1);
        }

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
        
        var data = question.Answers.Split(":");
        var dec = data[0].Split(";")[0].Trim();
        
        foreach (Match match in matches) {
            string expression = match.Groups[1].Value;
            var result = EvaluateExpression(expression);
            result = float.Parse(result.ToString("F" + dec, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            
            question.Question = question.Question.Replace(match.Value, result.ToString(CultureInfo.InvariantCulture));
            
        }

        question.Question = question.Question.Trim('\ufeff');

        
        foreach (var variable in question.Variables) {
            question.Explanation = question.Explanation.Replace("Floor" + variable.Key, MathF.Floor(variable.Value).ToString(CultureInfo.InvariantCulture));
            question.Explanation = question.Explanation.Replace("Ceil" + variable.Key, MathF.Ceiling(variable.Value).ToString(CultureInfo.InvariantCulture));
            question.Explanation = question.Explanation.Replace("Round" + variable.Key, MathF.Round(variable.Value).ToString(CultureInfo.InvariantCulture));
            
            question.Explanation = question.Explanation.Replace(variable.Key, variable.Value.ToString(CultureInfo.InvariantCulture));
        }
        
        MatchCollection matches2 = Regex.Matches(question.Explanation, pattern);
        
        foreach (Match match in matches2) {
            string expression = match.Groups[1].Value;
            var result = EvaluateExpression(expression);
            result = float.Parse(result.ToString("F" + dec, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

            question.Explanation = question.Explanation.Replace("Floor" + match.Value, MathF.Floor(result).ToString(CultureInfo.InvariantCulture));
            question.Explanation = question.Explanation.Replace("Ceil" + match.Value, MathF.Ceiling(result).ToString(CultureInfo.InvariantCulture));
            question.Explanation = question.Explanation.Replace("Round" + match.Value, MathF.Round(result).ToString(CultureInfo.InvariantCulture));
            
            question.Explanation = question.Explanation.Replace(match.Value, result.ToString(CultureInfo.InvariantCulture));
        }
        
        ChangeClocks(question);
    }

    private static void ChangeClocks(QuestionData question)
    {
        string pattern = @"hms\((.*?)\)|hm\((.*?)\)";
        MatchCollection matches = Regex.Matches(question.Question, pattern);
        
        question.Question = CalculateMatches(matches, question.Question);
        
        MatchCollection matches2 = Regex.Matches(question.Explanation, pattern);
        question.Explanation = CalculateMatches(matches2, question.Explanation);
        

        for (int i = 0; i < question.AnswerStrings.Length; i++)
        {
            var answerString = question.AnswerStrings[i];
            MatchCollection matches5 = Regex.Matches(answerString, pattern);
            question.AnswerStrings[i] = CalculateMatches(matches5, question.AnswerStrings[i]);
        }
    }

    private static string CalculateMatches(MatchCollection matches, string expression)
    {
        foreach (Match match in matches)
        {
            // Extract the value inside the brackets
            string innerValue = match.Groups[1].Value;
            bool isHms = !string.IsNullOrEmpty(innerValue);

            if (!isHms)
            {
                innerValue = match.Groups[2].Value;
            }
            innerValue = innerValue.Split(",")[0];
            // Parse the value as a float
            if (float.TryParse(innerValue, out float value))
            {
                // Convert the value to clock format
                value = float.Parse(value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                string clockFormat = GetClockFromSecond((int) value, isHms);
                expression = expression.Replace(match.Value, clockFormat.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                Debug.Log(expression);
                Debug.LogWarning("Unable to parse value inside brackets: " + innerValue);
            }
        }
        return expression;
    }

    static float EvaluateExpression(string expression)
    {
        try
        {
            expression = expression.Replace(",", ".");
            expression = expression.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
            ExpressionEvaluator.Evaluate(expression, out float res);
            return res;
        }
        catch (Exception e)
        {
            DebugManager.Instance.AddLogs("Error evaluating expression: " + expression + "\n" + e.Message);
            Debug.Log("Error evaluating expression: " + expression + "\n" + e.Message);
            throw;
        }
    }
}



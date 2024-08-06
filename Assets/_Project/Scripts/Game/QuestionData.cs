using System.Collections.Generic;

public class QuestionData
{
    public int QuestionIndex;
    public int ParameterCount = 0;
    public int AnswerCount = 0;
    
    public string Question;
    public string Explanation;
    public string Answers;
    public string AnswerFormule;
    public string Conditions;
    public string[] AnswerStrings;
    public string[] Ranges;
    public Dictionary<string, float> Variables = new();
    
}
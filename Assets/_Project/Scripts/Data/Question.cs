using Random = UnityEngine.Random;

public class Question
{
    public QuestionData questionData;
    public int correctAnswer;
    public int selectedAnswer = -1;
    
    public Result GetResult() {
        if (correctAnswer == selectedAnswer) return Result.Correct;
        if (selectedAnswer == -1) return Result.Blank;
        return Result.Wrong;
    }
    public void SetQuestion(int answerCount)
    {
        selectedAnswer = -1;
        correctAnswer = Random.Range(0, answerCount);
    }
}

public enum Result
{
    Correct,
    Wrong,
    Blank
}
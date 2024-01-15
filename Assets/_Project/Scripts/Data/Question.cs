using Random = UnityEngine.Random;

public class Question
{
    public int correctAnswer;
    public int selectedAnswer;
    
    public Result GetResult() {
        if (correctAnswer == selectedAnswer) return Result.Correct;
        if (selectedAnswer == -1) return Result.Blank;
        return Result.Wrong;
    }
    public void SetQuestion()
    {
        selectedAnswer = -1;
        correctAnswer = Random.Range(0, 3);
    }
}

public enum Result
{
    Correct,
    Wrong,
    Blank
}
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
        public static int QuestionCount;
        public static int CurrentQuestion;
        public static bool IsSolution;
        public static int AnswerCount;

        public static Question[] Questions;

        public static Question GetQuestion(int i)
        {
                if (i >= 0 && i < QuestionCount)
                        return Questions[i];
                Debug.Log("Index Out of Array");
                return null;
        }
        // Correct, Wrong, Blank
        public static int[] GetResults()
        {
                int correct = 0;
                int wrong = 0;
                int blank = 0;
                for (int i = 0; i < Questions.Length; i++)
                {
                        switch (Questions[i].GetResult())
                        {
                                case Result.Correct:
                                        correct++;
                                        break;
                                case Result.Wrong:
                                        wrong++;
                                        break;
                                case Result.Blank:
                                        blank++;
                                        break;
                                        
                        }
                }
                return  new int[3] {correct, wrong, blank};
        }
                

        public static List<bool> GetSelections()
        {
                List<bool> selections = new List<bool>();
                for (int i = 0; i < QuestionCount; i++)
                {
                        if (Questions[i].selectedAnswer == -1) selections.Add(false);
                        else selections.Add(true);
                }
                return selections;
        }

        public static Result GetResult(int i)
        {
                var question = Questions[i];
                if (question.selectedAnswer == question.correctAnswer) return Result.Correct;
                else if (question.selectedAnswer == -1) return Result.Blank;
                else return Result.Wrong;
        }

        public static void SelectQuestion(int i)
        {
                var question = Questions[CurrentQuestion];
                question.selectedAnswer = i;
        }

        public static int GetSelectedAnswerIndex()
        {
                return Questions[CurrentQuestion].selectedAnswer;
        }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class GameData
{
        public static int[] QuestionCount;
        public static int TestCount;
        public static int CurrentQuestion;
        public static int CurrentTest;
        public static bool IsSolution;
        public static int AnswerCount;
        public static List<Question[]> Questions;
        public static float[] TestTimes;
        public static float TestTime => TestTimes[CurrentTest];
        public static List<string> QuestionDatabaseUrls { get; set; }
        public static bool IsPopupLastButtonDisabled { get; set; }


        public static IEnumerator SetQuestionData(List<QuestionData> questionData, int testIndex) {
                if (DebugManager.Instance.IsDebugBuild)
                        yield return new WaitUntil(() => MathHandler.Instance.IsDataLoaded);
                for (int i = 0; i < questionData.Count; i++) {
                        var questiondata = questionData[i];
                        Questions[testIndex][i].questionData = questiondata;
                        Questions[testIndex][i].SetQuestion(questiondata.AnswerCount);
                }
        }

        public static void GetCurrentQuestionData(Action<QuestionData> callback)
        {
                CoroutineManager.Instance.StartManagedCoroutine(GetCurrentQuestionDataCoroutine(callback));
        }

        private static IEnumerator GetCurrentQuestionDataCoroutine(Action<QuestionData> callback)
        {
                if (DebugManager.Instance.IsDebugBuild && !IsSolution)
                {
                        while (!MathHandler.Instance.IsDataProcessed)
                        {
                                yield return new WaitForSeconds(0.1f); // Wait for 100 milliseconds
                        }
                }

                yield return new WaitForEndOfFrame();
                callback(Questions[CurrentTest][CurrentQuestion].questionData);
        }

        public static void GetQuestion(int i, Action<Question> callback)
        {
                CoroutineManager.Instance.StartManagedCoroutine(GetQuestionCoroutine(i, callback));
        }

        private static IEnumerator GetQuestionCoroutine(int i, Action<Question> callback)
        {
                if (DebugManager.Instance.IsDebugBuild && !IsSolution)
                {
                        while (!MathHandler.Instance.IsDataProcessed)
                        {
                                yield return new WaitForSeconds(0.1f); // Wait for 100 milliseconds
                        }
                }
                yield return new WaitForEndOfFrame();

                if (i >= 0 && i < QuestionCount[CurrentTest])
                {
                        callback(Questions[CurrentTest][i]);
                }
                else
                {
                        Debug.Log("Index Out of Array");
                        callback(null);
                }
        }

        
        // Correct, Wrong, Blank
        public static int[][] GetResults()
        { 
                int[] correct = { 0, 0, 0 };
                int[] wrong   =  { 0, 0, 0 };
                int[] blank   =  { 0, 0, 0 };
                for (int j = 0; j < Questions.Count; j++)
                {
                        var question = Questions[j];
                        for (int i = 0; i < question.Length; i++)
                        {
                                switch (question[i].GetResult())
                                {
                                        case Result.Correct:
                                                correct[j] += 1;
                                                break;
                                        case Result.Wrong:
                                                wrong[j] += 1;
                                                break;
                                        case Result.Blank:
                                                blank[j] += 1;
                                                break;
                                        
                                }
                        }
                }
                
                return new[] {correct, wrong, blank};
        }
                

        public static List<bool> GetSelections()
        {
                List<bool> selections = new List<bool>();
                for (int i = 0; i < QuestionCount[CurrentTest]; i++)
                {
                        if (Questions[CurrentTest][i].selectedAnswer == -1) selections.Add(false);
                        else selections.Add(true);
                }
                return selections;
        }

        public static Result GetResult(int i)
        {
                var question = Questions[CurrentTest][i];
                if (question.selectedAnswer == question.correctAnswer) return Result.Correct;
                else if (question.selectedAnswer == -1) return Result.Blank;
                else return Result.Wrong;
        }

        public static void SelectQuestion(int i)
        {
                var question = Questions[CurrentTest][CurrentQuestion];
                question.selectedAnswer = i;
        }

        public static void GetSelectedAnswerIndex(Action<int> callback)
        {
                CoroutineManager.Instance.StartManagedCoroutine(GetSelectedAnswerIndexCoroutine(callback));
        }

        private static IEnumerator GetSelectedAnswerIndexCoroutine(Action<int> callback)
        {
                if (DebugManager.Instance.IsDebugBuild)
                {
                        while (!MathHandler.Instance.IsDataLoaded)
                        {
                                yield return new WaitForSeconds(0.1f); // Wait for 100 milliseconds
                        }
                }
                yield return new WaitForEndOfFrame();

                int selectedAnswerIndex = Questions[CurrentTest][CurrentQuestion].selectedAnswer;
                callback(selectedAnswerIndex);
        }


        public static bool IsGameFinished() {
                if (CurrentTest == TestCount - 1) return true;
                else return false;
        }
}


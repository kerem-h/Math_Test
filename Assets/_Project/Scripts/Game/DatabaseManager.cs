using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionData
{
    public string Question;
    public string Answer;
    public string[] Options;
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;
    string database_url = "https://storage.googleapis.com/math-database/database.csv";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else {
            Destroy(this);
        }
    }

    private void Start()
    {
        StartCoroutine(GetData(database_url, (data) => {
            foreach (var questionData in data)
            {
                Debug.Log(questionData.Question);
            }
        }));
    }


    private IEnumerator GetData(string url, Action<List<QuestionData>> callback) 
    {
        var questions = new List<QuestionData>();
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
    
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error downloading file: " + request.error);
        }
        else
        {
            byte[] results = request.downloadHandler.data;
            questions = ProcessCSVFile(results);
        }
        callback?.Invoke(questions);
    }



    List<QuestionData> ProcessCSVFile(byte[] csvData)
    {
        List<QuestionData> questions = new List<QuestionData>();
        // Convert byte array to string
        string csvText = System.Text.Encoding.UTF8.GetString(csvData);
        // Split the CSV text into lines
        string[] lines = csvText.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            // Skip empty lines
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            // Parse each line into values, considering quoted cells
            var values = lines[i].Split("\t");
            QuestionData questionData = new QuestionData();
            questionData.Question = values[0];
            questionData.Answer = values[1];
            questionData.Options = values[2].Split(",");
            questions.Add(questionData);
        }
        return questions; 
    }


}

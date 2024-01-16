using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionData
{
    public string Question;
    public string Answer;
    public string[] Options;
    public string Explanation;
    public int ParameterCount = 0;
    public string FinalAnswer;
    public Dictionary<string, float> Variables = new();
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;
    string database_url = "https://storage.googleapis.com/math-database/database-example3.csv";

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
            MathHandler.Instance.questions = data;
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

        // Split the CSV text into lines, handling different line endings
        string[] lines = csvText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        for (int i = 0; i < lines.Length; i++) // Start from 1 if the first row is headers
        {
            // Skip empty lines
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            List<string> values = ParseCSVLine(lines[i]);
            if (values.Count >= 4) // Assuming at least 4 columns: Question, Answer, Options, Explanation
            {
                try
                {
                    QuestionData questionData = new QuestionData();
                    questionData.Question = values[0];
                    questionData.Answer = values[1];
                    questionData.Options = values[2].Split(';'); // Assuming options are separated by semicolons
                    questionData.Explanation = values[3];
                    questions.Add(questionData);
                }
                catch (Exception ex)
                {
                    // Handle or log the exception
                }
            }
        }
        return questions; 
    }
    List<string> ParseCSVLine(string line)
    {
        List<string> fields = new List<string>();
        bool inQuotes = false;
        StringBuilder fieldBuilder = new StringBuilder();

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }
        
            if (c == ',' && !inQuotes)
            {
                fields.Add(fieldBuilder.ToString());
                fieldBuilder.Clear();
            }
            else
            {
                fieldBuilder.Append(c);
            }
        }

        // Add the last field
        fields.Add(fieldBuilder.ToString());

        return fields;
    }


}

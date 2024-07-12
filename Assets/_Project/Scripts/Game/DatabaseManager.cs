using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// It should be selected DATAONLINE and CSVFILENAME in the inspector when the game is exported to WEBGL
public class DatabaseManager : MonoBehaviour
{
    # region Singleton
    public static DatabaseManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else {
            Destroy(this);
        }
    }
    # endregion
    
    [HideInInspector] public List<string> questionDatabaseUrls;

    public bool IsDataOnline = true;
    [SerializeField] private TextAsset csvFile;
    private void Start()
    {
        if (GameData.IsSolution) return;
        
        questionDatabaseUrls = GameData.QuestionDatabaseUrls;
        
        if (IsDataOnline) {
            StartCoroutine(GetData(questionDatabaseUrls, OnDataRecieved));
        }
        else 
        {
            var data = ProcessCSVFile(csvFile.bytes);
            MathHandler.Instance.questionsList = new List<List<QuestionData>> { data };
            if (DebugManager.Instance.IsDebugBuild)
                GameData.QuestionCount = new[] {data.Count};
            MathHandler.Instance.IsDataLoaded = true;
        }
    }

    private void OnDataRecieved(List<List<QuestionData>> data)
    {
        MathHandler.Instance.questionsList = data;

        if (data[0].Count >= 1)
        {
            if (DebugManager.Instance.IsDebugBuild)
                GameData.QuestionCount = new[] {data[0].Count};


            MathHandler.Instance.IsDataLoaded = true;
        }
    }


    IEnumerator GetData(List<string> urls, Action<List<List<QuestionData>>> callback) 
    {
        List<List<QuestionData>> questions = new();
        foreach (var url in urls)
        {

            List<QuestionData> data;
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if ((request.isNetworkError || request.isHttpError))
            {
                DebugManager.Instance.AddLogs("Error downloading file: " + request.error);
                Debug.LogError("Error downloading file: " + request.error);
                continue;
            }

            byte[] results = request.downloadHandler.data;
            data = ProcessCSVFile(results);
            questions.Add(data);
        }
        callback?.Invoke(questions);
    }


    List<QuestionData> ProcessCSVFile(byte[] csvData)
    {
        List<QuestionData> questions = new List<QuestionData>();
        string csvText = Encoding.UTF8.GetString(csvData);
        string[] lines = ParseCSVLines(csvText);

        for (int i = 0; i < lines.Length; i++)
        {
            
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            try {
                
                List<string> values = ParseCSVLine(lines[i]);
                if (values.Count >= 5) {

                        QuestionData questionData = new QuestionData();
                        questionData.QuestionIndex = i + 1;
                        questionData.Question = values[0].Trim();
                        questionData.AnswerFormule = values[1].Trim();
                        questionData.Ranges = values[2].Trim().Split(';');
                        questionData.Explanation = values[3].Trim();
                        questionData.Answers = values[4].Trim();
                        if (values.Count > 5) {
                            questionData.Conditions = values[5].Trim();
                        }
                        questions.Add(questionData); 
                }
            }
            catch (Exception ex) {
                DebugManager.Instance.AddLogs("Error parsing line " + i + ": " + ex.Message);
            }
        }
        return questions; 
    }

    private string[] ParseCSVLines(string csvText)
    {
        List<string> linesList = new List<string>();
        int quoteCount = 0;
        StringBuilder currentLine = new StringBuilder();

        for (int i = 0; i < csvText.Length; i++)
        {
            if (csvText[i] == '"')
            {
                quoteCount++;
            }

            // Add the character to the current line regardless
            currentLine.Append(csvText[i]);

            // Check if we are at a line break and not inside quotes
            if (csvText[i] == '\n' && quoteCount % 2 == 0)
            {
                // Add the accumulated line to the list and reset for the next line
                linesList.Add(currentLine.ToString());
                currentLine.Clear();
            }
        }

        // Add the last line if there's any content not followed by a newline
        if (currentLine.Length > 0)
        {
            linesList.Add(currentLine.ToString());
        }

        return linesList.ToArray();
    }


    private List<string> ParseCSVLine(string line)
    {
        var fields = new List<string>();
        var fieldBuilder = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"') inQuotes = !inQuotes;
            else if (c == ',' && !inQuotes)
            {
                fields.Add(fieldBuilder.ToString());
                fieldBuilder.Clear();
            }
            else fieldBuilder.Append(c);
        }
        fields.Add(fieldBuilder.ToString()); // Add last field

        return fields;
    }


}

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
    
    public List<string> questionDatabaseUrls;

    public bool IsDataOnline = true;
    [SerializeField] private TextAsset csvFile;
    private void Start()
    {
        if (GameData.IsSolution) return;
        
        
        if (IsDataOnline) {
            StartCoroutine(GetData(questionDatabaseUrls, OnDataRecieved));
        }
        else 
        {
            var data = ProcessCSVFile(csvFile.bytes);
            MathHandler.Instance.questionsList = new List<List<QuestionData>> { data };
        }
    }

    private void OnDataRecieved(List<List<QuestionData>> data)
    {
        MathHandler.Instance.questionsList = data;
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
                if (values.Count >= 6) {

                        QuestionData questionData = new QuestionData();
                        questionData.QuestionIndex = i + 1;
                        questionData.Question = values[0].Trim();
                        questionData.AnswerFormule = values[1].Trim();
                        questionData.Ranges = values[2].Trim().Split(';');
                        questionData.Explanation = values[3].Trim();
                        questionData.Answers = values[4].Trim();
                        questionData.ClockVariables = new List<string>(values[5].Trim().Split(";"));
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
        for (int i = 0; i < csvText.Length; i++) {
            if (csvText[i] == '"')
            {
                quoteCount++;
                if (quoteCount % 2 == 0) {
                    quoteCount = 0;
                }
            }
            if (csvText[i] == '\n' && quoteCount % 2 == 0) {
                linesList.Add(csvText.Substring(0, i));
                csvText = csvText.Substring(i + 1);
                i = 0;
            }
        }
        csvText.Trim();
        linesList.Add(csvText);
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

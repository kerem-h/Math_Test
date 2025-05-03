using System;
using TMPro;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    [Header("Texts")]

    [SerializeField] private TextMeshProUGUI correctText;
    [SerializeField] private TextMeshProUGUI wrongText;
    [SerializeField] private TextMeshProUGUI notAnsweredText;
    

    private int[] correct;
    private int[] wrong;
    private int[] blank;

    private void Start()
    {
        var results = GameData.GetResults();
        correct = results[0];
        wrong = results[1];
        blank = results[2];
        SetTexts();
    }

    private void SetTexts()
    {
        correctText.text = correct[0].ToString();
        wrongText.text = wrong[0].ToString();
        notAnsweredText.text = blank[0].ToString();
        SendData();


    }
    
    public void Solution()
    {
        GameData.IsSolution = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.GameScene.ToString());
    }
    public void Restart()
    {
        GameData.IsSolution = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scenes.GameScene.ToString());
    }

    public void SendData()
    {
        Debug.Log("Trying To Save");
        if (string.IsNullOrEmpty(MailManager.Instance.Email))
        {
            Debug.Log("Returned Empty");
            return;
        }

        Debug.Log("Calc Score");
        float testScore = (correct[0] + wrong[0] + blank[0]) > 0
            ? ((float)correct[0] / (correct[0] + wrong[0] + blank[0])) * 100f
            : 0f;

        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        // Get the test name based on the selected BuildType
        string testName = GetTestNameBasedOnBuildType();

        // Safely get elapsedTime
        float elapsed = 0f;
        if (Timer.Instance != null)
        {
            elapsed = Timer.Instance.elapsedTime;
        }

        Application.ExternalCall(
            "saveTestData",
            MailManager.Instance.Email,
            testName,
            timestamp,
            elapsed,
            testScore
        );
        Debug.Log("Made External Call");
    }

    private string GetTestNameBasedOnBuildType()
    {
        // Access the BuildType from BuildManager
        BuildManager.BuildType buildType = BuildManager.Instance.buildType;

        switch (buildType)
        {
            case BuildManager.BuildType.CSO:
                return "TAMIC -Maths";
            case BuildManager.BuildType.Tours1:
                return "EOPN - Maths 1";
            case BuildManager.BuildType.Tours2:
                return "EOPN - Maths 3";
            case BuildManager.BuildType.Eopan:
                return "EOPAN";
            case BuildManager.BuildType.Debug:
                return "DEBUG";
            case BuildManager.BuildType.Alat:
                return "ALAT";
            case BuildManager.BuildType.Suites:
                return "SUITES";
            default:
                return "Math Test";
        }
    }

}

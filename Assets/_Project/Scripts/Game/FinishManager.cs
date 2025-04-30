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
        string testName = "Math Test";

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

}
